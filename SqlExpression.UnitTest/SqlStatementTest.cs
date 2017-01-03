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
            var sql = TestSchema.Select((t, s) => { return t.Select(s.All()); });
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.isdel FROM test", sql.ToString());
        }
    }
}
