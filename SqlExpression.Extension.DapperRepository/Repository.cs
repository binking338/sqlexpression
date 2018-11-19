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

namespace SqlExpression.Extension.DapperRepository
{
    public class Repository<TSchema, TEntity>
        where TSchema : TableSchema<TSchema>, IAliasTableExpression, new()
        where TEntity : class, new()
    {
        internal TSchema schema;
        internal IDbConnection connection;

        public static Func<IInsertStatement, ISqlStatement> AppendReturnIdStatementHandler { get; set; }

        public Repository(IDbConnection connection)
        {
            this.schema = new TSchema();
            this.connection = connection;
        }

        public virtual TSchema Schema
        {
            get { return schema; }
            set { schema = value; }
        }

        protected virtual ISqlStatement AppendReturnIdStatement(IInsertStatement insert)
        {
            return AppendReturnIdStatementHandler?.Invoke(insert) ?? insert;
        }

        public void Test()
        {
        }

        public virtual object Insert(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null)
        {
            var exp = schema.Table
                            .InsertVarParam(columns?.Invoke(schema) ?? schema.All());
            if (schema.PK().Count() > 1)
            {
                var expReturnId = AppendReturnIdStatement(exp);
                if(expReturnId != exp)
                {
                    var id = connection.ExecuteScalar<object>(expReturnId, entity);
                    typeof(TEntity).GetProperty(schema.PKMapped().First().Alias).SetValue(entity, id);
                    return id;
                }
            }
            var rows = connection.Execute(exp, entity);
            return null;
        }

        public virtual object Insert<T>(TEntity entity, Func<TEntity, T> columns = null)
        {
            return Insert(entity, (TSchema s) => Properties2Columns<T>());
        }

        public virtual object InsertPartial<Partial>(Partial entity)
        {
            var exp = schema.Table
                            .InsertVarParam(Properties2Columns<Partial>());
            if (schema.PK().Count() > 1)
            {
                var expReturnId = AppendReturnIdStatement(exp);
                if (expReturnId != exp)
                {
                    var id = connection.ExecuteScalar<object>(expReturnId, entity);
                    return id;
                }
            }
            var rows = connection.Execute(exp, entity);
            return null;
        }

        public virtual int Update(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null, Func<TSchema, ISimpleValue> filter = null, object param = null)
        {
            if (columns == null) columns = s => s.All(false);
            var exp = schema.Table
                            .UpdateVarParam(columns(schema))
                            .Where(filter?.Invoke(schema) ?? schema.PK().AllEqVarParam());
            var paramNames = columns(schema).Select(c => exp.Option.Column2ParamContractHandler(c.Name));
            if (filter == null) paramNames = paramNames.Concat(schema.PK().Select(c => exp.Option.Column2ParamContractHandler(c.Name)));
            var dic = Properties2Dictionary(entity, null, paramNames);
            if (filter != null && param != null) Properties2Dictionary(param, dic);
            param = dic;
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.Execute(exp, dic);
        }

        public virtual int Update<T>(TEntity entity, Func<TEntity, T> columns, Func<TSchema, ISimpleValue> filter = null, object param = null)
        {
            return Update(entity, (TSchema s) => Properties2Columns<T>(true), filter, param);
        }

        public virtual int UpdatePartial<Partial>(Partial entity, Func<TSchema, ISimpleValue> filter = null, object param = null)
        {
            var columns = Properties2Columns<Partial>(true);
            var exp = schema.Table
                               .UpdateVarParam(columns)
                               .Where(filter?.Invoke(schema) ?? schema.PK().AllEqVarParam());
            var dic = Properties2Dictionary(entity, null);
            if (filter != null && param != null) Properties2Dictionary(param, dic);
            param = dic;
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.Execute(exp, dic);
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

        public virtual TEntity QueryFirst(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(schema.All())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QueryFirst<TEntity>(exp, param);
        }

        public virtual TEntity QueryFirstOrDefault(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(schema.All())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QueryFirstOrDefault<TEntity>(exp, param);
        }

        public virtual TEntity QuerySingle(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(schema.All())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QuerySingle<TEntity>(exp, param);
        }

        public virtual TEntity QuerySingleOrDefault(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(schema.All())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QuerySingleOrDefault<TEntity>(exp, param);
        }

        public virtual IEnumerable<Partial> QueryPartial<Partial>(Func<TEntity, Partial> columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(columns == null ? schema.All() : Properties2Columns<Partial>())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.Query<Partial>(exp, param);
        }

        public virtual Partial QueryPartialFirst<Partial>(Func<TEntity, Partial> columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(columns == null ? schema.All() : Properties2Columns<Partial>())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QueryFirst<Partial>(exp, param);
        }

        public virtual Partial QueryPartialFirstOrDefault<Partial>(Func<TEntity, Partial> columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(columns == null ? schema.All() : Properties2Columns<Partial>())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QueryFirstOrDefault<Partial>(exp, param);
        }

        public virtual Partial QueryPartialSingle<Partial>(Func<TEntity, Partial> columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(columns == null ? schema.All() : Properties2Columns<Partial>())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QuerySingle<Partial>(exp, param);
        }

        public virtual Partial QueryPartialSingleOrDefault<Partial>(Func<TEntity, Partial> columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            var exp = schema.Select(columns == null ? schema.All() : Properties2Columns<Partial>())
                            .Where(filter(schema));
            var missingParams = CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return connection.QuerySingleOrDefault<Partial>(exp, param);
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

        internal object PrimaryKey2ParamObject(object primaryKey)
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

        internal Dictionary<string, object> Properties2Dictionary(object src, Dictionary<string, object> des, IEnumerable<string> propertyNames = null)
        {
            if (src == null) return null;
            Dictionary<string, object> dic = des ?? new Dictionary<string, object>();
            if (src is IDictionary<string, object>)
            {
                foreach (var pair in src as IDictionary<string, object>)
                {
                    dic[pair.Key] = pair.Value;
                }
                return dic;
            }
            else
            {
                var type = src.GetType();
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    if (propertyNames == null || propertyNames.Contains(property.Name))
                    {
                        dic[property.Name] = property.GetValue(src);
                    }
                }
            }
            return dic;
        }

        private static ConcurrentDictionary<Type, IList<string>> Cache4ParamPropertyNames { get; } = new ConcurrentDictionary<Type, IList<string>>();
        internal List<string> CheckMissingParams(ISqlStatement exp, object param)
        {
            var paramNotProvided = new List<string>();
            var paramNames = exp.Params;
            if (paramNames == null || !paramNames.Any())
            {
                return paramNotProvided;
            }
            if (param == null)
            {
                paramNotProvided.AddRange(paramNames);
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
                paramNames.ToList().ForEach(paramName =>
                {
                    if (!paramPropertyNames.Contains(paramName))
                    {
                        paramNotProvided.Add(paramName);
                    }
                });
            }
            return paramNotProvided;
        }

        private static ConcurrentDictionary<Type, IEnumerable<IColumn>> Cache4Properties2Columns { get; } = new ConcurrentDictionary<Type, IEnumerable<IColumn>>();
        internal IEnumerable<IColumn> Properties2Columns<T>(bool excludePK = false)
        {
            var type = typeof(T);
            if (Cache4Properties2Columns.TryGetValue(type, out var columns))
            {
                if (excludePK) return columns.Except(schema.PK());
                return columns;
            }

            var properties = type.GetProperties();
            var list = new List<IColumn>();
            foreach (var property in properties)
            {
                var item = schema.AllMapped().FirstOrDefault(i => i.Alias == property.Name);
                if (item == null) throw new ArgumentException(string.Format(Error.ColumnNotDefined, property.Name));
                list.Add((item.Value as IColumn));
            }
            Cache4Properties2Columns.TryAdd(type, list);

            if (excludePK) return list.Except(schema.PK());
            return list;
        }

        #endregion
    }
}
