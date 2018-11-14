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
        public void OperatorOverrideTestLiteralValueEnum()
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
        public void OperatorOverrideTestLiteralValueInt()
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
        public void OperatorOverrideTestLiteralValueDouble()
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
        public void OperatorOverrideTestLiteralValueBool()
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
        public void OperatorOverrideTestLiteralValueString()
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
        public void OperatorOverrideTestLiteralValueChar()
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
        public void OperatorOverrideTestLiteralValueDateTime()
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
                e = t.Id + val.ToString("yyyy-MM-dd HH:mm:ss");
                Assert.AreEqual("@id+'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id - val.ToString("yyyy-MM-dd HH:mm:ss");
                Assert.AreEqual("@id-'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id * val.ToString("yyyy-MM-dd HH:mm:ss");
                Assert.AreEqual("@id*'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id / val.ToString("yyyy-MM-dd HH:mm:ss");
                Assert.AreEqual("@id/'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());

                e = t.Id % val.ToString("yyyy-MM-dd HH:mm:ss");
                Assert.AreEqual("@id%'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'", e.ToString());


                e = val.ToString("yyyy-MM-dd HH:mm:ss") + t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'+@id", e.ToString());

                e = val.ToString("yyyy-MM-dd HH:mm:ss") - t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'-@id", e.ToString());

                e = val.ToString("yyyy-MM-dd HH:mm:ss") * t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'*@id", e.ToString());

                e = val.ToString("yyyy-MM-dd HH:mm:ss") / t.Id;
                Assert.AreEqual("'" + val.ToString("yyyy-MM-dd HH:mm:ss") + "'/@id", e.ToString());

                e = val.ToString("yyyy-MM-dd HH:mm:ss") % t.Id;
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
    }
}
