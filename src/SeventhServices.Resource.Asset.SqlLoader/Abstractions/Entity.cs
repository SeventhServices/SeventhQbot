using System.Linq;

namespace SeventhServices.Resource.Asset.SqlLoader.Abstractions
{
    public class Entity
    {
        public override string ToString()
        {
            var properties = GetType().GetProperties();

            return string.Join("\n",
                properties.Take(10).Select(p =>
                    $"[{p.Name}]:{p.GetValue(this)}"
                ));
        }
    }
}