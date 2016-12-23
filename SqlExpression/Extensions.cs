using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class Extensions
    {
        /// <summary>
        /// 拼接字符串数组，拼接之前调用transformer变换字符串
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator"></param>
        /// <param name="transformer"></param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> array, string separator, Func<T, string> transformer = null)
        {
            if (array == null) return string.Empty;
            if (transformer == null) transformer = o => o.ToString();
            var wrapArray = from i in array select transformer(i);
            var result = string.Join(separator, wrapArray);
            return result;
        }
    }
}
