using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression
{
    public abstract class SchemaBase<Schema>
        where Schema : class, new()
    {
        static SchemaBase()
        {
            Instance = new Schema();
        }

        public static Table Table { get; protected set; }

        public static Schema Instance { get; protected set; }

        public static ISelectStatement Select(Func<ISelectStatement, Schema, ISelectStatement> fun)
        {
            var sql = new SelectStatement(new TableAliasExpression(Table, null));
            return fun(sql, Instance);
        }

        public static IUpdateStatement Update(Func<IUpdateStatement, Schema, IUpdateStatement> fun)
        {
            var sql = new UpdateStatement(new TableAliasExpression(Table, null));
            return fun(sql, Instance);
        }

        public static IInsertStatement Insert(Func<IInsertStatement, Schema, IInsertStatement> fun)
        {
            var sql = new InsertStatement(Table);
            return fun(sql, Instance);
        }

        public static IDeleteStatement Delete(Func<IDeleteStatement, Schema, IDeleteStatement> fun)
        {
            var sql = new DeleteStatement(Table);
            return fun(sql, Instance);
        }

        public static IFilterExpression Filter(Func<Schema, IFilterExpression> fun)
        {
            return fun(Instance);
        }
    }
}
