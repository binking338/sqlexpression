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

            LiteralValue l;

            l = (int)1;
            Assert.AreEqual("1", l.ToString());

            l = (short)1;
            Assert.AreEqual("1", l.ToString());

            l = (ushort)1;
            Assert.AreEqual("1", l.ToString());

            l = (long)1;
            Assert.AreEqual("1", l.ToString());

            l = (ulong)1;
            Assert.AreEqual("1", l.ToString());

            l = (uint)1;
            Assert.AreEqual("1", l.ToString());

            l = 1d;
            Assert.AreEqual("1", l.ToString());

            l = 1f;
            Assert.AreEqual("1", l.ToString());

            l = 1m;
            Assert.AreEqual("1", l.ToString());

            l = "1";
            Assert.AreEqual("'1'", l.ToString());

            l = '1';
            Assert.AreEqual("'1'", l.ToString());

            l = true;
            Assert.AreEqual("True", l.ToString());

            l = false;
            Assert.AreEqual("False", l.ToString());

            l = new DateTime(1970, 1, 1);
            Assert.AreEqual("'1970-01-01 00:00:00'", l.ToString());

            l = TestEnum.Item1;
            Assert.AreEqual("1", l.ToString());
        }
    }
}
