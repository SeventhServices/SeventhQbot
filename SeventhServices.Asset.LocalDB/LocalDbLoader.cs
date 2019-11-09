using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeventhServices.Asset.LocalDB.Extensions;
using SeventhServices.Asset.LocalDB.Serializer;

namespace SeventhServices.Asset.LocalDB
{
    public class LocalDbLoader
    {

        private readonly SqlSerializer _sqlSerializer = new SqlSerializer();

        public IEnumerable<T> TryLoad<T>()
        {
            var filePath = PathExtension.GetGameSqlFileInfos()
                .First(f => string.Concat(f.Name.Split('_')[1..^1]) 
                            == $"{typeof(T).Name.ToLower()}")
                .FullName;

            return Load<T>(filePath);
        }

        public IEnumerable<T> Load<T>(string path)
        {
            return _sqlSerializer.Deserialize<T>(File.ReadAllText(path));
        }
    }
}
