using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression.Extension;
using System.Linq;

namespace SqlExpression.UnitTest
{
    /// <summary>
    /// 过滤条件测试
    /// </summary>
    [TestClass]
    public class FilterTest
    {
        #region Filter

        /// <summary>
        /// 普通查询条件
        /// </summary>
        [TestMethod]
        public void ComparisonExpression()
        {
            TestDb db = new TestDb();
            var t = db.Foo.Schema;
            IExpression e = null;

            var val = 1000;
            e = t.Id.Eq(val);
            Assert.AreEqual("foo.id=" + val, e.ToString());

            e = t.Id.Gt(val);
            Assert.AreEqual("foo.id>" + val, e.ToString());

            e = t.Id.Lt(val);
            Assert.AreEqual("foo.id<" + val, e.ToString());

            e = t.Id.GtOrEq(val);
            Assert.AreEqual("foo.id>=" + val, e.ToString());

            e = t.Id.LtOrEq(val);
            Assert.AreEqual("foo.id<=" + val, e.ToString());


            e = t.Id == val;
            Assert.AreEqual("foo.id=" + val, e.ToString());

            e = t.Id > val;
            Assert.AreEqual("foo.id>" + val, e.ToString());

            e = t.Id < val;
            Assert.AreEqual("foo.id<" + val, e.ToString());

            e = t.Id >= val;
            Assert.AreEqual("foo.id>=" + val, e.ToString());

            e = t.Id <= val;
            Assert.AreEqual("foo.id<=" + val, e.ToString());


            e = t.Name.Like("%");
            Assert.AreEqual("foo.name LIKE '%'", e.ToString());

            e = t.Name.NotLike("%");
            Assert.AreEqual("foo.name NOT LIKE '%'", e.ToString());

            e = t.Id.Between(100, 1000);
            Assert.AreEqual("foo.id BETWEEN 100 AND 1000", e.ToString());

            e = t.Id.NotBetween(100, 1000);
            Assert.AreEqual("foo.id NOT BETWEEN 100 AND 1000", e.ToString());

            e = t.Id.In(1, 2, 3, 4);
            Assert.AreEqual("foo.id IN (1,2,3,4)", e.ToString());

            e = t.Id.NotIn(1, 2, 3, 4);
            Assert.AreEqual("foo.id NOT IN (1,2,3,4)", e.ToString());

            e = t.Id.IsNull();
            Assert.AreEqual("foo.id IS NULL", e.ToString());

            e = t.Id.IsNotNull();
            Assert.AreEqual("foo.id IS NOT NULL", e.ToString());
        }

        /// <summary>
        /// 普通查询条件 参数化查询
        /// </summary>
        [TestMethod]
        public void ComparisonExpressionVarParam()
        {
            TestDb db = new TestDb();
            var t = db.Foo.Schema;
            IExpression e = null;

            e = t.Id == t.Id.ToParam();
            Assert.AreEqual("foo.id=@id", e.ToString());

            e = t.Id > t.Id.ToParam();
            Assert.AreEqual("foo.id>@id", e.ToString());

            e = t.Id < t.Id.ToParam();
            Assert.AreEqual("foo.id<@id", e.ToString());

            e = t.Id >= t.Id.ToParam();
            Assert.AreEqual("foo.id>=@id", e.ToString());

            e = t.Id <= t.Id.ToParam();
            Assert.AreEqual("foo.id<=@id", e.ToString());


            e = t.Id.EqVarParam();
            Assert.AreEqual("foo.id=@id", e.ToString());

            e = t.Id.GtVarParam();
            Assert.AreEqual("foo.id>@id", e.ToString());

            e = t.Id.LtVarParam();
            Assert.AreEqual("foo.id<@id", e.ToString());

            e = t.Id.GtOrEqVarParam();
            Assert.AreEqual("foo.id>=@id", e.ToString());

            e = t.Id.LtOrEqVarParam();
            Assert.AreEqual("foo.id<=@id", e.ToString());


            e = t.Name.LikeVarParam();
            Assert.AreEqual("foo.name LIKE @name", e.ToString());

            e = t.Name.NotLikeVarParam();
            Assert.AreEqual("foo.name NOT LIKE @name", e.ToString());

            e = t.Id.BetweenVarParam();
            Assert.AreEqual("foo.id BETWEEN @idLower AND @idUpper", e.ToString());

            e = t.Id.NotBetweenVarParam();
            Assert.AreEqual("foo.id NOT BETWEEN @idLower AND @idUpper", e.ToString());
        }

