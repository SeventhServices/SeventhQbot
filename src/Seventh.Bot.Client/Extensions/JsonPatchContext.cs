using System.Net.Http;
using System.Text;

namespace Seventh.Bot.Client.Extensions
{
    class JsonPatchContext : StringContent
    {
        /// <summary>获取对应的ContentType</summary>
        public static string MediaType => 
            "application/json-patch+json";

        /// <summary>http请求的json内容</summary>
        /// <param name="json">json内容</param>
        /// <param name="encoding">编码</param>
        public JsonPatchContext(string json, Encoding encoding)
            : base(json ?? string.Empty, encoding, JsonPatchContext.MediaType)
        {
        }
    }
}
