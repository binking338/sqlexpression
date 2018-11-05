using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using SqlExpression;

namespace SqlExpression.Extension.Dapper
{
    public static class DapperExtensions
    {
        #region Query

        #region ISelectStatement
        public static IEnumerable<T> Query<T>(this ISelectStatement select, IDbConnection connection)
        {
            return connection.Query<T>(select.Expression);
        }

        public static IEnumerable<T> Query<T>(this ISelectStatement select, object param, IDbConnection connection)
        {
            return connection.Query<T>(select.Expression, param);
        }

        public static T QueryFirst<T>(this ISelectStatement select, object param, IDbConnection connection)
        {
            return connection.QueryFirst<T>(select.Expression, param);
        }

        public static T QueryFirst<T>(this ISelectStatement select, IDbConnection connection)
        {
            return connection.QueryFirst<T>(select.Expression);
        }

        public static T QueryFirstOrDefault<T>(this ISelectStatement select, object param, IDbConnection connection)
        {
            return connection.QueryFirstOrDefault<T>(select.Expression, param);
        }

        public static T QueryFirstOrDefault<T>(this ISelectStatement select, IDbConnection connection)
        {
            return connection.QueryFirstOrDefault<T>(select.Expression);
        }

        public static T QuerySingle<T>(this ISelectStatement select, object param, IDbConnection connection)
        {
            return connection.QuerySingle<T>(select.Expression, param);
        }

        public static T QuerySingle<T>(this ISelectStatement select, IDbConnection connection)
        {
            return connection.QuerySingle<T>(select.Expression);
        }

        public static T QuerySingleOrDefault<T>(this ISelectStatement select, object param, IDbConnection connection)
        {
            return connection.QuerySingleOrDefault<T>(select.Expression, param);
        }

        public static T QuerySingleOrDefault<T>(this ISelectStatement select, IDbConnection connection)
        {
            return connection.QuerySingleOrDefault<T>(select.Expression);
        }

        #endregion

        #region IQueryStatement
        public static IEnumerable<T> Query<T>(this IQueryStatement select, IDbConnection connection)
        {
            return connection.Query<T>(select.Expression);
        }

        public static IEnumerable<T> Query<T>(this IQueryStatement select, object param, IDbConnection connection)
        {
            return connection.Query<T>(select.Expression, param);
        }

        public static T QueryFirst<T>(this IQueryStatement select, object param, IDbConnection connection)
        {
            return connection.QueryFirst<T>(select.Expression, param);
        }

        public static T QueryFirst<T>(this IQueryStatement select, IDbConnection connection)
        {
            return connection.QueryFirst<T>(select.Expression);
        }

        public static T QueryFirstOrDefault<T>(this IQueryStatement select, object param, IDbConnection connection)
        {
            return connection.QueryFirstOrDefault<T>(select.Expression, param);
        }

        public static T QueryFirstOrDefault<T>(this IQueryStatement select, IDbConnection connection)
        {
            return connection.QueryFirstOrDefault<T>(select.Expression);
        }

        public static T QuerySingle<T>(this IQueryStatement select, object param, IDbConnection connection)
        {
            return connection.QuerySingle<T>(select.Expression, param);
        }

        public static T QuerySingle<T>(this IQueryStatement select, IDbConnection connection)
        {
            return connection.QuerySingle<T>(select.Expression);
        }

        public static T QuerySingleOrDefault<T>(this IQueryStatement select, object param, IDbConnection connection)
        {
            return connection.QuerySingleOrDefault<T>(select.Expression, param);
        }

        public static T QuerySingleOrDefault<T>(this IQueryStatement select, IDbConnection connection)
        {
            return connection.QuerySingleOrDefault<T>(select.Expression);
        }

        #endregion

        #endregion
    }
}
