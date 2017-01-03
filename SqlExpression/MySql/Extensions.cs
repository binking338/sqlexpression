using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.MySql
{
    public static class Extensions
    {
        static Extensions()
        {
            InnerInitial();
        }

        private static void InnerInitial()
        {
            TableExpression.Handlers[DBType.MySql] = (ex) => { return string.Format("`{0}`", ex.Name); };
            PropertyExpression.Handlers[DBType.MySql] = (ex) =>
            {
                return (new string[] {
                            ex.Table?.Expression,
                            string.IsNullOrWhiteSpace(ex.Name) ? string.Empty : string.Format("`{0}`", ex.Name)
                        }).Where(s => !string.IsNullOrWhiteSpace(s)).Join(".");
            };
        }

        public static void Initial() { }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="select"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static string Page(this ISelectStatement select, int pageindex, int pagesize)
        {
            return string.Format("{0} limit {1},{2}", select.Expression, (pageindex - 1) * pagesize, pagesize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static string Count(this ISelectStatement select)
        {
            return string.Format("select count(1) from ({0}) as __totalcount__", select.Expression);
        }

        public static string Exists(this ISelectStatement select)
        {
            return string.Format("select exists({0}) as __exists__", select.Expression);
        }

    }
}
