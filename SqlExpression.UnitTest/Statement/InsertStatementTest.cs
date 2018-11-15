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
    public class InsertStatementTest
    {
        [TestMethod]
        public void Test()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            IExpression e;

            e = (foo as IAliasTableExpression).Table.Insert(foo.Name, foo.Age).ValuesVarLiteral("hero", 18);
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES('hero',18)", e.ToString());

            e = (foo as IAliasTableExpression).Table.Insert(foo.Name, foo.Age).ValuesVarCustom("'hero'", "18");
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES('hero',18)", e.ToString());

            e = (foo as IAliasTableExpression).Table.Insert(foo.Name, foo.Age).ValuesVarParam();
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES(@Name,@Age)", e.ToString());

            e = (foo as IAliasTableExpression).Table.Insert(foo.Name, foo.Age).ValuesVarParam("val1", "val2");
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES(@val1,@val2)", e.ToString());

            e = (foo as IAliasTableExpression).Table.InsertVarParam(foo.Name, foo.Age);
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES(@Name,@Age)", e.ToString());

            e = (foo as IAliasTableExpression).Table.Insert().Columns(foo.Name, foo.Age).ValuesVarLiteral("hero", 18);
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES('hero',18)", e.ToString());

            e = (foo as IAliasTableExpression).Table.Insert().Columns(foo.Name, foo.Age).ValuesVarCustom("'hero'", "18");
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES('hero',18)", e.ToString());

            e = (foo as IAliasTableExpression).Table.Insert().Columns(foo.Name, foo.Age).ValuesVarParam();
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES(@Name,@Age)", e.ToString());
        }
    }
}
