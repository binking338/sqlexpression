using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;


namespace SqlExpression.UnitTest
{
    [TestClass]
    public class TableTest
    {
        [TestMethod]
        public void Test1()
        {
            var e = new Table("foo");

            Assert.AreEqual("foo", e.ToString());
        }

        [TestMethod]
        public void Test2()
        {
            Expression.DefaultOption.OpenQuotationMark = "`";
            Expression.DefaultOption.CloseQuotationMark = "`";
            var e = new Table("foo");

            Assert.AreEqual("`foo`", e.ToString());

            Expression.DefaultOption.OpenQuotationMark = "";
            Expression.DefaultOption.CloseQuotationMark = "";
        }

        [TestMethod]
        public void TestAsExtension1()
        {
            var e = new Table("foo").As("t");

            Assert.AreEqual("foo AS t", e.ToString());
        }

        [TestMethod]
        public void TestAsExtension2()
        {
            Expression.DefaultOption.OpenQuotationMark = "`";
            Expression.DefaultOption.CloseQuotationMark = "`";
            var e = new Table("foo").As("t");

            Assert.AreEqual("`foo` AS `t`", e.ToString());

            Expression.DefaultOption.OpenQuotationMark = "";
            Expression.DefaultOption.CloseQuotationMark = "";
        }
    }
}
