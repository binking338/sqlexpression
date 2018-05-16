using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class MySqlTest
    {
        [TestMethod]
        public void Test()
        {
            MySql.Extensions.EnableDefault();
            var foo = new Foo4MysqlSchema();
            Assert.AreEqual("`foo`.`oid`", foo.oid.Expression);
            Assert.AreEqual("`foo`", foo.As("foo").oid.Dataset.Expression);
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
                var datasetAlias = new DatasetAlias(alias);

                TableAlias = new TableAliasExpression(Table, datasetAlias);
                oid = new Column("oid", datasetAlias);
                oname = new Column("oname", datasetAlias);
                isdel = new Column("isdel", datasetAlias);
                PKs = new Column[] { oid };
                All = new Column[] { oid, oname, isdel };
            }

            public TableAliasExpression TableAlias { get; protected set; }

            public Column oid { get; protected set; }
            public Column oname { get; protected set; }
            public Column isdel { get; protected set; }

            public Column[] PKs { get; protected set; }

            public Column[] All { get; protected set; }
        }
    }
}
