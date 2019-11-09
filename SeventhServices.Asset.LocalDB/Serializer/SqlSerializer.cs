using System.Collections.Generic;
using SqlParse;

namespace SeventhServices.Asset.LocalDB.Serializer
{
    public class SqlSerializer
    {
        public IEnumerable<T> Deserialize<T>(string sqlString)
        {
            return new Reference().Parse<T>(sqlString);
        }
    }
}