using System.Collections.Generic;
using System.IO;
using System.Linq;
using Seventh.Resource.Asset.SqlLoader.Extensions;
using Seventh.Resource.Asset.SqlLoader.Serializer;

namespace Seventh.Resource.Asset.SqlLoader
{
    public class LocalDbLoader
    {
        public IEnumerable<T> TryLoad<T>()
        {
            var filePath = PathExtension.GetGameSqlFileInfos()
                .FirstOrDefault(f => string.Concat(f.Name.Split('_')[1..^1]) 
                            == $"{typeof(T).Name.ToLower()}")?.FullName;

            return filePath == null ? null : Load<T>(filePath);
        }

        public static IEnumerable<T> Load<T>(string path)
        {
            return SqlSerializer.Deserialize<T>(File.ReadAllText(path));
        }
    }
}
