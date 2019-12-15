using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SeventhServices.Asset.Common.Classes;
using SeventhServices.Asset.Common.Enums;
using SeventhServices.Asset.Common.Utilities;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.Asset.Services;
using SeventhServices.Client.Common.Enums;
using SeventhServices.Client.Common.Params;
using SeventhServices.Client.Network.Interfaces;
using SeventhServices.Client.Network.Models.Extensions;
using SeventhServices.Client.Network.Models.Request;
using SeventhServices.Client.Network.Models.Request.Event;
using SeventhServices.Client.Network.Models.Request.Friend;
using SeventhServices.Client.Network.Models.Request.Present;
using SeventhServices.Client.Network.Models.Response;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Common;
using SeventhServices.QQRobot.Parser;
using SeventhServices.QQRobot.Parser.Abstractions;
using SeventhServices.QQRobot.Extensions;
using SeventhServices.QQRobot.Resource.Entities;
using SeventhServices.QQRobot.Resource.Repositories;
using SeventhServices.QQRobot.Utils;
using WebApiClient;


namespace SeventhServices.QQRobot.Services
{
    public class MessageParser
    {
        private readonly IRepository<Card> _cardRepository;
        private readonly IRepository<Character> _characterRepository;
        private readonly BindRepository _bindRepository;
        private readonly AssetVersionCheckService _assetVersionCheck;
        private readonly GameVersionCheckService _gameVersionCheck;
        private readonly ISeventhApiClient _apiClient;
        private readonly ICommandParser _commandParser;
        private readonly StatusService _statusService;
        private readonly Regex _intRegex = new Regex("[1-9]\\d*");

