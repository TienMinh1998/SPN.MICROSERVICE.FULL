using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.DapperExtension
{
    public static class DictionaryExtension
    {
        public static T? GetValueByKey<T>(this Dictionary<string, string> searchDic, string key)
        {
            try
            {
                key = key.Trim();
                if (string.IsNullOrEmpty(key)) return default(T);
                string value = searchDic.Keys.Contains(key) ? searchDic[key] : null;
                if (value == null)
                {
                    return default(T);
                }
                else
                {
                    value = value.Trim();
                    if (typeof(T) == typeof(string))
                        return (T)Convert.ChangeType(value, typeof(T));
                    if (typeof(T) == typeof(bool))
                        return (T)Convert.ChangeType(value, typeof(T));
                    if (typeof(T) == typeof(int))
                        return (T)Convert.ChangeType(value, typeof(T));
                    if (typeof(T) == typeof(double))
                        return (T)Convert.ChangeType(value, typeof(T));
                    if (typeof(T) == typeof(long))
                        return (T)Convert.ChangeType(value, typeof(T));
                }

                Type targetType = typeof(T?);
                Type underlyingType = Nullable.GetUnderlyingType(targetType);

                object result = (underlyingType == typeof(Guid))
                              ? Guid.Parse(value)
                              : Convert.ChangeType(value, underlyingType);

                // Cast the result to the nullable type
                object nullableResult = result != null
                    ? Activator.CreateInstance(targetType, result)
                    : null;

                return (T)nullableResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't convert key when search, error : " + ex.Message);
                return default(T);
            }
        }
    }
}
