using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class MySqlTest
    {
        [TestMethod]
        public void Test()
        {
            MySql.Extensions.EnableDefault();
            Assert.AreEqual("`foo`.`oid`", Foo4MysqlSchema.Instance.oid.Expression);
            Assert.AreEqual("`foo`", Foo4MysqlSchema.Instance.oid.Dataset.Expression);
            MySql.Extensions.DisableDefault();
        }

        public class Foo4MysqlSchema : SchemaBase<Foo4MysqlSchema>
        {
            private static Column[] __PKs;
            private static Column[] __All;
            private static Column _oid;
            private static Column _oname;
            private static Column _isdel;

            static Foo4MysqlSchema()
            {
                Table = new Table("foo");
                _oid = new Column("oid") { Dataset = new DatasetAlias(Table.Name) };
                _oname = new Column("oname") { Dataset = new DatasetAlias(Table.Name) };
                _isdel = new Column("isdel") { Dataset = new DatasetAlias(Table.Name) };
                __PKs = new Column[] { _oid };
                __All = new Column[] { _oid, _oname, _isdel };
            }

            public Column oid { get { return _oid; } }
            public Column oname { get { return _oname; } }
            public Column isdel { get { return _isdel; } }

            public Column[] PKs()
            {
                return __PKs;
            }

            public Column[] All()
            {
                return __All;
            }
        }
    }
}
