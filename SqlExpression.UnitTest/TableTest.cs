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
        public void Test()
        {
            var e = new Table("foo");
            Assert.AreEqual("foo", e.ToString());

            Misc.UsingQuotationMark(() =>
            {
                e = new Table("foo");
                Assert.AreEqual("`foo`", e.ToString());
            });
        }
    }
}
