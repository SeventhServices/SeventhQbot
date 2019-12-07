using System;
using System.Text.Json;
using WebApiClient;

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