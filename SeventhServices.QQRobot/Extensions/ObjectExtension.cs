using System;
using System.Linq;

namespace SeventhServices.QQRobot.Extensions
{
    public static class ObjectExtension
    {
        public static string FormatToString(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj)); ;

            var properties = obj.GetType().GetProperties();

            return string.Join("\n",
                properties.Select(p =>
                    $"[{p.Name}]:{p.GetValue(obj)}"
                ));
        }

        public static string FormatToString(this object obj, int propertyCount)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var properties = obj.GetType().GetProperties();

            return string.Join("\n",
                properties.Take(propertyCount).Select(p =>
                    $"[{p.Name}]:{p.GetValue(obj)}"
                ));
        }

        public static string FormatToString(this Type obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj)); ;

            var properties = obj.GetProperties();

            return string.Join("\n",
                properties.Select(p =>
                    $"[{p.Name}]:{p.GetValue(obj)}"
                ));
        }
    }
}