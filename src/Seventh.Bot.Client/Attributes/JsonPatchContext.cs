using System;
using System.Text.RegularExpressions;
using Seventh.Bot.Client.Extensions;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace Seventh.Bot.Client.Attributes
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