using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class LiteralValueTest
    {
        [TestMethod]
        public void Test()
        {
            IExpression e;

            e = new LiteralValue(null);
            Assert.AreEqual("NULL", e.ToString());

            e = new LiteralValue(DBNull.Value);
            Assert.AreEqual("NULL", e.ToString());

            e = new LiteralValue(1);
            Assert.AreEqual("1", e.ToString());

            e = new LiteralValue("1");
            Assert.AreEqual("'1'", e.ToString());

            e = new LiteralValue('1');
            Assert.AreEqual("'1'", e.ToString());

            e = new LiteralValue(true);
            Assert.AreEqual("True", e.ToString());

            e = new LiteralValue(false);
            Assert.AreEqual("False", e.ToString());

            e = new LiteralValue(new DateTime(1970, 1, 1));
            Assert.AreEqual("'1970-01-01 00:00:00'", e.ToString());

            e = new LiteralValue(TestEnum.Item1);
            Assert.AreEqual("1", e.ToString());
        }
    }
}
