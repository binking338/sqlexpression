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

        public static TableExpression Table { get; protected set; }

        public static Schema Instance { get; protected set; }

        public static ISelectStatement Select(Func<ISelectStatement, Schema, ISelectStatement> fun)
        {
            var sql = new SelectStatement();
            sql.Tables = new ITableExpression[] { Table };
            return fun(sql, Instance);
        }

        public static IUpdateStatement Update(Func<IUpdateStatement, Schema, IUpdateStatement> fun)
        {
            var sql = new UpdateStatement();
            sql.Table = Table;
            return fun(sql, Instance);
        }

        public static IInsertStatement Insert(Func<IInsertStatement, Schema, IInsertStatement> fun)
        {
            var sql = new InsertStatement();
            sql.Table = Table;
            return fun(sql, Instance);
        }

        public static IDeleteStatement Delete(Func<IDeleteStatement, Schema, IDeleteStatement> fun)
        {
            var sql = new DeleteStatement();
            sql.Table = Table;
            return fun(sql, Instance);
        }
    }
}
