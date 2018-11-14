using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class ParamTest
    {
        [TestMethod]
        public void Test()
        {
            IExpression e;

            e = new Param("val");
            Assert.AreEqual("@val", e.ToString());

            Misc.UsingParamMark(() =>
            {
                e = new Param("val");
                Assert.AreEqual("?val", e.ToString());
            }, "?");
        }

        [TestMethod]
        public void TestOperatorOverrideByEnumLiteralValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;

            var val = TestEnum.Item1;

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+" + (int)val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-" + (int)val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*" + (int)val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/" + (int)val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%" + (int)val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual((int)val + "+@id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual((int)val + "-@id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual((int)val + "*@id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual((int)val + "/@id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual((int)val + "%@id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id=" + (int)val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("@id<>" + (int)val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>" + (int)val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<" + (int)val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>=" + (int)val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<=" + (int)val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual((int)val + "=@id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual((int)val + "<>@id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual((int)val + ">@id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual((int)val + "<@id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual((int)val + ">=@id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual((int)val + "<=@id", e.ToString());

            });
        }

        [TestMethod]
        public void TestOperatorOverrideByIntLiteralValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;

            var val = 1;

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%" + val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual(val + "+@id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual(val + "-@id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual(val + "*@id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual(val + "/@id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual(val + "%@id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id=" + val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("@id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<=" + val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual(val + "=@id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual(val + "<>@id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual(val + ">@id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual(val + "<@id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual(val + ">=@id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual(val + "<=@id", e.ToString());
            });
        }

        [TestMethod]
        public void TestOperatorOverrideByDoubleLiteralValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;

            var val = 1.0;

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%" + val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual(val + "+@id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual(val + "-@id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual(val + "*@id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual(val + "/@id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual(val + "%@id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id=" + val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("@id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<=" + val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual(val + "=@id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual(val + "<>@id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual(val + ">@id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual(val + "<@id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual(val + ">=@id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual(val + "<=@id", e.ToString());
            });
        }

        [TestMethod]
        public void TestOperatorOverrideByBoolLiteralValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;

            var val = true;

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%" + val, e.ToString());


                e = val + t.Id;
                Assert.AreEqual(val + "+@id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual(val + "-@id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual(val + "*@id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual(val + "/@id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual(val + "%@id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id=" + val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("@id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<=" + val, e.ToString());


                e = val == t.Id;
                Assert.AreEqual(val + "=@id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual(val + "<>@id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual(val + ">@id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual(val + "<@id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual(val + ">=@id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual(val + "<=@id", e.ToString());
            });
        }

        [TestMethod]
        public void TestOperatorOverrideByStringLiteralValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;

            var val = "1";

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+'" + val + "'", e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-'" + val + "'", e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*'" + val + "'", e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/'" + val + "'", e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%'" + val + "'", e.ToString());


                e = val + t.Id;
                Assert.AreEqual("'" + val + "'+@id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual("'" + val + "'-@id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual("'" + val + "'*@id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual("'" + val + "'/@id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual("'" + val + "'%@id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id='" + val + "'", e.ToString());

                e = t.Id != val;
                Assert.AreEqual("@id<>'" + val + "'", e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>'" + val + "'", e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<'" + val + "'", e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>='" + val + "'", e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<='" + val + "'", e.ToString());


                e = val == t.Id;
                Assert.AreEqual("'" + val + "'=@id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual("'" + val + "'<>@id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual("'" + val + "'>@id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual("'" + val + "'<@id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual("'" + val + "'>=@id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual("'" + val + "'<=@id", e.ToString());
            });
        }

        [TestMethod]
        public void TestOperatorOverrideByCharLiteralValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;

            var val = '1';

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+'" + val + "'", e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-'" + val + "'", e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*'" + val + "'", e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/'" + val + "'", e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%'" + val + "'", e.ToString());


                e = val + t.Id;
                Assert.AreEqual("'" + val + "'+@id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual("'" + val + "'-@id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual("'" + val + "'*@id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual("'" + val + "'/@id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual("'" + val + "'%@id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id='" + val + "'", e.ToString());

                e = t.Id != val;
                Assert.AreEqual("@id<>'" + val + "'", e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>'" + val + "'", e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<'" + val + "'", e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>='" + val + "'", e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<='" + val + "'", e.ToString());


                e = val == t.Id;
                Assert.AreEqual("'" + val + "'=@id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual("'" + val + "'<>@id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual("'" + val + "'>@id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual("'" + val + "'<@id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual("'" + val + "'>=@id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual("'" + val + "'<=@id", e.ToString());
            });
        }

        [TestMethod]
        public void TestOperatorOverrideByDateTimeLiteralValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;

            var val = new DateTime(1970, 1, 1);

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());


                e = val + t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'+@id", e.ToString());

                e = val - t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'-@id", e.ToString());

                e = val * t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'*@id", e.ToString());

                e = val / t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'/@id", e.ToString());

                e = val % t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'%@id", e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id='" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());
                e = t.Id != val;
                Assert.AreEqual("@id<>'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>='" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<='" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());


                e = val == t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'=@id", e.ToString());

                e = val != t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'<>@id", e.ToString());

                e = val > t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'>@id", e.ToString());

                e = val < t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'<@id", e.ToString());

                e = val >= t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'>=@id", e.ToString());

                e = val <= t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'<=@id", e.ToString());
            });
        }

        [TestMethod]
        public void TestOperatorOverrideByISimpleValue()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;
            ISimpleValue val = LiteralValue.Parse(1);

            Misc.UsingParamMark(() =>
            {
                e = t.Id + val;
                Assert.AreEqual("@id+" + val, e.ToString());

                e = t.Id - val;
                Assert.AreEqual("@id-" + val, e.ToString());

                e = t.Id * val;
                Assert.AreEqual("@id*" + val, e.ToString());

                e = t.Id / val;
                Assert.AreEqual("@id/" + val, e.ToString());

                e = t.Id % val;
                Assert.AreEqual("@id%" + val, e.ToString());


                e = t.Id == val;
                Assert.AreEqual("@id=" + val, e.ToString());
                e = t.Id != val;
                Assert.AreEqual("@id<>" + val, e.ToString());

                e = t.Id > val;
                Assert.AreEqual("@id>" + val, e.ToString());

                e = t.Id < val;
                Assert.AreEqual("@id<" + val, e.ToString());

                e = t.Id >= val;
                Assert.AreEqual("@id>=" + val, e.ToString());

                e = t.Id <= val;
                Assert.AreEqual("@id<=" + val, e.ToString());
            });
        }

        [TestMethod]
        public void TestOperatorOverrideLogicExpression()
        {
            var t = new
            {
                Id = new Param("id"),
                Name = new Param("name")
            };
            IExpression e = null;
            LiteralValue l = LiteralValue.Parse(true);
            ISimpleValue val = l;

            Misc.UsingQuotationMark(() =>
            {
                e = t.Id & val;
                Assert.AreEqual("@id AND True", e.ToString());
                e = t.Id | val;
                Assert.AreEqual("@id OR True", e.ToString());

                e = t.Id & l;
                Assert.AreEqual("@id AND True", e.ToString());
                e = t.Id | l;
                Assert.AreEqual("@id OR True", e.ToString());

                e = l & t.Id;
                Assert.AreEqual("True AND @id", e.ToString());
                e = l | t.Id;
                Assert.AreEqual("True OR @id", e.ToString());

            }, string.Empty, string.Empty);
        }
    }
}
