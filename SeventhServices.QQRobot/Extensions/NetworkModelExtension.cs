using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using SeventhServices.Client.Common.Params;
using SeventhServices.Client.Network.Models.Response;
using SeventhServices.Client.Network.Models.Response.Event;
using SeventhServices.QQRobot.Parser.Abstractions;

namespace SeventhServices.QQRobot.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetworkModelExtension
    {
        public static bool CheckError(this ApiResult result, MessageCommand command)
        {
            if (result?.Error != null)
            {
                command?.ReturnMessage.Add($"[{result.Error.ErrorCode}] : {result.Error.ErrorMessage}\n" +
                                    $"[DEBUG]:{typeof(RequestParams).FormatToString()}");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="dateTime"></param>
        /// <param name="pickupPid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string FormatRankingWithPickupPid(this EventRankingUserResponse response, DateTime dateTime, int pickupPid, int count = 7)
        {
            var formatRanking = FormatRankingWithPickupPid(response, pickupPid, count);
            if (formatRanking == null)
            {
                return null;
            }
            return string.Concat($"[Update Time] : {dateTime.ToString(CultureInfo.CurrentCulture)} \n\n", formatRanking);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="dateTime"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string FormatRanking(this EventRankingUserResponse response, DateTime dateTime, int count = 7)
        {
            return string.Concat($"[Update Time] : {dateTime.ToString(CultureInfo.CurrentCulture)} \n\n", FormatRanking(response,count));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="pickupPid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string FormatRankingWithPickupPid(this EventRankingUserResponse response, int pickupPid ,int count = 7)
        {
            if (count <= 0 || count >= 50)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (response?.EventRankingUser == null)
            {
                return string.Empty;
            }

            var index = response.EventRankingUser.UserRanking.EventParticipants.Select((r,i) => new {r.UserId,i}).FirstOrDefault(r => r.UserId == pickupPid);

            if (index == null)
            {
                return null;
            }

            var maxRange = count / 2;
            Range range;
            if (index.i >= 49 - maxRange)
            {
                range = new Range(50 - count, 50);
            }
            else if (index.i <= 0 + maxRange)
            {
                range = new Range(0, count);
            }
            else
            {
                range = new Range(index.i - maxRange, index.i + maxRange + 1);
            }

            return string.Join("\n",
                response.EventRankingUser.UserRanking.EventParticipants[range]
                    .Select(b => $"[{b.Rank}位] : {b.Score}pt \n" +
                                 $"[Level / Name / id] \n:lv.{b.Level} / {b.UserName} / {b.UserId}"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string FormatRanking(this EventRankingUserResponse response, int count = 7)
        {
            if (response?.EventRankingUser == null)
            {
                return string.Empty;
            }

            return string.Join("\n",
                response.EventRankingUser.UserRanking.EventParticipants.Take(count)
                    .Select(b => $"[{b.Rank}位] : {b.Score}pt \n" +
                                 $"[Level/Name/id] :{b.Level}/{b.UserName}/{b.UserId}"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatBorder(this EventRankingUserResponse response, DateTime dateTime)
        {
            return string.Concat($"[Update Time] : {dateTime.ToString(CultureInfo.CurrentCulture)} \n\n", FormatBorder(response));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string FormatBorder(this EventRankingUserResponse response)
        {
            if (response?.EventRankingUser == null)
            {
                return string.Empty;
            }

            return string.Join("\n", 
                response.EventRankingUser.BorderList
                    .Select( b => $"[{b.BorderRank}位] : {b.Border}pt"));
        }
    }
}