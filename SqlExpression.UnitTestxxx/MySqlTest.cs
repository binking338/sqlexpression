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
            var foo = new Foo4MysqlSchema();
            Assert.AreEqual("`foo`.`oid`", foo.oid.Expression);
            Assert.AreEqual("`foo`", foo.As("foo").oid.DatasetAlias.Expression);
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
                var datasetAlias = new AliasDataset(alias);

                TableAlias = new AliasTableExpression(Table, datasetAlias);
                oid = new Field("oid", datasetAlias);
                oname = new Field("oname", datasetAlias);
                isdel = new Field("isdel", datasetAlias);
                PKs = new Field[] { oid };
                All = new Field[] { oid, oname, isdel };
            }

            public AliasTableExpression TableAlias { get; protected set; }

            public Field oid { get; protected set; }
            public Field oname { get; protected set; }
            public Field isdel { get; protected set; }

            public Field[] PKs { get; protected set; }

            public Field[] All { get; protected set; }
        }
    }
}