        public MessageParser(
            IRepository<Card> cardRepository,
            IRepository<Character> characterRepository,
            BindRepository bindRepository,
            AssetVersionCheckService assetVersionCheck,
            GameVersionCheckService gameVersionCheck,
            ISeventhApiClient apiClient,
            ICommandParser commandParser,
            StatusService statusService)
        {
            _cardRepository = cardRepository;
            _characterRepository = characterRepository;
            _bindRepository = bindRepository;
            _assetVersionCheck = assetVersionCheck;
            _gameVersionCheck = gameVersionCheck;
            _apiClient = apiClient;
            _commandParser = commandParser;
            _statusService = statusService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public MessageCommand Parse(string message, string qq)
        {
            if (message == null)
            {
                return null;
            }

            if (_commandParser.Any())
            {
                return _commandParser.Parse(message, qq);
            }

            _commandParser.Add(
                new StringParsing(c =>
                    {
                        RequestParams.Version = _statusService.GameVersion.Version;
                        RequestParams.Pid = _statusService.Account.Pid;
                        RequestParams.Uuid = _statusService.Account.Uuid;
                        RequestParams.Rev = _statusService.Rev;
                        c.Continue = true;
                    })
                    .WhenStartWith(RobotOptions.Command)
                    .BreakOnFailed(true));

            AddBindParse();
            AddClientParse();
            AddSetParse();
            AddQuery();

            return _commandParser.Parse(message, qq);
        }

        private void AddClientParse()
        {
            _commandParser.Add(new StringParsing(c =>
            {
                var nowGameVersion = _gameVersionCheck.TryUpdate(false, GameVersionCheckSource.QooApp).Result;
                var rev = _assetVersionCheck.TryUpdate().Result;

                RequestParams.Version = nowGameVersion.Version;
                RequestParams.Rev = rev;

                var robotStatus = ConfigureWatcher.GetFreshConfigure<RobotStatus>();
                robotStatus.DownloadConfig =
                    _apiClient.Inspection(new InspectionRequest())
                    .GetAwaiter().GetResult().Inspection.DownloadConfig;

                var myPageResult = _apiClient.MyPage(new MyPageRequest { SubRev = robotStatus.SubRev }).GetAwaiter().GetResult();

                if (myPageResult.CheckError(c))
                {
                    return;
                }

                robotStatus.SubRev = myPageResult.MyPage.SubRev;

                c.ReturnMessage.Add($"[Version]:{RequestParams.Version}\n" +
                                    $"[Rev]:{RequestParams.Rev}\n" +
                                    $"[SubRev] : {robotStatus.SubRev}\n" +
                                    robotStatus.DownloadConfig.FormatToString());

                ConfigureWatcher.RefreshConfigure<RobotStatus>(robotStatus);

            }).WhenStartWith("检查更新"));

            _commandParser.Add(new StringParsing((c, m, q) =>
            {

                var binding = _bindRepository.GetOneByQq(q);

                if (!binding.CheckBinding(c, q))
                {
                    return;
                }

                var loginRequest = new LoginRequest();
                loginRequest.UseAccount(binding.BoundAccount.Pid, binding.BoundAccount.Uuid);
                var loginResult = _apiClient.Login(loginRequest).GetAwaiter().GetResult();

                if (loginResult.CheckError(c))
                {
                    return;
                }

                c.ReturnMessage.Add($"[{q}:登录成功" +
                                    $"[isRev]:{loginResult.IsDevelopment}\n" +
                                    $"[Profile]:{loginResult.Login.UserStatus.UserId}/{loginResult.Login.UserStatus.UserName}/{loginResult.Login.UserStatus.UserCode}]" +
                                    $"{ProcessMessageUtils.LargeCardPic(loginResult.Login.UserStatus.ProfileCardId)}");

                var myPageRequest = new MyPageRequest();
                myPageRequest.UseAccount(binding.BoundAccount.Pid, binding.BoundAccount.Uuid);
                var myPageResult = _apiClient.MyPage(myPageRequest).GetAwaiter().GetResult();

                if (myPageResult.CheckError(c))
                {
                    return;
                }

                c.ReturnMessage.Add($"[{q}:签到成功" +
                                    $"[isRev]:{myPageResult.IsDevelopment}\n" +
                                    $"[SubRev] : {myPageResult.MyPage.SubRev}\n [LoginBonus] : \n" +
                                    myPageResult.MyPage.LoginBonusList.FormatLoginBonus());

            }).WhenStartWith("游戏签到"));

            _commandParser.Add(new StringParsing((c, m, q) =>
            {

                var binding = _bindRepository.GetOneByQq(q);

                if (!binding.CheckBinding(c, q))
                {
                    return;
                }

                var presentMainRequest = new PresentMainRequest();
                presentMainRequest.UseAccount(binding.BoundAccount.Pid, binding.BoundAccount.Uuid);
                var result = _apiClient.PresentMain(presentMainRequest).GetAwaiter().GetResult();

                if (result.CheckError(c))
                {
                    return;
                }

                c.ReturnMessage.Add($"[{q}:Presents" +
                                    $"[isRev]:{result.IsDevelopment}\n" +
                                    string.Join("\n", result.PresentBox.UserPresentList.Take(3).Select(p => p.FormatToString())));

            }).WhenStartWith("游戏礼物"));

            _commandParser.Add(new StringParsing(c => { c.ReturnMessage.Add("等待更新..."); })
                .WhenStartWith("更新活动类型"));
        }

        private void AddSetParse()
        {
            _commandParser.Add(new StringParsing((c, m) =>
                {
                    c.ReturnMessage.Add(typeof(RequestParams).FormatToString());
                })
                .WhenStartWith("当前请求参数"));

            _commandParser.Add(new StringParsing((c, m) =>
                {
                    c.ReturnMessage.Add(ConfigureWatcher.GetFreshConfigure<RobotStatus>().FormatToString());
                })
                .WhenStartWith("当前状态参数"));

            _commandParser.Add(new StringParsing((c, m) =>
                {
                    c.ReturnMessage.Add(typeof(MemoryCache).FormatToString());
                })
                .WhenStartWith("当前缓存状态"));

            _commandParser.Add(new StringParsing((c, m, q) =>
                {
                    var binding = _bindRepository.GetByQq(q).ToArray();

                    if (binding.Any())
                    {
                        c.ReturnMessage.Add(string.Join("\n",
                            binding.Select(b => b.FormatToString(4))));
                    }
                    else
                    {
                        c.ReturnMessage.Add($"{q}未绑定，请私信团团pid/id(账号文件内)绑定哦\n" +
                                            "私信格式 [ 团团绑定 pid (这里填pid) uid (这里填id) ]");
                    }
                })
                .WhenStartWith("当前绑定状态"));

            _commandParser.Add(new StringParsing((c, m) =>
                {
                    c.ReturnMessage.Add(string.Join("\n",
                        Enum.GetValues(typeof(OpenEventType))
                            .Cast<int>()
                            .Select(v => v == (int)ConfigureWatcher.GetFreshConfigure<RobotStatus>().OpenEventType
                                ? $"[{v}] : {Enum.GetName(typeof(OpenEventType), v)}"
                                : $" {v}  : {Enum.GetName(typeof(OpenEventType), v)}"
                            )));
                })
                .WhenStartWith("当前活动类型"));

            _commandParser.Add(new StringParsing((c, m) =>
            {
                var paramExisted = TryGetNum(m, out var param);
                if (paramExisted && Enum.IsDefined(typeof(OpenEventType), param))
                {
                    var robotStatus = ConfigureWatcher.GetFreshConfigure<RobotStatus>();

                    robotStatus.OpenEventType = (OpenEventType)param;
                    c.ReturnMessage.Add(robotStatus.OpenEventType.ToString());

                    ConfigureWatcher.RefreshConfigure<RobotStatus>(robotStatus);
                }
                else
                {
                    c.ReturnMessage.Add("Failed");
                }
            }).WhenStartWith("设置活动类型"));
        }

        private void AddBindParse()
        {
            _commandParser.Add(new StringParsing((c, m, q) =>
            {
                var result = TryGetNum(m, out var pid);
                if (!result)
                {
                    c.ReturnMessage.Add("团团想要一个正确的Pid!");
                }
                try
                {
                    var addResult = _bindRepository.Add(new AccountBinding
                    {
                        Qq = q,
                        BindTime = DateTime.Now,
                        IsPidOnly = true,
                        BoundAccountPid = pid.ToString(CultureInfo.CurrentCulture),
                        BoundAccount = new BoundAccount(q, m.Trim(), string.Empty, false)
                    });

                    if (addResult != null)
                    {
                        c.ReturnMessage.Add($"{q} : {addResult.BoundAccountPid} 绑定成功");
                    }
                }
                catch (Exception e)
                {
                    c.ReturnMessage.Add($"{q}已经绑定,{e.Message}");
                }

            }).WhenStartWith("仅pid绑定"));

            _commandParser.Add(new StringParsing((c, m, q) =>
            {
                var pidIndex = m.IndexOf("pid", StringComparison.Ordinal);
                var idIndex = m.IndexOf("uid", StringComparison.Ordinal);
                var pid = m.Substring(pidIndex + 3, idIndex - pidIndex - 3).Trim();
                var id = m.Substring(idIndex + 3).Trim();
                try
                {
                    var addResult = _bindRepository.Add(new AccountBinding
                    {
                        Qq = q,
                        BindTime = DateTime.Now,
                        BoundAccountPid = pid,
                        BoundAccount = new BoundAccount(q, pid, id),
                    });
                    if (addResult == null)
                    {
                        return;
                    }

                    c.ReturnMessage.Add($"{q}添加成功\n" +
                                        $"[pid] : {pid}\n" +
                                        $"[id] : {id}");
                    var inspection = new InspectionRequest();
                    inspection.UseAccount(addResult.BoundAccount.Pid, addResult.BoundAccount.Uuid);
                    var result = _apiClient.Inspection(inspection).GetAwaiter().GetResult();

                    if (result.CheckError(c))
                    {
                        return;
                    }

                    c.ReturnMessage.Add($"[pid];{result.Pid}\n" +
                                        $"[isDev];{result.IsDevelopment}\n" +
                                        $"{result.Inspection.DownloadConfig.FormatToString()}");
                }
                catch (Exception e)
                {
                    c.ReturnMessage.Add(e.Message);
                }
            }).WhenStartWith("绑定"));

            _commandParser.Add(new StringParsing((c, m, q) =>
            {
                var pidIndex = m.IndexOf("pid", StringComparison.Ordinal);
                var idIndex = m.IndexOf("uid", StringComparison.Ordinal);
                var ivsIndex = m.IndexOf("ivs", StringComparison.Ordinal);
                var pid = m.Substring(pidIndex + 3, idIndex - pidIndex - 3).Trim();
                var id = m.Substring(idIndex + 3, ivsIndex - idIndex - 3).Trim();
                var ivs = m.Substring(ivsIndex + 3).Trim();
                try
                {
                    var addResult = _bindRepository.Add(new AccountBinding
                    {
                        Qq = q,
                        BindTime = DateTime.Now,
                        BoundAccountPid = pid,
                        BoundAccount = new BoundAccount(q, pid, id, ivs)
                    });
                    if (addResult == null)
                    {
                        return;
                    }

                    c.ReturnMessage.Add($"{q}添加成功\n" +
                                        $"[pid] : {pid}\n" +
                                        $"[id] : {id}");
                    var inspection = new InspectionRequest();
                    inspection.UseAccount(addResult.BoundAccount.Pid, addResult.BoundAccount.Uuid);
                    var result = _apiClient.Inspection(inspection).GetAwaiter().GetResult();

                    if (result.CheckError(c))
                    {
                        return;
                    }

                    c.ReturnMessage.Add($"[pid];{result.Pid}\n" +
                                        $"[isDev];{result.IsDevelopment}\n" +
                                        $"{result.Inspection.DownloadConfig.FormatToString()}");
                }
                catch (Exception e)
                {
                    c.ReturnMessage.Add(e.Message);
                }
            }).WhenStartWith("跨端绑定"));

            _commandParser.Add(new StringParsing((c, m, q) =>
            {
                try
                {
                    _bindRepository.RemoveAll(q);
                }
                catch (Exception e)
                {
                    c.ReturnMessage.Add($"取消失败{e.Message}");
                    return;
                }

                c.ReturnMessage.Add("已取消绑定");
            }).WhenStartWith("取消全部绑定"));
        }

        private void AddQuery()
        {
            _commandParser.Add(new StringParsing(c =>
               {
                   c.Continue = true;
               })
            .WhenStartWith("查"));

            _commandParser.Add(new StringParsing((c, m) =>
           {
               var result = _apiClient.FriendSearch(new FriendSearchRequest
               {
                   SearchParam = m.Trim()
               }).GetAwaiter().GetResult();

               if (result.CheckError(c))
               {
                   return;
               }

               if (result.FriendSearch.SearchList == null || !result.FriendSearch.SearchList.Any())
               {
                   c.ReturnMessage.Add($"团团没有找到这位支配人…");
                   return;
               }
               c.ReturnMessage.Add(string.Join("\n",
                   result.FriendSearch.SearchList.Take(5).Select(f =>
                       $"[{f.UserName}] : lv.{f.Level} \n" +
                       $"[Pid] : {f.UserId}\n" +
                       $"{f.Comment}"
                   )));

           }).WhenStartWith("支配人"));

            _commandParser.Add(new StringParsing(((c, m) =>
            {
                var paramExisted = TryGetNum(m, out var maxRank);
                if (paramExisted)
                {
                    var result = _apiClient.EventRankingUser(new EventRankingUserRequest
                    { EventType = ConfigureWatcher.GetFreshConfigure<RobotStatus>().OpenEventType, MaxRank = maxRank }).GetAwaiter().GetResult();

                    if (result.CheckError(c))
                    {
                        return;
                    }

                    c.ReturnMessage.Add(result.FormatRanking(DateTime.Now, 7));
                    return;
                }

                if (DateTime.Now - ConfigureWatcher.GetFreshConfigure<RobotStatus>().LastEventBorderDateTime < TimeSpan.FromMinutes(3)
                    && MemoryCache.EventRankingCache != null)
                {
                    c.ReturnMessage.Add(MemoryCache.EventRankingCache);
                }
                else
                {
                    var robotStatus = ConfigureWatcher.GetFreshConfigure<RobotStatus>();
                    var result = _apiClient.EventRankingUser(new EventRankingUserRequest
                    { EventType = robotStatus.OpenEventType }).GetAwaiter().GetResult();

                    if (result.CheckError(c))
                    {
                        return;
                    }

                    robotStatus.LastEventBorderDateTime = DateTime.Now;
                    MemoryCache.EventRankingCache = result.FormatBorder(robotStatus.LastEventBorderDateTime);
                    ConfigureWatcher.RefreshConfigure<RobotStatus>(robotStatus);
                    c.ReturnMessage.Add(MemoryCache.EventRankingCache);
                }
            })).WhenStartWith("档线"));

            _commandParser.Add(new StringParsing(((c, m) =>
            {
                var paramExisted = TryGetNum(m, out var maxRank);
                if (paramExisted)
                {
                    var maxRankResult = _apiClient.EventRankingUser(new EventRankingUserRequest
                    {
                        EventType = ConfigureWatcher.GetFreshConfigure<RobotStatus>().OpenEventType,
                        RankingType = RankingCategory.HighScoreRanking,
                        MaxRank = maxRank
                    }).GetAwaiter().GetResult();

                    if (maxRankResult.CheckError(c))
                    {
                        return;
                    }

                    c.ReturnMessage.Add(maxRankResult.FormatRanking(DateTime.Now, 7));
                    return;
                }

                var robotStatus = ConfigureWatcher.GetFreshConfigure<RobotStatus>();
                var result = _apiClient.EventRankingUser(new EventRankingUserRequest
                { EventType = robotStatus.OpenEventType, RankingType = RankingCategory.HighScoreRanking }).GetAwaiter().GetResult();

                if (result.CheckError(c))
                {
                    return;
                }

                c.ReturnMessage.Add(result.FormatBorder(DateTime.Now));

            })).WhenStartWith("高分档线"));

            _commandParser.Add(new StringParsing(((c, m, q) =>
            {
                var bind = _bindRepository.GetOneByQq(q);
                if (bind != null)
                {
                    var pid = Convert.ToInt32(bind.BoundAccountPid, CultureInfo.CurrentCulture);
                    var result = _apiClient.EventRankingUser(new EventRankingUserRequest
                    {
                        EventType = ConfigureWatcher.GetFreshConfigure<RobotStatus>().OpenEventType,
                        PickupUserId = pid
                    }).GetAwaiter().GetResult();

                    if (result.CheckError(c))
                    {
                        return;
                    }

                    c.ReturnMessage.Add(result.FormatRankingWithPickupPid(DateTime.Now, pid, 7)
                        ?? "团团没找到你的排名...是不是你这次活动摸了！");
                }
                else
                {
                    c.ReturnMessage.Add($"{q}未绑定，请私信团团pid/id(账号文件内)绑定哦\n" +
                                        "私信格式 [ 团团绑定 pid (这里填pid) uid (这里填id) ]");
                }
            })).WhenStartWith("我的档线"));

            _commandParser.Add(new StringParsing(((c, m, q) =>
            {
                var bind = _bindRepository.GetOneByQq(q);
                if (bind != null)
                {
                    var pid = Convert.ToInt32(bind.BoundAccountPid, CultureInfo.CurrentCulture);
                    var result = _apiClient.EventRankingUser(new EventRankingUserRequest
                    {
                        EventType = ConfigureWatcher.GetFreshConfigure<RobotStatus>().OpenEventType,
                        RankingType = RankingCategory.HighScoreRanking,
                        PickupUserId = pid
                    }).GetAwaiter().GetResult();

                    if (result.CheckError(c))
                    {
                        return;
                    }

                    c.ReturnMessage.Add(result.FormatRankingWithPickupPid(DateTime.Now, pid, 7)
                                        ?? "团团没找到你的排名...是不是你这次活动摸了！");
                }
                else
                {
                    c.ReturnMessage.Add($"{q}未绑定，请私信团团pid/id(账号文件内)绑定哦\n" +
                                        "私信格式 [ 团团绑定 pid (这里填pid) uid (这里填id) ]");
                }
            })).WhenStartWith("我的高分档线"));

            _commandParser.Add(
                new StringParsing((c, m) =>
                {
                    var character = _characterRepository.FuzzyGetByName(m.Trim());
                    if (character == null)
                    {
                        c.ReturnMessage.Add("团团没有到这位偶像…当然卡就更没找到了……");
                        return;
                    }

                    var indexMessage = _cardRepository.GetByCharacterId(character.CharacterId)
                        .Distinct(card => card.CardName)
                        .Select((card, index) => new { index = index / 10, card })
                        .GroupBy(i => i.index)
                        .Select(grouping =>
                            grouping.Select(arg => $"[{arg.card.CardId}] : {arg.card.CardName}"))
                        .Select(s => string.Join("\n", s));
                    c.ReturnMessage.AddRange(indexMessage);
                }).WhenStartWith("偶像卡"));

            _commandParser.Add(
                new StringParsing((c, m) =>
                {
                    var result = TryGetNum(m, out var id);
                    c.ReturnMessage.Add(result
                        ? _characterRepository.GetById(id).ToString()
                        : _characterRepository.FuzzyGetByName(m.Trim()).ToString());
                }).WhenStartWith("偶像"));

            _commandParser.Add(
                new StringParsing((c, m) =>
                {
                    var result = TryGetNum(m, out var id);
                    if (!result)
                    {
                        c.ReturnMessage.Add("团团需要你提供卡的id!(毕竟团团目前还只会这么找x)");
                        return;
                    }
                    c.ReturnMessage.Add(ProcessMessageUtils.LargeCardPic(id));
                }).WhenStartWith("卡图"));

            _commandParser.Add(
                new StringParsing((c, m) =>
                {
                    var result = TryGetNum(m, out var id);
                    if (!result)
                    {
                        c.ReturnMessage.Add("团团需要你提供卡的id!(毕竟团团目前还只会这么找x)");
                        return;
                    }
                    c.ReturnMessage.Add(
                        _cardRepository.GetById(id).ToString());
                }).WhenStartWith("卡数据"));
        }

        private bool TryGetNum(string message, out int num)
        {
            var match = _intRegex.Match(message);
            if (match.Success)
            {
                num = int.Parse(match.Value, CultureInfo.CurrentCulture);
                return true;
            }

            num = 0;
            return false;
        }

    }
}