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

        public static IEnumerable<T> Query<T>(this ISelectStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.Query<T>(select.Exp, param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this ISelectStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this ISelectStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this ISelectStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this ISelectStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        #endregion

        #region IQueryStatement

        public static IEnumerable<T> Query<T>(this IQueryStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.Query<T>(select.Exp, param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this IQueryStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IQueryStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this IQueryStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IQueryStatement select, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault<T>(select.Exp, param, transaction, commandTimout, commandType);
        }

        #endregion

        #endregion

        public static T ExecuteScalar<T>(this ISqlStatement sql, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.ExecuteScalar<T>(sql.Exp, param, transaction, commandTimout, commandType);
        }

        public static int Execute(this ISqlStatement sql, IDbConnection connection, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.Execute(sql.Exp, param, transaction, commandTimout, commandType);
        }

    }
}
