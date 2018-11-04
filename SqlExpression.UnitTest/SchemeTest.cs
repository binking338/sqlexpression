using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression.UnitTest.Scheme;
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
            var foo = new Foo();
            var exp = foo
                .Select(foo.GetAllFields())
                .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero'", exp.Expression);
            exp = foo
                .Select(foo.GetAllFieldsMapping())
                .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id AS Id,foo.name AS Name,foo.age AS Age,foo.gender AS Gender,foo.isdel AS Isdel FROM foo WHERE foo.name='hero'", exp.Expression);
        }
        [TestMethod]
        public void JoinStatement()
        {
            var foo = new Foo();
            var bar = new Bar();
            var exp = foo.Join(bar).On(foo.Name == bar.Name)
                .Select(foo.GetAllFields())
                .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.Expression);
            exp = foo.Join(bar).On(foo.Name == bar.Name)
                .Select(foo.GetAllFieldsMapping())
                .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id AS Id,foo.name AS Name,foo.age AS Age,foo.gender AS Gender,foo.isdel AS Isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.Expression);
        }
    }
}
