using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.UnitTest
{
    public abstract class SchemaBase<Schema>
        where Schema : class, new()
    {
        static SchemaBase()
        {
            Instance = new Schema();
        }

        public static TableExpression Table { get; set; }

        public static Schema Instance { get; set; }

        public static SelectStatement Select(Func<TableExpression, Schema, SelectStatement> fun)
        {
            return fun(Table, Instance);
        }

        public static UpdateStatement Update(Func<TableExpression, Schema, UpdateStatement> fun)
        {
            return fun(Table, Instance);
        }

        public static InsertStatement Insert(Func<TableExpression, Schema, InsertStatement> fun)
        {
            return fun(Table, Instance);
        }

        public static DeleteStatement Delete(Func<TableExpression, Schema, DeleteStatement> fun)
        {
            return fun(Table, Instance);
        }
    }


    public class TestSchema : SchemaBase<TestSchema>
    {
        static TestSchema()
        {
            Table = new TableExpression("test");
        }

        public TestSchema()
        {
            oid = new PropertyExpression("oid") { Type = DBType.MySql, Table = Table };
            oname = new PropertyExpression("oname") { Type = DBType.MySql, Table = Table };
            age = new PropertyExpression("age") { Type = DBType.MySql, Table = Table };
            isdel = new PropertyExpression("isdel") { Type = DBType.MySql, Table = Table };
            _PKs = new PropertyExpression[] { oid };
            _All = new PropertyExpression[] { oid, oname, age, isdel };
        }

        public PropertyExpression oid { get; protected set; }
        public PropertyExpression oname { get; protected set; }
        public PropertyExpression age { get; protected set; }
        public PropertyExpression isdel { get; protected set; }

        protected PropertyExpression[] _PKs { get; set; }
        protected PropertyExpression[] _All { get; set; }

        public PropertyExpression[] PKs()
        {
            return _PKs;
        }

        public PropertyExpression[] All()
        {
            return _All;
        }
    }
}
