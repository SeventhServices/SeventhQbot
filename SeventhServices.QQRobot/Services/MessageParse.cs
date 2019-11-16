using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using SeventhServices.Asset.Common.Crypt;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.Asset.Services;
using SeventhServices.Client.Common;
using SeventhServices.Client.Network.Interface;
using SeventhServices.Client.Network.Models.Extensions;
using SeventhServices.Client.Network.Models.Request;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Parser;
using SeventhServices.QQRobot.Parser.Abstractions;
using SeventhServices.QQRobot.Extensions;
using SeventhServices.QQRobot.Parser.Commands;

namespace SeventhServices.QQRobot.Services
{
    public class MessageParser
    {
        private readonly IRepository<Card> _cardRepository;
        private readonly IRepository<Character> _characterRepository;
        private readonly BindDictionary _bindDictionary;
        private readonly AssetVersionCheckService _assetVersionCheck;
        private readonly GameVersionCheckService _gameVersionCheck;
        private readonly ISeventhApiClient _apiClient;
        private readonly ICommandParser _commandParser;
        private readonly Regex intRegex = new Regex("[1-9]\\d*");


        public MessageParser(
            IRepository<Card> cardRepository,
            IRepository<Character> characterRepository,
            BindDictionary bindDictionary,
            AssetVersionCheckService assetVersionCheck,
            GameVersionCheckService gameVersionCheck,
            ISeventhApiClient apiClient,
            ICommandParser commandParser)
        {
            _cardRepository = cardRepository;
            _characterRepository = characterRepository;
            _bindDictionary = bindDictionary;
            _assetVersionCheck = assetVersionCheck;
            _gameVersionCheck = gameVersionCheck;
            _apiClient = apiClient;
            _commandParser = commandParser;
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
                return _commandParser.Parse(message,qq);
            }

            _commandParser.Add(
                new StringParsing(c => { c.CanReturn = false; })
                    .WhenStartWith(RobotOptions.Command));

            AddBindParse();

            AddClientParse();

            AddQuery();


            return _commandParser.Parse(message, qq);
        }

        private void AddClientParse()
        {
            _commandParser.Add(new StringParsing(c =>
            {
                _assetVersionCheck.TryUpdate().Wait();
                var rev = _assetVersionCheck.NowRev;
                var nowGameVersion = _gameVersionCheck.TryUpdate().Result;

                Params.Version = nowGameVersion.Version;
                Params.VersionCode = nowGameVersion.VersionCode;
                Params.Rev = rev;

                c.ReturnMessage.Add($"[VersionCode]:{Params.VersionCode}\n" +
                                    $"[Version]:{Params.Version}\n" +
                                    $"[Rev]:{Params.Rev}");

            }).WhenStartWith("检查更新"));
            _commandParser.Add(new StringParsing((c,m,q) =>
            {
                var isBind = _bindDictionary.TryGetAccount(q, out var account);
                if (isBind)
                {
                    var loginRequest = new LoginRequest();
                    loginRequest.UseAccount(account.Pid,account.Uuid);
                    var response = _apiClient.Login(loginRequest).GetAwaiter().GetResult();
                    if (response.error == null)
                    {
                        c.ReturnMessage.Add($"[{q}:{response.pid}]...\n" +
                                            $"[isRev]:{response.is_development}\n" +
                                            $"[status]:{response.login.userStatus.FormatToString()}");
                    }
                    else
                    {
                        c.ReturnMessage.Add($"[{q}:{response.pid}]...\n" +
                                            $"{response.error.errorMessage}");
                    }

                }
                else
                {
                    c.ReturnMessage.Add($"{q}未绑定，请私信团团pid/id(账号文件内)绑定哦\n" +
                                        "私信格式[团团绑定pid(这里填pid)uid(这里填id)]");
                }

            }).WhenStartWith("游戏签到"));
        }

        private void AddBindParse()
        {
            _commandParser.Add(
                new StringParsing((c, m, q) =>
                {
                    var pidIndex = m.IndexOf("pid", StringComparison.Ordinal);
                    var idIndex = m.IndexOf("uid", StringComparison.Ordinal);
                    var pid = m.Substring(pidIndex + 3, idIndex - pidIndex - 3);
                    var id = m.Substring(idIndex + 3);
                    _bindDictionary.TryAddAccount(q, pid, id);
                    c.ReturnMessage.Add($"Added\n" +
                                                    $"[pid] : {pid}\n" +
                                                    $"[id] : {id}");
                    _bindDictionary.TryGetAccount(q, out var account);
                    if (account != null)
                    {
                        c.ReturnMessage.Add($"Got\n" +
                                                        $"[pid] : {account.Pid}\n" +
                                                        $"[id] : {account.Uuid}");
                        var inspection = new InspectionRequest();
                        inspection.UseAccount(account.Pid, account.Uuid);
                        var inspectionResponse = _apiClient.Inspection(inspection).GetAwaiter().GetResult();
                        c.ReturnMessage.Add($"[pid];{inspectionResponse.pid}\n" +
                                                        $"[isDev];{inspectionResponse.is_development}");
                    }
                    else
                    {
                        c.ReturnMessage.Add("Got failed");
                    }

                })
                   .WhenStartWith("绑定"));

            _commandParser.Add(
                new StringParsing((c,m,q) =>
                {
                    c.ReturnMessage.Add(_bindDictionary.RemoveAccount(q).ToString(CultureInfo.CurrentCulture));
                })
                    .WhenStartWith("取消绑定"));
        }

        private void AddQuery()
        {
            _commandParser.Add(
    new StringParsing(c => { c.CanReturn = false; })
        .WhenStartWith("查"));

            _commandParser.Add(
                new StringParsing((c, m) =>
                {
                    var characterId = _characterRepository.FuzzyGetByName(m.Trim())?.CharacterId;
                    var indexMessage = _cardRepository.GetByCharacterId(characterId ?? 1)
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
                    var useId = TryGetNum(m, out var id);
                    c.ReturnMessage.Add(useId
                        ? _characterRepository.GetById(id).ToString()
                        : _characterRepository.FuzzyGetByName(m).ToString());
                }).WhenStartWith("偶像"));

            _commandParser.Add(
                new StringParsing((c, m) =>
                {
                    var id = int.Parse(intRegex.Match(m).Value, CultureInfo.CurrentCulture);
                    c.ReturnMessage.Add(
                        ProcessMessageUtils.FilterSendPic(
                            $"http://qbot.sagilio.net:65321/Card/l/card_l_{id:D5}.jpg"));
                }).WhenStartWith("卡图"));

            _commandParser.Add(
                new StringParsing((c, m) =>
                {
                    var id = int.Parse(intRegex.Match(m).Value, CultureInfo.CurrentCulture);
                    c.ReturnMessage.Add(
                        _cardRepository.GetById(id).ToString());
                }).WhenStartWith("卡数据"));
        }

        private bool TryGetNum(string message, out int num)
        {
            var match = intRegex.Match(message);
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