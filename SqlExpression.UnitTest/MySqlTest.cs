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
            SqlExpression.MySql.Extensions.Initial();
            Assert.AreEqual("`test`.`oid`", TestSchema.Instance.oid.Expression);
        }
    }
}
