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

            LiteralValue literal;

            literal = (int)1;
            Assert.AreEqual("1", literal.ToString());

            literal = (short)1;
            Assert.AreEqual("1", literal.ToString());

            literal = (ushort)1;
            Assert.AreEqual("1", literal.ToString());

            literal = (long)1;
            Assert.AreEqual("1", literal.ToString());

            literal = (ulong)1;
            Assert.AreEqual("1", literal.ToString());

            literal = (uint)1;
            Assert.AreEqual("1", literal.ToString());

            literal = 1d;
            Assert.AreEqual("1", literal.ToString());

            literal = 1f;
            Assert.AreEqual("1", literal.ToString());

            literal = 1m;
            Assert.AreEqual("1", literal.ToString());

            literal = "1";
            Assert.AreEqual("'1'", literal.ToString());

            literal = '1';
            Assert.AreEqual("'1'", literal.ToString());

            literal = true;
            Assert.AreEqual("True", literal.ToString());

            literal = false;
            Assert.AreEqual("False", literal.ToString());

            literal = new DateTime(1970, 1, 1);
            Assert.AreEqual("'1970-01-01 00:00:00'", literal.ToString());

            literal = TestEnum.Item1;
            Assert.AreEqual("1", literal.ToString());
        }

        [TestMethod]
        public void TestOperatorOverrideByISimpleValue()
        {
            IExpression e = null;
            LiteralValue literal = LiteralValue.Parse(1);

            e = literal + literal;
            Assert.AreEqual(literal.ToString() + "+" + literal.ToString(), e.ToString());

            e = literal - literal;
            Assert.AreEqual(literal.ToString() + "-" + literal.ToString(), e.ToString());

            e = literal * literal;
            Assert.AreEqual(literal.ToString() + "*" + literal.ToString(), e.ToString());

            e = literal / literal;
            Assert.AreEqual(literal.ToString() + "/" + literal.ToString(), e.ToString());

            e = literal % literal;
            Assert.AreEqual(literal.ToString() + "%" + literal.ToString(), e.ToString());


            e = literal == literal;
            Assert.AreEqual(literal.ToString() + "=" + literal.ToString(), e.ToString());

            e = literal != literal;
            Assert.AreEqual(literal.ToString() + "<>" + literal.ToString(), e.ToString());

            e = literal > literal;
            Assert.AreEqual(literal.ToString() + ">" + literal.ToString(), e.ToString());

            e = literal < literal;
            Assert.AreEqual(literal.ToString() + "<" + literal.ToString(), e.ToString());

            e = literal >= literal;
            Assert.AreEqual(literal.ToString() + ">=" + literal.ToString(), e.ToString());

            e = literal <= literal;
            Assert.AreEqual(literal.ToString() + "<=" + literal.ToString(), e.ToString());
        }

        [TestMethod]
        public void TestOperatorOverrideLogicExpression()
        {
            IExpression e = null;
            LiteralValue literal = LiteralValue.Parse(true);

            e = literal & literal;
            Assert.AreEqual("True AND True", e.ToString());
            e = literal | literal;
            Assert.AreEqual("True OR True", e.ToString());

        }
    }
}
