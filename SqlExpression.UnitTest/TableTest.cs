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
            var e = new Table("foo");

            Misc.UsingQuotationMark(() =>
            {
                Assert.AreEqual("`foo`", e.ToString());
            });
        }

        [TestMethod]
        public void ExtensionAliasTest1()
        {
            var e = new Table("foo").As("t");

            Assert.AreEqual("foo AS t", e.ToString());
        }

        [TestMethod]
        public void ExtensionAliasTest2()
        {
            var e = new Table("foo").As("t");

            Misc.UsingQuotationMark(() =>
            {
                Assert.AreEqual("`foo` AS `t`", e.ToString());
            });
        }

        [TestMethod]
        public void ExtensionAsteriskTest()
        {
            IExpression e;

            e = new Table("foo").Asterisk();
            Assert.AreEqual("foo.*", e.ToString());
        }
    }
}
