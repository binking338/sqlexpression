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

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Delete();
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=1", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Delete().Where(foo.Age > 18);
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=1 AND foo.age>18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Where(foo.Age > 18).Delete();
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=1 AND foo.age>18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Delete().OrWhere(foo.Age > 18);
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=1 OR foo.age>18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).OrWhere(foo.Age > 18).Delete();
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=1 OR foo.age>18", e.ToString());
        }
    }
}
