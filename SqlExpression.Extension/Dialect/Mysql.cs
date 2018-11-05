﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.Extension.Dialect
{
    public static class Mysql
    {
        public static void EnableDialect()
        {
            Expression.OpenQuotationMark = "`";
            Expression.CloseQuotationMark = "`";
        }

        public static void DisableDialect()
        {
            Expression.OpenQuotationMark = string.Empty;
            Expression.CloseQuotationMark = string.Empty;
        }

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
        /// 返回计数sql语句
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static string Count(this ISelectStatement select)
        {
            return string.Format("SELECT COUNT(*) FROM ({0}) AS __totalcount__", select.Expression);
        }

        /// <summary>
        /// 判断是否存在sql语句
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static string Exists(this ISelectStatement select)
        {
            return string.Format("SELECT EXISTS({0}) AS __exists__", select.Expression);
        }

    }
}