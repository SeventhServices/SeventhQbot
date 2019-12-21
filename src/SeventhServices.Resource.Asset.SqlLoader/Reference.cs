using System;
using System.Collections.Generic;
using System.Reflection;
using SeventhServices.Resource.Asset.SqlLoader.Extensions;

namespace SqlParse
{
    public class Reference
    {
        // Token: 0x06005A23 RID: 23075 RVA: 0x0010202C File Offset: 0x0010022C
        /// <summary>
        /// 分析 sql 字符串
        /// </summary>
        /// <typeparam name="T">sql对应的类</typeparam>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public IEnumerable<T> Parse<T>(string sqlString) 
        {
            data = sqlString;
            index = data.IndexOf('(');
            while (data[index] != ')')
            {
                headers.Add(typeof(T).GetProperty(
                    getNextValue().SnakeToCamel()));
            }
            for (; ; )
            {
                while (data[index] != '(')
                {
                    index++;
                    if (index == data.Length)
                    {
                        goto Break;
                    }
                }
                if (data[index + 1] == '`')
                {
                    index++;
                }
                else
                {
                    yield return getNextObject<T>();
                }
            }
            Break:
            yield break;
        }

        // Token: 0x06005A24 RID: 23076 RVA: 0x00102058 File Offset: 0x00100258
        /// <summary>
        /// 获取开头的属性的下一个值
        /// </summary>
        /// <returns></returns>
        private string getNextValue()
        {
            index++;
            char c = data[index];
            string text = string.Empty;
            index++;
            for (; ; )
            {
                if (data[index] == c)
                {
                    index++;
                    if (data[index] != c)
                    {
                        break;
                    }
                }
                text += data[index];
                index++;
            }
            return text;
        }

        // Token: 0x06005A25 RID: 23077 RVA: 0x0010210C File Offset: 0x0010030C
        /// <summary>
        /// 获取下一组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T getNextObject<T>()
        {
            T t = Activator.CreateInstance<T>();
            foreach (PropertyInfo propertyInfo in headers)
            {
                string nextValue = getNextValue();
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        propertyInfo.SetValue(t, int.Parse(nextValue), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(float))
                    {
                        propertyInfo.SetValue(t, float.Parse(nextValue), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(t, nextValue, null);
                    }
                }
            }
            return t;
        }

        // Token: 0x040048EA RID: 18666
        private List<PropertyInfo> headers = new List<PropertyInfo>();

        // Token: 0x040048EB RID: 18667
        private string data;

        // Token: 0x040048EC RID: 18668
        private int index;
    }
}