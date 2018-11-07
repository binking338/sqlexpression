using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class SchemeTest
    {
        [TestMethod]
        public void SimpleStatement()
        {
            var foo = Db.Foo;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.GetAllFields());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero'", exp.ToString());

            exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.GetAllFieldsMapping());
            Assert.AreEqual("SELECT foo.id AS Id,foo.name AS Name,foo.age AS Age,foo.gender AS Gender,foo.isdel AS Isdel FROM foo WHERE foo.name='hero'", exp.ToString());

            exp = foo
                .Select(foo.GetAllFields())
                .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void JoinStatement()
        {
            var foo = Db.Foo;
            var bar = Db.Bar;
            var exp = foo.Join(bar).On(foo.Name == bar.Name)
                .Where(foo.Name == "hero")
                .Select(foo.GetAllFields());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.ToString());

            exp = foo.Join(bar).On(foo.Name == bar.Name)
                .Where(foo.Name == "hero")
                .Select(foo.GetAllFieldsMapping());
            Assert.AreEqual("SELECT foo.id AS Id,foo.name AS Name,foo.age AS Age,foo.gender AS Gender,foo.isdel AS Isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.ToString());

            exp = foo.Join(bar).On(foo.Name == bar.Name)
                    .Select(foo.GetAllFields())
                    .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.ToString());
        }
    }
}
