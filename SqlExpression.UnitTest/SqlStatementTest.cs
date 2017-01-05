using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression;
using SqlExpression.MySql;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class SqlStatementTest
    {
        [TestMethod]
        public void Select()
        {
            ISelectStatement statement;
            statement = TestSchema.Select((sql, s) => { return sql.Get(s.All()); });
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test", statement.ToString());

            statement = TestSchema.Select((sql, s) => { return sql.Get(s.oid).Where(s.oid > 1); });
            Assert.AreEqual("SELECT test.oid FROM test WHERE test.oid>1", statement.ToString());

            statement = TestSchema.Select((sql, s) => { return sql.Get(s.age).OrderBy(s.age.Desc()); });
            Assert.AreEqual("SELECT test.age FROM test ORDER BY test.age DESC", statement.ToString());

            statement = TestSchema.Select((sql, s) => { return sql.Get(s.gender, s.age.Avg()).GroupBy(s.gender).Having(s.age.Avg() < 40); });
            Assert.AreEqual("SELECT test.gender,AVG(test.age) FROM test GROUP BY test.gender HAVING AVG(test.age)<40", statement.ToString());

            statement = TestSchema.Select((sql, s) => { return sql.Get(s.All()).Where(s.oid > 1).OrderBy(s.age.Desc()); });
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test WHERE test.oid>1 ORDER BY test.age DESC", statement.ToString());

            statement = TestSchema.Select((sql, s) => { return sql.Get(s.age.Avg()).Where(s.oid > 1).GroupBy(s.gender).Having(s.age.Avg() > 18).OrderBy(s.age.Desc()); });
            Assert.AreEqual("SELECT AVG(test.age) FROM test WHERE test.oid>1 GROUP BY test.gender HAVING AVG(test.age)>18 ORDER BY test.age DESC", statement.ToString());
        }

        public void Join()
        {

        }
    }
}
