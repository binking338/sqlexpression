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

        public static IEnumerable<T> Query<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.Query<T>(select.ToString(), param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        #endregion

        #region IQueryStatement

        public static IEnumerable<T> Query<T>(this IDbConnection connection, IQueryStatement select, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.Query<T>(select.ToString(), param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection connection, IQueryStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection connection, IQueryStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this IDbConnection connection, IQueryStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbConnection connection, IQueryStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault<T>(select.ToString(), param, transaction, commandTimout, commandType);
        }

        #endregion

        #endregion

        public static T ExecuteScalar<T>(this IDbConnection connection, ISqlStatement sql, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.ExecuteScalar<T>(sql.ToString(), param, transaction, commandTimout, commandType);
        }

        public static int Execute(this IDbConnection connection, ISqlStatement sql, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            return connection.Execute(sql.ToString(), param, transaction, commandTimout, commandType);
        }

    }
}
