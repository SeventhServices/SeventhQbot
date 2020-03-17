using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Seventh.Resource.Asset.SqlLoader.Extensions
{
    public static class PathExtension
    {
        public static string Local { get; set; } = Path.GetDirectoryName(Assembly.GetAssembly(typeof(PathExtension)).Location);

        public static string GetGameSqlDirectory()
        {
            return Path.Combine(Local, "GameSql");
        }

        public static IEnumerable<FileInfo> GetGameSqlFileInfos()
        {
            return new DirectoryInfo(GetGameSqlDirectory()).GetFiles();
        }
    }
}