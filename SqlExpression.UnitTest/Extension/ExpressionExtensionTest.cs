using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;


namespace SqlExpression.UnitTest.Extension
{
    [TestClass]
    public class ExpressionExtensionTest
    {
        [TestMethod]
        public void ConcatTest()
        {
            IExpression e;

            e = (new CustomExpression("select * from a")).Concat(new CustomExpression("select * from b"));
            Assert.AreEqual("select * from a;select * from b", e.ToString());
        }

        [TestMethod]
        public void BracketTest()
        {
            IExpression e;

            e = (new CustomExpression("foo=1")).Bracket();
            Assert.AreEqual("(foo=1)", e.ToString());
        }

        [TestMethod]
        public void AliasTest()
        {
            IExpression e;

            e = new Table("foo").As("t");
            Assert.AreEqual("foo AS t", e.ToString());

            e = new LiteralValue("id").As("Id");
            Assert.AreEqual("'id' AS Id", e.ToString());

            e = new Column("id").As("Id");
            Assert.AreEqual("id AS Id", e.ToString());

            e = new Column("id", "foo").As("Id");
            Assert.AreEqual("foo.id AS Id", e.ToString());

            e = new SubQueryExpression(new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { new SelectItemExpression(new LiteralValue(1)) }))).As("Val");
            Assert.AreEqual("(SELECT 1) AS Val", e.ToString());
        }

        [TestMethod]
        public void UsingQuotationMarkAliasTest()
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

                e = new SubQueryExpression(new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { new SelectItemExpression(new LiteralValue(1)) }))).As("Val");
                Assert.AreEqual("(SELECT 1) AS `Val`", e.ToString());
            });
        }

        [TestMethod]
        public void AsteriskTest()
        {
            IExpression e;

            e = new Table("foo").Asterisk();
            Assert.AreEqual("foo.*", e.ToString());
        }

        [TestMethod]
        public void ToParamTest()
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
        public void SetTest()
        {
            IExpression e;

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
        }

        [TestMethod]
        public void OrderEnumTest()
        {
            IExpression e;

            e = new Column("id").Asc();
            Assert.AreEqual("id ASC", e.ToString());

            e = new Column("id").Desc();
            Assert.AreEqual("id DESC", e.ToString());
        }

        [TestMethod]
        public void FunctionTest()
        {
            IExpression e;

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
        }


        [TestMethod]
        public void LogicTest()
        {
            IExpression e;

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
        }


        [TestMethod]
        public void ComparisonTest()
        {
            IExpression e;

            e = new Column("id").IsNull();
            Assert.AreEqual("id IS NULL", e.ToString());

            e = new Column("id").IsNotNull();
            Assert.AreEqual("id IS NOT NULL", e.ToString());

        }
    }
}