using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Dapper;
using SqlExpression;
using SqlExpression.Extension;
using SqlExpression.Extension.Dapper;

namespace SqlExpression
{
    public class Respository<TSchema, TEntity>
        where TSchema : TableSchema<TSchema>, IAliasTableExpression, new()
        where TEntity : class, new()
    {
        protected TSchema schema;
        protected IDbConnection connection;

        public Respository(IDbConnection connection)
        {
            this.schema = new TSchema();
            this.connection = connection;
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
            connection.Execute(exp, entity);
            return null;
        }

        public virtual object Insert(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null)
        {
            var exp = schema.Table
                            .InsertVarParam(columns?.Invoke(schema) ?? schema.All());
            var rows = connection.Execute(exp, entity);
            return null;
        }

        public virtual bool Update<T>(TEntity entity, Func<TEntity, T> columns = null, Func<TSchema, ISimpleValue> filter = null, object param = null)
        {
            var exp = schema.Table
                            .UpdateVarParam(columns == null ? schema.All().Except(schema.PK()) : Properties2Columns<T>())
                            .Where(schema.PK().AllEqVarParam());
            return connection.Execute(exp, entity) > 0;
        }

        public virtual bool Update(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null)
        {
            var exp = schema.Table
                            .UpdateVarParam(columns?.Invoke(schema) ?? schema.All().Except(schema.PK()))
                            .Where(schema.PK().AllEqVarParam());
            return connection.Execute(exp, entity) > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            var exp = schema.Table
                            .Delete()
                            .Where(schema.PK().AllEqVarParam());
            return connection.Execute(exp, entity) > 0;
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
            var param = PrimaryKey2ParamObject(primaryKey);
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.Execute(exp, param) > 0;
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
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.Execute(exp, param);

        }

        public virtual TEntity Get(object primaryKey)
        {
            var exp = schema.Select(schema.All())
                            .Where(schema.PK().AllEqVarParam());
            var param = PrimaryKey2ParamObject(primaryKey);
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QueryFirstOrDefault<TEntity>(exp, param);
        }

        public virtual IEnumerable<TEntity> GetList(IEnumerable<object> primaryKeys)
        {
            if (schema.PK().Length != 1)
            {
                throw new NotSupportedException();
            }
            if (primaryKeys == null || !primaryKeys.Any())
            {
                throw new ArgumentException(nameof(primaryKeys));
            }
            var exp = schema.Select(schema.All())
                            .Where(schema.PK().First().In(primaryKeys));
            return connection.Query<TEntity>(exp);
        }

        public virtual IEnumerable<TEntity> Query(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(schema.All())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.Query<TEntity>(exp, param);
        }

        public virtual IEnumerable<T> QueryPartial<T>(Func<TEntity, T> columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(columns == null ? schema.All() : Properties2Columns<T>())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.Query<T>(exp, param);
        }

        public virtual long Count(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(new AllColumns(schema.Alias).Count())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QuerySingle<long>(exp, param);
        }

        public virtual bool Exists(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(new AllColumns(schema.Alias))
                            .Where(filter(schema))
                            .ToExistsSql();
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QuerySingle<bool>(exp, param);
        }

        public virtual bool NotExists(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(new AllColumns(schema.Alias))
                            .Where(filter(schema))
                            .ToNotExistsSql();
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QuerySingle<bool>(exp, param);
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

        private static ConcurrentDictionary<Type, IList<string>> Cache4ParamPropertyNames { get; } = new ConcurrentDictionary<Type, IList<string>>();
        protected List<string> CheckMissingParams(ISqlStatement exp, object param)
        {
            var paramNotProvided = new List<string>();
            var paramNames = exp.Params;
            if (paramNames == null || !paramNames.Any())
            {
                return paramNotProvided;
            }

            if (param is IDictionary<string, object>)
            {
                var dic = (param as IDictionary<string, object>);
                paramNames.ToList().ForEach(paramName =>
                {
                    if (!dic.ContainsKey(paramName))
                    {
                        paramNotProvided.Add(paramName);
                    }
                });
            }
            else
            {
                var type = param.GetType();
                if (!Cache4ParamPropertyNames.TryGetValue(type, out var paramPropertyNames))
                {
                    paramPropertyNames = new List<string>();
                    var properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        paramPropertyNames.Add(property.Name);
                    }
                }
                paramNames.ToList().ForEach(paramName => {
                    if(!paramPropertyNames.Contains(paramName))
                    {
                        paramNotProvided.Add(paramName);
                    }
                });
            }
            return paramNotProvided;
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
                list.Add((item.Value as IColumn));
            }
            Cache4Properties2Columns.TryAdd(type, list);
            return list;
        }

        #endregion
    }
}
