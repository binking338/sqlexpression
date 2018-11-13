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

            e = LiteralValue.Null;
            Assert.AreEqual("NULL", e.ToString());

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



            e = LiteralValue.Parse(null);
            Assert.AreEqual("NULL", e.ToString());

            e = LiteralValue.Parse(DBNull.Value);
            Assert.AreEqual("NULL", e.ToString());

            e = LiteralValue.Parse(1);
            Assert.AreEqual("1", e.ToString());

            e = LiteralValue.Parse("1");
            Assert.AreEqual("'1'", e.ToString());

            e = LiteralValue.Parse('1');
            Assert.AreEqual("'1'", e.ToString());

            e = LiteralValue.Parse(true);
            Assert.AreEqual("True", e.ToString());

            e = LiteralValue.Parse(false);
            Assert.AreEqual("False", e.ToString());

            e = LiteralValue.Parse(new DateTime(1970, 1, 1));
            Assert.AreEqual("'1970-01-01 00:00:00'", e.ToString());

            e = LiteralValue.Parse(TestEnum.Item1);
            Assert.AreEqual("1", e.ToString());
        }
    }
}
