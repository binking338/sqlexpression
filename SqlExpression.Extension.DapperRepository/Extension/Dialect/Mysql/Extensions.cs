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

namespace SqlExpression.Extension.Dialect.Mysql
{
    public static class Extensions
    {
        public static IEnumerable<TEntity> QueryByPage<TSchema, TEntity>(this Repository<TSchema, TEntity> repository, Func<TSchema, ISimpleValue> filter, object param = null, int pagesize = 50, int pageindex = 1)

            where TSchema : TableSchema<TSchema>, IAliasTableExpression, new()
            where TEntity : class, new()
        {
            var schema = repository.schema;
            var exp = schema.Select(schema.All())
                            .Where(filter(schema))
                            .Page(pagesize, pageindex);
            var missingParams = repository.CheckMissingParams(exp, param);
            if (missingParams.Any())
            {
                throw new ArgumentException(string.Format(Error.ParamMissing, string.Join(",", missingParams)), nameof(param));
            }
            return repository.connection.Query<TEntity>(exp, param);
        }
    }
}
