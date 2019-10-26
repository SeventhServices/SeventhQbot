using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.VisualBasic.CompilerServices;
using SeventhServices.QQRobot.Client.Extensions;
using WebApiClient;
using WebApiClient.Defaults;

namespace SeventhServices.QQRobot.Client.Formats
{
    public class JsonTextFormatter : IJsonFormatter
    {

        public string Serialize(object obj, FormatOptions options)
        {
            return JsonSerializer.Serialize(obj);
        }

        public object Deserialize(string json, Type objType)
        {

            return JsonSerializer.Deserialize(json, objType);
        }
    }
}