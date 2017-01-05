using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression;
using SqlExpression.MySql;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class SqlStatementTest
    {
        ISqlStatement statement;

        #region Filter

        [TestMethod]
        public void Filter()
        {

        }

        #endregion

        #region Select

        [TestMethod]
        public void Select()
        {
            statement = TestSchema.Select((sql, s) => sql.Get(s.All()));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.oid).Where(s.oid > 1));
            Assert.AreEqual("SELECT test.oid FROM test WHERE test.oid>1", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.age).OrderBy(s.age.Desc()));
            Assert.AreEqual("SELECT test.age FROM test ORDER BY test.age DESC", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.gender, s.age.Avg()).GroupBy(s.gender).Having(s.age.Avg() < 40));
            Assert.AreEqual("SELECT test.gender,AVG(test.age) FROM test GROUP BY test.gender HAVING AVG(test.age)<40", statement.ToString());

            statement = TestSchema.Select((sql, s) =>sql.Get(s.All()).Where(s.oid > 1).OrderBy(s.age.Desc()));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test WHERE test.oid>1 ORDER BY test.age DESC", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.age.Avg()).Where(s.oid > 1).GroupBy(s.gender).Having(s.age.Avg() > 18).OrderBy(s.age.Desc()));
            Assert.AreEqual("SELECT AVG(test.age) FROM test WHERE test.oid>1 GROUP BY test.gender HAVING AVG(test.age)>18 ORDER BY test.age DESC", statement.ToString());
        }

        [TestMethod]
        public void Join()
        {
            statement = TestSchema.Select((sql, s) => sql.Get(s.All()).InnerJoin(FooSchema.Table, FooSchema.Filter((foo) => s.oid == foo.oid)));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test JOIN foo ON test.oid=foo.oid", statement.ToString());
        }

        [TestMethod]
        public void Union()
        {
            statement = FooSchema.Select((sql, s) => sql.Get(s.oid, s.oname))
                .Union(BarSchema.Select((sql, s) => sql.Get(s.oid, s.oname)));
            Assert.AreEqual("SELECT foo.oid,foo.oname FROM foo UNION SELECT bar.oid,bar.oname FROM bar", statement.ToString());
        }

        #endregion

        #region Update

        [TestMethod]
        public void Update()
        {
            statement = FooSchema.Update((sql, s) => sql.SetVarParam(s.oname).Where(s.oid == 1));
            Assert.AreEqual("UPDATE foo SET foo.oname=@oname WHERE foo.oid=1", statement.ToString());
        }

        #endregion

        #region Insert

        #endregion

        #region Delete

        #endregion
    }
}
