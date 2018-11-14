using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression.Extension;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SqlExpression.UnitTest.Extension
{
    [TestClass]
    public class ExpressionExtensionTest
    {
        [TestMethod]
        public void TestConcat()
        {
            IExpression e;

            e = (new CustomExpression("select * from a")).Concat(new CustomExpression("select * from b"));
            Assert.AreEqual("select * from a;select * from b", e.ToString());
        }

        [TestMethod]
        public void TestBracket()
        {
            IExpression e;

            e = (new CustomExpression("foo=1")).Bracket();
            Assert.AreEqual("(foo=1)", e.ToString());
        }

        [TestMethod]
        public void TestAlias()
        {
            IExpression e;
            Misc.UsingQuotationMark(() =>
            {
                e = new Table("foo").As("t");
                Assert.AreEqual("foo AS t", e.ToString());

                e = LiteralValue.Parse("id").As("Id");
                Assert.AreEqual("'id' AS Id", e.ToString());

                e = new Column("id").As("Id");
                Assert.AreEqual("id AS Id", e.ToString());

                e = new Column("id", "foo").As("Id");
                Assert.AreEqual("foo.id AS Id", e.ToString());

                e = new SubQueryExpression(new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { new SelectItemExpression(LiteralValue.Parse(1)) }))).As("Val");
                Assert.AreEqual("(SELECT 1) AS Val", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestAliasUsingQuotationMark()
        {
            IExpression e;
            Misc.UsingQuotationMark(() =>
            {
                e = new Table("foo").As("t");
                Assert.AreEqual("`foo` AS `t`", e.ToString());

                e = new LiteralValue("id").As("Id");
                Assert.AreEqual("'id' AS `Id`", e.ToString());

                e = new Column("id").As("Id");
                Assert.AreEqual("`id` AS `Id`", e.ToString());

                e = new Column("id", "foo").As("Id");
                Assert.AreEqual("`foo`.`id` AS `Id`", e.ToString());

                e = new SubQueryExpression(new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { new SelectItemExpression(LiteralValue.Parse(1)) }))).As("Val");
                Assert.AreEqual("(SELECT 1) AS `Val`", e.ToString());
            });
        }

        [TestMethod]
        public void TestAsterisk()
        {
            IExpression e;

            Misc.UsingQuotationMark(() =>
            {
                e = new Table("foo").Asterisk();
                Assert.AreEqual("foo.*", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestExtensionToParam()
        {
            IExpression e;
            Misc.UsingParamNameAsColumnName(() =>
            {
                e = new Column("id").ToParam();
                Assert.AreEqual("@id", e.ToString());

                e = new Column("id").ToP();
                Assert.AreEqual("@id", e.ToString());

                e = new Column("id").ToParam("val");
                Assert.AreEqual("@val", e.ToString());

                e = new Column("id").ToP("val");
                Assert.AreEqual("@val", e.ToString());

            });
        }

        [TestMethod]
        public void TestExtensionSet()
        {
            IExpression e;

            Misc.UsingQuotationMark(() =>
            {
                e = new Column("id").Set(null);
                Assert.AreEqual("id=NULL", e.ToString());

                e = new Column("id").Set(1);
                Assert.AreEqual("id=1", e.ToString());

                e = new Column("id").Set("1");
                Assert.AreEqual("id='1'", e.ToString());

                e = new Column("id").Set('1');
                Assert.AreEqual("id='1'", e.ToString());

                e = new Column("id").Set(true);
                Assert.AreEqual("id=True", e.ToString());

                e = new Column("id").Set(false);
                Assert.AreEqual("id=False", e.ToString());

                e = new Column("id").Set(new DateTime(1970, 1, 1));
                Assert.AreEqual("id='1970-01-01 00:00:00'", e.ToString());

                e = new Column("id").Set(TestEnum.Item1);
                Assert.AreEqual("id=1", e.ToString());

                e = new Column("id").SetVarParam();
                Assert.AreEqual("id=@id", e.ToString());

                e = new Column("id").SetVarParam("val");
                Assert.AreEqual("id=@val", e.ToString());

                e = new Column("id").SetP();
                Assert.AreEqual("id=@id", e.ToString());

                e = new Column("id").SetP("val");
                Assert.AreEqual("id=@val", e.ToString());

                e = new Column("id").SetVarCustom("1");
                Assert.AreEqual("id=1", e.ToString());

                e = new Column("id").SetVarCustom("'1'");
                Assert.AreEqual("id='1'", e.ToString());

                e = new Column("id").SetVarCustom("true");
                Assert.AreEqual("id=true", e.ToString());

                e = new Column("id").SetC("1");
                Assert.AreEqual("id=1", e.ToString());

                e = new Column("id").SetC("'1'");
                Assert.AreEqual("id='1'", e.ToString());

                e = new Column("id").SetC("true");
                Assert.AreEqual("id=true", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestExtensionOrderEnum()
        {
            IExpression e;

            Misc.UsingQuotationMark(() =>
            {
                e = new Column("id").Asc();
                Assert.AreEqual("id ASC", e.ToString());

                e = new Column("id").Desc();
                Assert.AreEqual("id DESC", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestExtensionFunction()
        {
            IExpression e;

            Misc.UsingQuotationMark(() =>
            {
                e = new Column("id").Sum();
                Assert.AreEqual("SUM(id)", e.ToString());

                e = new Column("id").Count();
                Assert.AreEqual("COUNT(id)", e.ToString());

                e = new Column("id").Avg();
                Assert.AreEqual("AVG(id)", e.ToString());

                e = new Column("id").Min();
                Assert.AreEqual("MIN(id)", e.ToString());

                e = new Column("id").Max();
                Assert.AreEqual("MAX(id)", e.ToString());
            }, string.Empty, string.Empty);
        }

        [TestMethod]
        public void TestExtensionArithmetic()
        {
            IExpression e;

            Misc.UsingQuotationMark(() =>
            {
                e = new Column("id").Add(1);
                Assert.AreEqual("id+1", e.ToString());

                e = new Column("id").Sub(1);
                Assert.AreEqual("id-1", e.ToString());

                e = new Column("id").Mul(1);
                Assert.AreEqual("id*1", e.ToString());

                e = new Column("id").Div(1);
                Assert.AreEqual("id/1", e.ToString());

                e = new Column("id").Mod(1);
                Assert.AreEqual("id%1", e.ToString());
            }, string.Empty, string.Empty);
        }


        [TestMethod]
        public void TestExtensionLogicExpression()
        {
            IExpression e;

            Misc.UsingQuotationMark(() =>
            {
                e = new LiteralValue(true).And(true);
                Assert.AreEqual("True AND True", e.ToString());

                e = new LiteralValue(true).Or(true);
                Assert.AreEqual("True OR True", e.ToString());

                e = new LiteralValue(true).And(null);
                Assert.AreEqual("True AND NULL", e.ToString());

                e = new LiteralValue(true).Or(null);
                Assert.AreEqual("True OR NULL", e.ToString());

                e = new LiteralValue(true).And(new LiteralValue(true));
                Assert.AreEqual("True AND True", e.ToString());

                e = new LiteralValue(true).Or(new LiteralValue(true));
                Assert.AreEqual("True OR True", e.ToString());

                e = new LiteralValue[] { LiteralValue.Parse(true), LiteralValue.Parse(true), LiteralValue.Parse(true) }.AllSatisfied();
                Assert.AreEqual("True AND True AND True", e.ToString());

                e = new LiteralValue[] { LiteralValue.Parse(true), LiteralValue.Parse(true), LiteralValue.Parse(true) }.AnySatisfied();
                Assert.AreEqual("True OR True OR True", e.ToString());
            }, string.Empty, string.Empty);

            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };


            Misc.UsingParamNameAsColumnName(() =>
            {
                e = new List<Column>() { t.Id, t.Name }.AllEqVarParam();
                Assert.AreEqual("foo.id=@id AND foo.name=@name", e.ToString());

                e = new List<Column>() { t.Id, t.Name }.AllEqP();
                Assert.AreEqual("foo.id=@id AND foo.name=@name", e.ToString());

                e = new List<Column>() { t.Id, t.Name }.AnyEqVarParam();
                Assert.AreEqual("foo.id=@id OR foo.name=@name", e.ToString());

                e = new List<Column>() { t.Id, t.Name }.AnyEqP();
                Assert.AreEqual("foo.id=@id OR foo.name=@name", e.ToString());
            });
        }


        [TestMethod]
        public void TestExtensionComparisonExpression()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;

            Misc.UsingQuotationMark(() =>
            {
                var val = 1000;
                e = t.Id.Eq(val);
                Assert.AreEqual("foo.id=" + val, e.ToString());

                e = t.Id.Gt(val);
                Assert.AreEqual("foo.id>" + val, e.ToString());

                e = t.Id.Lt(val);
                Assert.AreEqual("foo.id<" + val, e.ToString());

                e = t.Id.GtOrEq(val);
                Assert.AreEqual("foo.id>=" + val, e.ToString());

                e = t.Id.LtOrEq(val);
                Assert.AreEqual("foo.id<=" + val, e.ToString());

                e = t.Name.Like("%");
                Assert.AreEqual("foo.name LIKE '%'", e.ToString());

                e = t.Name.NotLike("%");
                Assert.AreEqual("foo.name NOT LIKE '%'", e.ToString());

                e = t.Id.Between(100, 1000);
                Assert.AreEqual("foo.id BETWEEN 100 AND 1000", e.ToString());

                e = t.Id.NotBetween(100, 1000);
                Assert.AreEqual("foo.id NOT BETWEEN 100 AND 1000", e.ToString());

                e = t.Id.In(1, 2, 3, 4);
                Assert.AreEqual("foo.id IN (1,2,3,4)", e.ToString());

                e = t.Id.NotIn(1, 2, 3, 4);
                Assert.AreEqual("foo.id NOT IN (1,2,3,4)", e.ToString());

                e = t.Id.IsNull();
                Assert.AreEqual("foo.id IS NULL", e.ToString());

                e = t.Id.IsNotNull();
                Assert.AreEqual("foo.id IS NOT NULL", e.ToString());



                e = t.Id.EqVarParam("val");
                Assert.AreEqual("foo.id=@val", e.ToString());

                e = t.Id.GtVarParam("val");
                Assert.AreEqual("foo.id>@val", e.ToString());

                e = t.Id.LtVarParam("val");
                Assert.AreEqual("foo.id<@val", e.ToString());

                e = t.Id.GtOrEqVarParam("val");
                Assert.AreEqual("foo.id>=@val", e.ToString());

                e = t.Id.LtOrEqVarParam("val");
                Assert.AreEqual("foo.id<=@val", e.ToString());


                e = t.Name.LikeVarParam("val");
                Assert.AreEqual("foo.name LIKE @val", e.ToString());

                e = t.Name.NotLikeVarParam("val");
                Assert.AreEqual("foo.name NOT LIKE @val", e.ToString());


                e = t.Id.BetweenVarParam("val1", "val2");
                Assert.AreEqual("foo.id BETWEEN @val1 AND @val2", e.ToString());

                e = t.Id.NotBetweenVarParam("val1", "val2");
                Assert.AreEqual("foo.id NOT BETWEEN @val1 AND @val2", e.ToString());

                Misc.UsingParamNameAsColumnName(() =>
                {
                    e = t.Id.EqVarParam();
                    Assert.AreEqual("foo.id=@id", e.ToString());

                    e = t.Id.GtVarParam();
                    Assert.AreEqual("foo.id>@id", e.ToString());

                    e = t.Id.LtVarParam();
                    Assert.AreEqual("foo.id<@id", e.ToString());

                    e = t.Id.GtOrEqVarParam();
                    Assert.AreEqual("foo.id>=@id", e.ToString());

                    e = t.Id.LtOrEqVarParam();
                    Assert.AreEqual("foo.id<=@id", e.ToString());


                    e = t.Name.LikeVarParam();
                    Assert.AreEqual("foo.name LIKE @name", e.ToString());

                    e = t.Name.NotLikeVarParam();
                    Assert.AreEqual("foo.name NOT LIKE @name", e.ToString());

                    e = t.Id.BetweenVarParam();
                    Assert.AreEqual("foo.id BETWEEN @idLower AND @idUpper", e.ToString());

                    e = t.Id.NotBetweenVarParam();
                    Assert.AreEqual("foo.id NOT BETWEEN @idLower AND @idUpper", e.ToString());
                });


                Misc.UsingParamName2UpperCamalCase(() =>
                {
                    e = t.Id == t.Id.ToParam();
                    Assert.AreEqual("foo.id=@Id", e.ToString());

                    e = t.Id > t.Id.ToParam();
                    Assert.AreEqual("foo.id>@Id", e.ToString());

                    e = t.Id < t.Id.ToParam();
                    Assert.AreEqual("foo.id<@Id", e.ToString());

                    e = t.Id >= t.Id.ToParam();
                    Assert.AreEqual("foo.id>=@Id", e.ToString());

                    e = t.Id <= t.Id.ToParam();
                    Assert.AreEqual("foo.id<=@Id", e.ToString());


                    e = t.Id.EqVarParam();
                    Assert.AreEqual("foo.id=@Id", e.ToString());

                    e = t.Id.GtVarParam();
                    Assert.AreEqual("foo.id>@Id", e.ToString());

                    e = t.Id.LtVarParam();
                    Assert.AreEqual("foo.id<@Id", e.ToString());

                    e = t.Id.GtOrEqVarParam();
                    Assert.AreEqual("foo.id>=@Id", e.ToString());

                    e = t.Id.LtOrEqVarParam();
                    Assert.AreEqual("foo.id<=@Id", e.ToString());


                    e = t.Name.LikeVarParam();
                    Assert.AreEqual("foo.name LIKE @Name", e.ToString());

                    e = t.Name.NotLikeVarParam();
                    Assert.AreEqual("foo.name NOT LIKE @Name", e.ToString());


                    e = t.Id.BetweenVarParam();
                    Assert.AreEqual("foo.id BETWEEN @IdLower AND @IdUpper", e.ToString());

                    e = t.Id.NotBetweenVarParam();
                    Assert.AreEqual("foo.id NOT BETWEEN @IdLower AND @IdUpper", e.ToString());
                });

            }, string.Empty, string.Empty);
        }
    }
}