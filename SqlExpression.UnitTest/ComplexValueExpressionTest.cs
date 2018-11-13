using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class OperatorOverrideTest
    {
        [TestMethod]
        public void Test()
        {
            var t = new
            {
                Id = new Column("id", "foo"),
                Name = new Column("name", "foo")
            };
            IExpression e = null;


            e = t.Id > 1 & t.Id < 100;
            Assert.AreEqual("foo.id>1 AND foo.id<100", e.ToString());

            e = t.Id > 1 | t.Id < 100;
            Assert.AreEqual("foo.id>1 OR foo.id<100", e.ToString());

            e = t.Id > 1 & t.Id < 100 | t.Name.Like("%");
            Assert.AreEqual("foo.id>1 AND foo.id<100 OR foo.name LIKE '%'", e.ToString());

            e = t.Name.Like("%") | t.Id > 1 & t.Id < 100;
            Assert.AreEqual("foo.name LIKE '%' OR foo.id>1 AND foo.id<100", e.ToString());


            #region  根据C#表达式自动决定是否添加括号（结合运算符 & | 的优先级）
            e = (t.Id > 1 | t.Id < 100) & t.Name.Like("%");
            Assert.AreEqual("(foo.id>1 OR foo.id<100) AND foo.name LIKE '%'", e.ToString());

            e = t.Name.Like("%") & (t.Id > 1 | t.Id < 100);
            Assert.AreEqual("foo.name LIKE '%' AND (foo.id>1 OR foo.id<100)", e.ToString());

            e = t.Id > 1 | t.Id < 100 & t.Name.Like("%");
            Assert.AreEqual("foo.id>1 OR foo.id<100 AND foo.name LIKE '%'", e.ToString());
            #endregion
        }
    }
}
