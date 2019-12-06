using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading.Tasks;

namespace SeventhServices.QQRobot
{
    public class JsonFormInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var contentType = context.HttpContext.Request.ContentType;
            if (string.IsNullOrEmpty(contentType) || contentType == "text/plain" ||
                contentType == "application/octet-stream")
                return true;

            return false;
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}