        /// <summary>
        /// 逻辑条件
        /// </summary>
        [TestMethod]
        public void LogicExpression()
        {
            TestDb db = new TestDb();
            var t = db.Foo.Schema;
            IExpression e = null;

            e = t.Id > 1 & t.Id < 100;
            Assert.AreEqual("foo.id>1 AND foo.id<100", e.ToString());

            e = (t.Id > 1).And(t.Id < 100);
            Assert.AreEqual("foo.id>1 AND foo.id<100", e.ToString());

            e = t.Id > 1 | t.Id < 100;
            Assert.AreEqual("foo.id>1 OR foo.id<100", e.ToString());

            e = (t.Id > 1).Or(t.Id < 100);
            Assert.AreEqual("foo.id>1 OR foo.id<100", e.ToString());

            e = t.Id > 1 & t.Id < 100 | t.Name.Like("%");
            Assert.AreEqual("foo.id>1 AND foo.id<100 OR foo.name LIKE '%'", e.ToString());

            e = t.Name.Like("%") | t.Id > 1 & t.Id < 100;
            Assert.AreEqual("foo.name LIKE '%' OR foo.id>1 AND foo.id<100", e.ToString());


            #region 根据调用顺序自动决定是否添加括号（结合SQL中 AND OR 的优先级）
            e = (t.Id > 1).And(t.Id < 100).Or(t.Name.Like("%"));
            Assert.AreEqual("foo.id>1 AND foo.id<100 OR foo.name LIKE '%'", e.ToString());

            e = t.Name.Like("%").Or((t.Id > 1).And(t.Id < 100));
            Assert.AreEqual("foo.name LIKE '%' OR foo.id>1 AND foo.id<100", e.ToString());

            e = t.Name.Like("%").Or(t.Id > 1).And(t.Id < 100);
            Assert.AreEqual("(foo.name LIKE '%' OR foo.id>1) AND foo.id<100", e.ToString());
            #endregion

            #region  根据C#表达式自动决定是否添加括号（结合运算符 & | 的优先级）
            e = (t.Id > 1 | t.Id < 100) & t.Name.Like("%");
            Assert.AreEqual("(foo.id>1 OR foo.id<100) AND foo.name LIKE '%'", e.ToString());

            e = t.Name.Like("%") & (t.Id > 1 | t.Id < 100);
            Assert.AreEqual("foo.name LIKE '%' AND (foo.id>1 OR foo.id<100)", e.ToString());

            e = t.Id > 1 | t.Id < 100 & t.Name.Like("%");
            Assert.AreEqual("foo.id>1 OR foo.id<100 AND foo.name LIKE '%'", e.ToString());
            #endregion

            #region 显式指定括号
            e = (t.Id > 1 | t.Id < 100).Bracket() & t.Name.Like("%");
            Assert.AreEqual("(foo.id>1 OR foo.id<100) AND foo.name LIKE '%'", e.ToString());

            e = t.Id > 1 | (t.Id < 100 & t.Name.Like("%")).Bracket();
            Assert.AreEqual("foo.id>1 OR (foo.id<100 AND foo.name LIKE '%')", e.ToString());

            e = (t.Name.Like("%") | t.Id > 1).Bracket() & t.Id < 100;
            Assert.AreEqual("(foo.name LIKE '%' OR foo.id>1) AND foo.id<100", e.ToString());
            #endregion
        }

        #endregion
    }
}
