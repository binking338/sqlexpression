using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Dapper;
using Microsoft.Extensions.Logging;
using SqlExpression;

namespace SqlExpression.Extension.Dapper
{
    public static class DapperExtensions
    {
        private static ThreadLocal<string> threadLocalLastSql = new ThreadLocal<string>(() => null);

        public static ILogger Logger { get; set; }
        public static bool EnableLog { get; set; } = true;
        public static string LastSql { get { return threadLocalLastSql.Value; } }

        #region Query

        #region ISelectStatement

        public static IEnumerable<T> Query<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = select.ToString();
            threadLocalLastSql.Value = sql;
            if(EnableLog) Logger?.LogDebug(sql);
            return connection.Query<T>(sql, param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = select.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirst<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = select.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = select.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingle<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbConnection connection, ISelectStatement select, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = select.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingleOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static IEnumerable<T> Query<T>(this IDbConnection connection, IExpressionMapper<ISelectStatement, T> selectMapper, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = selectMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.Query<T>(sql, param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection connection, IExpressionMapper<ISelectStatement, T> selectMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = selectMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirst<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection connection, IExpressionMapper<ISelectStatement, T> selectMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = selectMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this IDbConnection connection, IExpressionMapper<ISelectStatement, T> selectMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = selectMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingle<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbConnection connection, IExpressionMapper<ISelectStatement, T> selectMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = selectMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingleOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        #endregion

        #region IQueryStatement

        public static IEnumerable<T> Query<T>(this IDbConnection connection, IQueryStatement query, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = query.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.Query<T>(sql, param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection connection, IQueryStatement query, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = query.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirst<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection connection, IQueryStatement query, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = query.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbConnection connection, IQueryStatement query, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = query.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingleOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this IDbConnection connection, IQueryStatement query, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = query.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingle<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static IEnumerable<T> Query<T>(this IDbConnection connection, IExpressionMapper<IQueryStatement, T> queryMapper, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = queryMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.Query<T>(sql, param, transaction, buffered, commandTimout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection connection, IExpressionMapper<IQueryStatement, T> queryMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = queryMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirst<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection connection, IExpressionMapper<IQueryStatement, T> queryMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = queryMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingle<T>(this IDbConnection connection, IExpressionMapper<IQueryStatement, T> queryMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = queryMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingle<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbConnection connection, IExpressionMapper<IQueryStatement, T> queryMapper, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = queryMapper.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.QuerySingleOrDefault<T>(sql, param, transaction, commandTimout, commandType);
        }

        #endregion

        #endregion

        public static T ExecuteScalar<T>(this IDbConnection connection, ISqlStatement statement, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = statement.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.ExecuteScalar<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static T ExecuteScalar<T>(this IDbConnection connection, IExpressionMapper<IQueryStatement, T> statement, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = statement.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.ExecuteScalar<T>(sql, param, transaction, commandTimout, commandType);
        }

        public static int Execute(this IDbConnection connection, ISqlStatement statement, object param = null, IDbTransaction transaction = null, int? commandTimout = null, CommandType? commandType = null)
        {
            var sql = statement.ToString();
            threadLocalLastSql.Value = sql;
            if (EnableLog) Logger?.LogDebug(sql);
            return connection.Execute(sql, param, transaction, commandTimout, commandType);
        }

    }
}
