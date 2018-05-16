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
            Assert.AreEqual("`foo`.`oid`", new Foo4MysqlSchema().oid.Expression);
            Assert.AreEqual("`foo`", new Foo4MysqlSchema().As("foo").oid.Dataset.Expression);
            MySql.Extensions.DisableDefault();
        }

        public class Foo4MysqlSchema
        {
            public static Table Table { get; protected set; }

            static Foo4MysqlSchema()
            {
                Table = new Table("foo");
            }

            public Foo4MysqlSchema As(string alias)
            {
                return new Foo4MysqlSchema(alias);
            }

            public Foo4MysqlSchema()
                : this("foo")
            {

            }

            public Foo4MysqlSchema(string alias)
            {
                oid = new Column("oid", new DatasetAlias(alias));
                oname = new Column("oname", new DatasetAlias(alias));
                isdel = new Column("isdel", new DatasetAlias(alias));
                PKs = new Column[] { oid };
                All = new Column[] { oid, oname, isdel };
            }

            public Column oid { get; set; }
            public Column oname { get; set; }
            public Column isdel { get; set; }

            public Column[] PKs { get; set; }

            public Column[] All { get; set; }
        }
    }
}
