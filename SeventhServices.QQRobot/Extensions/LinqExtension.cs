using System;
using System.Collections.Generic;

namespace SeventhServices.QQRobot.Extensions
{
    public static class LinqExtension
    {
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                var elementValue = keySelector(element);
                if (seenKeys.Add(elementValue)) yield return element;
            }
        }
    }
}