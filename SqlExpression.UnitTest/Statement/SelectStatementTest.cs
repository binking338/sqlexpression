using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SqlExpression.Extension;
using SqlExpression.Extension.Dapper;

namespace SqlExpression.UnitTest.Statement
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SelectStatementTest
    {
        [TestMethod]
        public void SimpleQueryStatement()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Age >= 18)
                .Select(foo.Name)
                .GroupBy(foo.Name)
                .Having(foo.Name == "hero")
                .OrderBy(foo.Asterisk().Count().Desc());
            Assert.AreEqual("SELECT foo.name FROM foo WHERE foo.age>=18 GROUP BY foo.name HAVING foo.name='hero' ORDER BY COUNT(foo.*) DESC", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_SingleTable1()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_SingleTable2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.AllMapped());
            Assert.AreEqual("SELECT foo.id AS Id,foo.name AS Name,foo.age AS Age,foo.gender AS Gender,foo.isdel AS Isdel FROM foo WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_SingleTable3()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Select(foo.All())
                .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_JoinTable1()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Join(bar).On(foo.Name == bar.Name)
                .Where(foo.Name == "hero")
                .Select(foo.All());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_JoinTable2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Join(bar).On(foo.Name == bar.Name)
                .Where(foo.Name == "hero")
                .Select(foo.AllMapped());
            Assert.AreEqual("SELECT foo.id AS Id,foo.name AS Name,foo.age AS Age,foo.gender AS Gender,foo.isdel AS Isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_JoinTable3()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Join(bar).On(foo.Name == bar.Name)
                    .Select(foo.All())
                    .Where(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo JOIN bar ON foo.name=bar.name WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_JoinTable4()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var foo1 = foo.As("foo1");
            var exp = foo.Join(bar).On(foo.Name == bar.Name)
                         .Join(foo1).On(foo1.Id == foo.Id)
                         .Where(foo.Name == "hero")
                         .Select(foo.All());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo JOIN bar ON foo.name=bar.name JOIN foo AS foo1 ON foo1.id=foo.id WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_JoinTable5()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var foo1 = foo.As("foo1");
            var exp = foo.Join(bar.Join(foo1).On(foo1.Id == bar.Id))
                         .On(foo.Name == bar.Name)
                         .Where(foo.Name == "hero")
                         .Select(foo.All());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo JOIN (bar JOIN foo AS foo1 ON foo1.id=bar.id) ON foo.name=bar.name WHERE foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_GroupBy()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Age >= 18)
                .Select(foo.Name)
                .GroupBy(foo.Name)
                .Having(foo.Name == "hero");
            Assert.AreEqual("SELECT foo.name FROM foo WHERE foo.age>=18 GROUP BY foo.name HAVING foo.name='hero'", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_OrderBy()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .OrderBy(foo.Age.Desc());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' ORDER BY foo.age DESC", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_Exists()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .Exists();
            Assert.AreEqual("EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero')", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_Exists2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .OrderBy(foo.Age.Desc())
                .Exists();
            Assert.AreEqual("EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero')", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_NotExists()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .NotExists();
            Assert.AreEqual("NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero')", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_NotExists2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .OrderBy(foo.Age.Desc())
                .NotExists();
            Assert.AreEqual("NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero')", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_ToCountSql()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .ToCountSql();
            Assert.AreEqual("SELECT COUNT(*) AS __totalcount__ FROM (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero')", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_ToCountSql2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .OrderBy(foo.Age.Desc())
                .ToCountSql();
            Assert.AreEqual("SELECT COUNT(*) AS __totalcount__ FROM (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero')", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_ToExistsSql1()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .ToExistsSql();
            Assert.AreEqual("SELECT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero') AS __exists__", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_ToExistsSql2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .OrderBy(foo.Age.Desc())
                .ToExistsSql();
            Assert.AreEqual("SELECT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero') AS __exists__", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_ToNotExistsSql1()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .ToNotExistsSql();
            Assert.AreEqual("SELECT NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero') AS __notexists__", exp.ToString());
        }

        [TestMethod]
        public void SimpleQueryStatement_ToNotExistsSql2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var exp = foo
                .Where(foo.Name == "hero")
                .Select(foo.All())
                .OrderBy(foo.Age.Desc())
                .ToNotExistsSql();
            Assert.AreEqual("SELECT NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero') AS __notexists__", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
               .Union(bar.Select(bar.All()).Where(bar.Age == "hero"));
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero'", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_OrderBy()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .OrderBy(foo.Age.Desc());
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero' ORDER BY foo.age DESC", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_Exists()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .Exists();
            Assert.AreEqual("EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero')", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_Exists2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .OrderBy(foo.Age.Desc())
                      .Exists();
            Assert.AreEqual("EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero')", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_NotExists()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .NotExists();
            Assert.AreEqual("NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero')", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_NotExists2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .OrderBy(foo.Age.Desc())
                      .NotExists();
            Assert.AreEqual("NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero')", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_ToCountSql()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .ToCountSql();
            Assert.AreEqual("SELECT COUNT(*) AS __totalcount__ FROM (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero')", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_ToCountSql2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .OrderBy(foo.Age.Desc())
                      .ToCountSql();
            Assert.AreEqual("SELECT COUNT(*) AS __totalcount__ FROM (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero')", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_ToExistsSql()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .ToExistsSql();
            Assert.AreEqual("SELECT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero') AS __exists__", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_ToExistsSql2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .OrderBy(foo.Age.Desc())
                      .ToExistsSql();
            Assert.AreEqual("SELECT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero') AS __exists__", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_ToNotExistsSql()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                         .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                         .ToNotExistsSql();
            Assert.AreEqual("SELECT NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero') AS __notexists__", exp.ToString());
        }

        [TestMethod]
        public void UnionQueryStatement_ToNotExistsSql2()
        {
            TestDb db = new TestDb();
            var foo = db.Foo.Schema;
            var bar = db.Bar.Schema;
            var exp = foo.Select(foo.All()).Where(foo.Name == "hero")
                      .Union(bar.Select(bar.All()).Where(bar.Age == "hero"))
                      .OrderBy(foo.Age.Desc())
                      .ToNotExistsSql();
            Assert.AreEqual("SELECT NOT EXISTS (SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.name='hero' UNION SELECT bar.id,bar.name,bar.age,bar.gender,bar.isdel FROM bar WHERE bar.age='hero') AS __notexists__", exp.ToString());
        }

    }
}
