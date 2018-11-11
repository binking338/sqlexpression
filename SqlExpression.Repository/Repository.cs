using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using SqlExpression;
using SqlExpression.Extension;
using Dapper;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace SqlExpression
{
    public class Respository<TSchema, TEntity>
        where TSchema : SchemaExpression<TSchema>, IAliasTableExpression, new()
        where TEntity : class, new()
    {
        protected TSchema schema;
        protected IDbConnection connection;
        protected ILogger logger;
        protected bool enableLog;

        public Respository(IDbConnection connection, ILogger logger = null, bool enableLog = true)
        {
            this.schema = new TSchema();
            this.connection = connection;
            this.logger = logger;
            this.enableLog = enableLog;
        }

        public virtual TSchema Schema
        {
            get { return schema; }
            set { schema = value; }
        }

        public virtual object Insert<T>(TEntity entity, Func<TEntity, T> columns = null)
        {
            var exp = schema.Table
                            .InsertVarParam(columns == null ? schema.All() : Properties2Columns<T>());
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            connection.Execute(sql, entity);
            return null;
        }

        public virtual object Insert(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null)
        {
            var exp = schema.Table
                            .InsertVarParam(columns == null ? schema.All() : columns(schema));
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            var rows = connection.Execute(sql, entity);
            return null;
        }

        public virtual bool Update<T>(TEntity entity, Func<TEntity, T> columns = null, Func<TSchema, ISimpleValue> filter = null, object param = null)
        {
            var exp = schema.Table
                            .UpdateVarParam(columns == null ? schema.All().Except(schema.PK()) : Properties2Columns<T>())
                            .Where(schema.PK().AllEqVarParam());
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Execute(sql, entity) > 0;
        }

        public virtual bool Update(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null)
        {
            var exp = schema.Table
                            .UpdateVarParam(columns == null ? schema.All().Except(schema.PK()) : columns(schema))
                            .Where(schema.PK().AllEqVarParam());
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Execute(sql, entity) > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            var exp = schema.Table
                            .Delete()
                            .Where(schema.PK().AllEqVarParam());
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Execute(sql, entity) > 0;
        }

        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            var rows = 0;
            foreach (var entity in entities)
            {
                if (Delete(entity)) rows++;
            }
            return rows;
        }

        public virtual bool Delete(object primaryKey)
        {
            var exp = schema.Table
                            .Delete()
                            .Where(schema.PK().AllEqVarParam());
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Execute(sql, PrimaryKey2ParamObject(primaryKey)) > 0;
        }

        public virtual int Delete(IEnumerable<object> primaryKeys)
        {
            var rows = 0;
            foreach (var primaryKey in primaryKeys)
            {
                if (Delete(primaryKey)) rows++;
            }
            return rows;
        }

        public virtual int Delete(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Table
                            .Delete()
                            .Where(filter(schema));
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Execute(sql, param);

        }

        public virtual TEntity Get(object primaryKey)
        {
            var exp = schema.Select(schema.All())
                            .Where(schema.PK().AllEqVarParam());
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.QueryFirstOrDefault<TEntity>(sql, PrimaryKey2ParamObject(primaryKey));
        }

        public virtual IEnumerable<TEntity> GetList(IEnumerable<object> primaryKeys)
        {
            if (schema.PK().Length != 1)
            {
                throw new NotSupportedException();
            }
            if (primaryKeys == null || primaryKeys.Any())
            {
                throw new ArgumentException(nameof(primaryKeys));
            }
            var exp = schema.Select(schema.All())
                            .Where(schema.PK().First().In(primaryKeys));
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Query<TEntity>(sql);
        }

        public virtual IEnumerable<TEntity> Query(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(schema.All())
                            .Where(filter(schema));
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Query<TEntity>(sql, param);
        }

        public virtual IEnumerable<T> QueryPartial<T>(Func<TEntity, T> columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(columns == null ? schema.All() : Properties2Columns<T>())
                            .Where(filter(Schema));
            var sql = exp.ToString();
            if (enableLog && logger != null) logger.LogDebug(sql);
            return connection.Query<T>(sql, param);
        }

        #region 工具函数

        protected object PrimaryKey2ParamObject(object primaryKey)
        {
            if (schema.PK().Length <= 0)
            {
                throw new NotSupportedException();
            }
            if (schema.PK().Length == 1 && primaryKey.GetType().IsValueType)
            {
                var param = new Dictionary<string, object>();
                param[schema.PK().First().ToParam().Name] = primaryKey;
                primaryKey = param;
            }
            return primaryKey;
        }

        private static ConcurrentDictionary<Type, IEnumerable<IColumn>> Cache4Properties2Columns { get; } = new ConcurrentDictionary<Type, IEnumerable<IColumn>>();
        protected IEnumerable<IColumn> Properties2Columns<T>()
        {
            var type = typeof(T);
            if (Cache4Properties2Columns.TryGetValue(type, out var columns))
            {
                return columns;
            }

            var properties = type.GetProperties();
            var list = new List<IColumn>();
            foreach (var property in properties)
            {
                var item = schema.AllMapped().First(i => i.Alias == property.Name);
                list.Add((item.Column as IColumn));
            }
            Cache4Properties2Columns.TryAdd(type, list);
            return list;
        }

        #endregion
    }
}
