using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;


namespace SqlExpression.UnitTest
{
    [TestClass]
    public class ColumnTest
    {
        [TestMethod]
        public void Test1()
        {
            IExpression e;

            e = new Column("id");
            Assert.AreEqual("id", e.ToString());

            e = new Column("id", "foo");
            Assert.AreEqual("foo.id", e.ToString());

            e = new Column("*", "foo");
            Assert.AreEqual("foo.*", e.ToString());
        }

        [TestMethod]
        public void Test2()
        {
            IExpression e;

            Expression.DefaultOption.OpenQuotationMark = "`";
            Expression.DefaultOption.CloseQuotationMark = "`";

            e = new Column("id");
            Assert.AreEqual("`id`", e.ToString());

            e = new Column("id", "foo");
            Assert.AreEqual("`foo`.`id`", e.ToString());

            e = new Column("*", "foo");
            Assert.AreEqual("`foo`.*", e.ToString());

            Expression.DefaultOption.OpenQuotationMark = "";
            Expression.DefaultOption.CloseQuotationMark = "";
        }

        [TestMethod]
        public void ExtensionAliasTest()
        {
            IExpression e;

            e = new Column("id").As("Id");
            Assert.AreEqual("id AS Id", e.ToString());

            e = new Column("id", "foo").As("Id");
            Assert.AreEqual("foo.id AS Id", e.ToString());
        }

        [TestMethod]
        public void ExtensionOrderTest()
        {
            IExpression e;

            e = new Column("id").Asc();
            Assert.AreEqual("id ASC", e.ToString());

            e = new Column("id").Desc();
            Assert.AreEqual("id DESC", e.ToString());
        }

        [TestMethod]
        public void ExtensionAsteriskTest()
        {
            IExpression e;

            e = new Table("foo").Asterisk();
            Assert.AreEqual("foo.*", e.ToString());
        }

        [TestMethod]
        public void ExtensionToParamTest()
        {
            IExpression e;

            e = new Column("id").ToParam();
            Assert.AreEqual("@id", e.ToString());

            e = new Column("id").ToP();
            Assert.AreEqual("@id", e.ToString());

            e = new Column("id").ToParam("val");
            Assert.AreEqual("@val", e.ToString());

            e = new Column("id").ToP("val");
            Assert.AreEqual("@val", e.ToString());
        }
        
        [TestMethod]
        public void ExtensionSetTest()
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

            e = new Column("id").Set(new DateTime(1970,1,1));
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
    }
    public enum TestEnum
    {
        Item1 = 1,
        Item2 = 2,
    }
}
