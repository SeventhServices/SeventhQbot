using System;
using System.Collections.Generic;
using System.Reflection;
using Seventh.Resource.Asset.SqlLoader.Extensions;

namespace Seventh.Resource.Asset.SqlLoader.Serializer
{
    public class SqlParser<T>
    {
        private readonly List<PropertyInfo> _headers = new List<PropertyInfo>();
        private string _data;
        private int _index;

        /// <summary>
        /// 分析 sql 字符串
        /// </summary>
        /// <typeparam name="T">sql对应的类</typeparam>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public IEnumerable<T> Parse(string sqlString) 
        {
            _data = sqlString;
            _index = _data.IndexOf('(');
            while (_data[_index] != ')')
            {
                _headers.Add(typeof(T).GetProperty(
                    GetNextValue().SnakeToCamel()));
            }
            for (; ; )
            {
                while (_data[_index] != '(')
                {
                    _index++;
                    if (_index == _data.Length)
                    {
                        yield break;
                    }
                }
                if (_data[_index + 1] == '`')
                {
                    _index++;
                }
                else
                {
                    yield return GetNextObject();
                }
            }
        }

        /// <summary>
        /// 获取开头的属性的下一个值
        /// </summary>
        /// <returns></returns>
        private string GetNextValue()
        {
            _index++;
            char c = _data[_index];
            string text = string.Empty;
            _index++;
            for (; ; )
            {
                if (_data[_index] == c)
                {
                    _index++;
                    if (_data[_index] != c)
                    {
                        break;
                    }
                }
                text += _data[_index];
                _index++;
            }
            return text;
        }

        /// <summary>
        /// 获取下一组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetNextObject()
        {
            var t = Activator.CreateInstance<T>();
            foreach (var propertyInfo in _headers)
            {
                var nextValue = GetNextValue();
                if (propertyInfo == null) continue;

                if (propertyInfo.PropertyType == typeof(bool))
                {
                    var value = nextValue.ToLower() == "1" 
                                    || nextValue.ToLower() == "true";
                    propertyInfo.SetValue(t, value);
                }
                else
                {
                    var value = Convert.ChangeType(nextValue, propertyInfo.PropertyType);
                    propertyInfo.SetValue(t, value);
                }

            }
            return t;
        }
    }
}