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
    public class UpdateStatementTest
    {
        [TestMethod]
        public void Test()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            IExpression e;

            e = (foo as IAliasTableExpression).Table.Update(foo.Name, foo.Age).ValuesVarLiteral("hero", 18);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update(foo.Name, foo.Age).ValuesVarCustom("'hero'", "18");
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update(foo.Name, foo.Age).ValuesVarParam();
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update(foo.Name, foo.Age).ValuesVarParam("val1", "val2");
            Assert.AreEqual("UPDATE foo SET foo.name=@val1,foo.age=@val2", e.ToString());

            e = (foo as IAliasTableExpression).Table.UpdateVarParam(foo.Name, foo.Age);
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update().SetVarLiteral(foo.Name, "hero").SetVarLiteral(foo.Age, 18);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update().SetVarCustom(foo.Name, "'hero'").SetVarCustom(foo.Age, "18");
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update().SetVarParam(foo.Name).SetVarParam(foo.Age);
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update().SetVarParam(foo.Name, "val1").SetVarParam(foo.Age, "val2");
            Assert.AreEqual("UPDATE foo SET foo.name=@val1,foo.age=@val2", e.ToString());

            e = (foo as IAliasTableExpression).Table.Update().SetVarLiteral(foo.Name, "hero").SetVarLiteral(foo.Age, 18).Where(foo.Id==1);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18 WHERE foo.id=1", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Update().SetVarLiteral(foo.Name, "hero").SetVarLiteral(foo.Age, 18);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18 WHERE foo.id=1", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Update().SetVarLiteral(foo.Name, "hero").SetVarLiteral(foo.Age, 18).Where(foo.Age > 18);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18 WHERE foo.id=1 AND foo.age>18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Update().SetVarLiteral(foo.Name, "hero").SetVarLiteral(foo.Age, 18).OrWhere(foo.Age > 18);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18 WHERE foo.id=1 OR foo.age>18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).Where(foo.Age > 18).Update().SetVarLiteral(foo.Name, "hero").SetVarLiteral(foo.Age, 18);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18 WHERE foo.id=1 AND foo.age>18", e.ToString());

            e = (foo as IAliasTableExpression).Table.Where(foo.Id == 1).OrWhere(foo.Age > 18).Update().SetVarLiteral(foo.Name, "hero").SetVarLiteral(foo.Age, 18);
            Assert.AreEqual("UPDATE foo SET foo.name='hero',foo.age=18 WHERE foo.id=1 OR foo.age>18", e.ToString());
        }
    }
}
