using System.Linq;

namespace SeventhServices.Resource.Asset.SqlLoader.Extensions
{
    public static class StringExtension
    {
        // Token: 0x06003A5E RID: 14942 RVA: 0x000264E7 File Offset: 0x000246E7
        /// <summary>
        /// 扩展方法，修正命名规范 card_id => CardId
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SnakeToCamel(this string str)
        {
            return str.Split(new[]
            {
                '_'
            }).Aggregate(string.Empty, (buffer, data) 
                => buffer + char.ToUpper(data[0]) + data.Substring(1));
        }
    }
}