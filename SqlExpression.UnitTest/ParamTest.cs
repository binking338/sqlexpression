using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class ParamTest
    {
        [TestMethod]
        public void Test()
        {
            IExpression e;

            e = new Param("val");
            Assert.AreEqual("@val", e.ToString());

            Misc.UsingParamMark(() =>
            {
                e = new Param("val");
                Assert.AreEqual("?val", e.ToString());
            }, "?");
        }
    }
}
