using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace SqlExpression.UnitTest
{
    [TestClass]
    public class ColumnTest
    {
        [TestMethod]
        public void Test()
        {
            IExpression e;

            Misc.UsingQuotationMark(() =>
            {

                e = new Column("id");
                Assert.AreEqual("id", e.ToString());

                e = new Column("id", "foo");
                Assert.AreEqual("foo.id", e.ToString());

                e = new Column("*", "foo");
                Assert.AreEqual("foo.*", e.ToString());

            }, string.Empty, string.Empty);

            Misc.UsingQuotationMark(() =>
            {

                e = new Column("id");
                Assert.AreEqual("`id`", e.ToString());

                e = new Column("id", "foo");
                Assert.AreEqual("`foo`.`id`", e.ToString());

                e = new Column("*", "foo");
                Assert.AreEqual("`foo`.*", e.ToString());

            });
        }

        [TestMethod]
        public void TestOperatorOverrideByEnumLiteralValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            var val = TestEnum.Item1;

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+" + (int)val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-" + (int)val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*" + (int)val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/" + (int)val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%" + (int)val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual((int)val + "+foo.id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual((int)val + "-foo.id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual((int)val + "*foo.id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual((int)val + "/foo.id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual((int)val + "%foo.id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id=" + (int)val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("foo.id<>" + (int)val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>" + (int)val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<" + (int)val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>=" + (int)val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<=" + (int)val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual((int)val + "=foo.id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual((int)val + "<>foo.id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual((int)val + ">foo.id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual((int)val + "<foo.id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual((int)val + ">=foo.id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual((int)val + "<=foo.id", e.ToString());

            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideByIntLiteralValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            var val = 1;

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%" + val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual(val + "+foo.id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual(val + "-foo.id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual(val + "*foo.id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual(val + "/foo.id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual(val + "%foo.id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id=" + val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("foo.id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<=" + val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual(val + "=foo.id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual(val + "<>foo.id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual(val + ">foo.id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual(val + "<foo.id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual(val + ">=foo.id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual(val + "<=foo.id", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideByDoubleLiteralValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            var val = 1.0;

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%" + val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual(val + "+foo.id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual(val + "-foo.id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual(val + "*foo.id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual(val + "/foo.id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual(val + "%foo.id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id=" + val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("foo.id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<=" + val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual(val + "=foo.id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual(val + "<>foo.id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual(val + ">foo.id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual(val + "<foo.id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual(val + ">=foo.id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual(val + "<=foo.id", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideByBoolLiteralValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            var val = true;

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%" + val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual(val + "+foo.id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual(val + "-foo.id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual(val + "*foo.id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual(val + "/foo.id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual(val + "%foo.id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id=" + val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("foo.id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<=" + val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual(val + "=foo.id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual(val + "<>foo.id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual(val + ">foo.id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual(val + "<foo.id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual(val + ">=foo.id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual(val + "<=foo.id", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideByStringLiteralValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            var val = "1";

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+'" + val + "'", e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-'" + val + "'", e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*'" + val + "'", e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/'" + val + "'", e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%'" + val + "'", e.ToString());


                e = val + t.Id;
                Assert.AreEqual("'" + val + "'+foo.id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual("'" + val + "'-foo.id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual("'" + val + "'*foo.id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual("'" + val + "'/foo.id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual("'" + val + "'%foo.id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id='" + val + "'", e.ToString());

                e = t.Id != val;
                Assert.AreEqual("foo.id<>'" + val + "'", e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>'" + val + "'", e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<'" + val + "'", e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>='" + val + "'", e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<='" + val + "'", e.ToString());


                e = val == t.Id;
                Assert.AreEqual("'" + val + "'=foo.id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual("'" + val + "'<>foo.id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual("'" + val + "'>foo.id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual("'" + val + "'<foo.id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual("'" + val + "'>=foo.id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual("'" + val + "'<=foo.id", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideByCharLiteralValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            var val = '1';

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+'" + val + "'", e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-'" + val + "'", e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*'" + val + "'", e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/'" + val + "'", e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%'" + val + "'", e.ToString());


                e = val + t.Id;
                Assert.AreEqual("'" + val + "'+foo.id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual("'" + val + "'-foo.id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual("'" + val + "'*foo.id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual("'" + val + "'/foo.id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual("'" + val + "'%foo.id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id='" + val + "'", e.ToString());

                e = t.Id != val;
                Assert.AreEqual("foo.id<>'" + val + "'", e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>'" + val + "'", e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<'" + val + "'", e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>='" + val + "'", e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<='" + val + "'", e.ToString());


                e = val == t.Id;
                Assert.AreEqual("'" + val + "'=foo.id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual("'" + val + "'<>foo.id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual("'" + val + "'>foo.id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual("'" + val + "'<foo.id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual("'" + val + "'>=foo.id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual("'" + val + "'<=foo.id", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideByDateTimeLiteralValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            var val = new DateTime(1970, 1, 1);

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());


                e = val + t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'+foo.id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'-foo.id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'*foo.id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'/foo.id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'%foo.id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id='" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id != val;
                Assert.AreEqual("foo.id<>'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>='" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<='" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());


                e = val == t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'=foo.id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'<>foo.id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'>foo.id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'<foo.id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'>=foo.id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'<=foo.id", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideByISimpleValue()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;
            ISimpleValue val = LiteralValue.Parse(1);

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("foo.id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("foo.id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("foo.id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("foo.id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("foo.id%" + val, e.ToString());


                e = t.Id == val;
                Assert.AreEqual("foo.id=" + val, e.ToString());

                e = t.Id != val;
                Assert.AreEqual("foo.id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("foo.id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("foo.id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("foo.id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("foo.id<=" + val, e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestOperatorOverrideLogicExpression()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;
            LiteralValue literal = LiteralValue.Parse(true);
            ISimpleValue val = literal;

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id & val;
                Assert.AreEqual("foo.id AND True", e.ToString());
                e = t.Id | val;
                Assert.AreEqual("foo.id OR True", e.ToString());

                e = t.Id & literal;
                Assert.AreEqual("foo.id AND True", e.ToString());
                e = t.Id | literal;
                Assert.AreEqual("foo.id OR True", e.ToString());

                e = literal & t.Id;
                Assert.AreEqual("True AND foo.id", e.ToString());
                e = literal | t.Id;
                Assert.AreEqual("True OR foo.id", e.ToString());

            }, string.Empty, string.Empty);
        }
    }
}
