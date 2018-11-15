using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;

namespace SqlExpression.UnitTest.Statement
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class DeleteStatementTest
    {
        [TestMethod]
        public void Test()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            IExpression e;

            e = (foo as IAliasTableExpression).Table.Delete();
            Assert.AreEqual("DELETE FROM foo", e.ToString());

            e = (foo as IAliasTableExpression).Table.Delete().Where(foo.Id == 1);
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=1", e.ToString());
        }
    }
}
