using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SeventhServices.QQRobot.Client.Extensions;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace SeventhServices.QQRobot.Client.Attributes
{
    public class JsonPatchContextAttribute : JsonContentAttribute
    {
        protected override void SetHttpContent(ApiActionContext context, ApiParameterDescriptor parameter)
        {
            if (context == null)
            {
                throw new NullReferenceException();
            }
            var jsonFormatter = context.HttpApiConfig.JsonFormatter;
            var obj = parameter?.Value;
            var json = jsonFormatter.Serialize(obj, null);
            context.RequestMessage.Content = new JsonPatchContext(Regex.Unescape(json), System.Text.Encoding.UTF8);
        }
    }
}