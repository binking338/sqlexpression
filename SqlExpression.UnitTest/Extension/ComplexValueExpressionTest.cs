using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression.Extension;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class ComplexValueExpressionTest
    {
        [TestMethod]
        public void Test()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id > 1 & t.Id < 100;
                Assert.AreEqual("foo.id>1 AND foo.id<100", e.ToString());

                e = t.Id > 1 | t.Id < 100;
                Assert.AreEqual("foo.id>1 OR foo.id<100", e.ToString());

                e = t.Id > 1 & t.Id < 100 & t.Name.Like("%");
                Assert.AreEqual("foo.id>1 AND foo.id<100 AND foo.name LIKE '%'", e.ToString());

                e = t.Id > 1 | t.Id < 100 | t.Name.Like("%");
                Assert.AreEqual("foo.id>1 OR foo.id<100 OR foo.name LIKE '%'", e.ToString());

                e = t.Id > 1 & t.Id < 100 | t.Name.Like("%");
                Assert.AreEqual("foo.id>1 AND foo.id<100 OR foo.name LIKE '%'", e.ToString());

                e = t.Name.Like("%") | t.Id > 1 & t.Id < 100;
                Assert.AreEqual("foo.name LIKE '%' OR foo.id>1 AND foo.id<100", e.ToString());


                e = (t.Id > 1 | t.Id < 100) & t.Name.Like("%");
                Assert.AreEqual("(foo.id>1 OR foo.id<100) AND foo.name LIKE '%'", e.ToString());

                e = t.Name.Like("%") & (t.Id > 1 | t.Id < 100);
                Assert.AreEqual("foo.name LIKE '%' AND (foo.id>1 OR foo.id<100)", e.ToString());

                e = (t.Id > 1 | t.Id < 100) & (t.Name.Like("%") | t.Name.NotLike("wang%"));
                Assert.AreEqual("(foo.id>1 OR foo.id<100) AND (foo.name LIKE '%' OR foo.name NOT LIKE 'wang%')", e.ToString());

            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void Test2()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            Misc.UsingQuotationMark(() =>
            {
                e = (t.Id > 1)
                    .And(t.Id < 100);
                Assert.AreEqual("foo.id>1 AND foo.id<100", e.ToString());

                e = (t.Id > 1)
                    .Or(t.Id < 100);
                Assert.AreEqual("foo.id>1 OR foo.id<100", e.ToString());

                e = (t.Id > 1).And(t.Id < 100).And(t.Name.Like("%"));
                Assert.AreEqual("foo.id>1 AND foo.id<100 AND foo.name LIKE '%'", e.ToString());

                e = (t.Id > 1).Or(t.Id < 100).Or(t.Name.Like("%"));
                Assert.AreEqual("foo.id>1 OR foo.id<100 OR foo.name LIKE '%'", e.ToString());

                e = (t.Id > 1).And(t.Id < 100)
                    .Or(t.Name.Like("%"));
                Assert.AreEqual("foo.id>1 AND foo.id<100 OR foo.name LIKE '%'", e.ToString());

                e = t.Name.Like("%")
                   .Or((t.Id > 1).And(t.Id < 100));
                Assert.AreEqual("foo.name LIKE '%' OR foo.id>1 AND foo.id<100", e.ToString());


                e = (t.Id > 1).Or(t.Id < 100)
                    .And(t.Name.Like("%"));
                Assert.AreEqual("(foo.id>1 OR foo.id<100) AND foo.name LIKE '%'", e.ToString());

                e = t.Name.Like("%")
                    .And((t.Id > 1).Or(t.Id < 100));
                Assert.AreEqual("foo.name LIKE '%' AND (foo.id>1 OR foo.id<100)", e.ToString());

                e = ((t.Id > 1).Or(t.Id < 100))
                    .And(t.Name.Like("%").Or(t.Name.NotLike("wang%")));
                Assert.AreEqual("(foo.id>1 OR foo.id<100) AND (foo.name LIKE '%' OR foo.name NOT LIKE 'wang%')", e.ToString());

            }, string.Empty, string.Empty);
        }
    }
}
