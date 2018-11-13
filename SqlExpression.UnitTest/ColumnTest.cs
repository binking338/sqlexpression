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
        public void Test()
        {
            IExpression e;

            e = new Column("id");
            Assert.AreEqual("id", e.ToString());

            e = new Column("id", "foo");
            Assert.AreEqual("foo.id", e.ToString());

            e = new Column("*", "foo");
            Assert.AreEqual("foo.*", e.ToString());

            Misc.UsingQuotationMark(() => {

                e = new Column("id");
                Assert.AreEqual("`id`", e.ToString());

                e = new Column("id", "foo");
                Assert.AreEqual("`foo`.`id`", e.ToString());

                e = new Column("*", "foo");
                Assert.AreEqual("`foo`.*", e.ToString());

            });
        }
    }
}
