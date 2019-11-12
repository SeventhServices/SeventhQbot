using System.Linq;

namespace SeventhServices.Asset.LocalDB.Abstractions
{
    public class Entity
    {
        public override string ToString()
        {
            var properties = GetType().GetProperties();

            return string.Join("\n",
                properties.Take(5).Select(p =>
                    $"[{p.Name}]:{p.GetValue(this)}"
                ));
        }
    }
}