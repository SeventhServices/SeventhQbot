using System;
using System.Collections.Generic;
using System.Reflection;
using Seventh.Resource.Asset.SqlLoader.Extensions;

namespace Seventh.Resource.Asset.SqlLoader.Serializer
{
    public static class SqlSerializer
    {
        public static IEnumerable<T> Deserialize<T>(string sqlString)
        {
            return new SqlParser<T>().Parse(sqlString);
        }

        public static SqlParser<T> GetParser<T>()
        {
            return new SqlParser<T>();
        }
    }
}