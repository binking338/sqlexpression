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
            var t = TestDb.Foo;
            IExpression e = null;

            var val = 1000;
            e = t.Id.Eq(val);
            Assert.AreEqual(e.ToString(), "foo.id=" + val);

            e = t.Id.Gt(val);
            Assert.AreEqual(e.ToString(), "foo.id>" + val);

            e = t.Id.Lt(val);
            Assert.AreEqual(e.ToString(), "foo.id<" + val);

            e = t.Id.GtOrEq(val);
            Assert.AreEqual(e.ToString(), "foo.id>=" + val);

            e = t.Id.LtOrEq(val);
            Assert.AreEqual(e.ToString(), "foo.id<=" + val);


            e = t.Id == val;
            Assert.AreEqual(e.ToString(), "foo.id=" + val);

            e = t.Id > val;
            Assert.AreEqual(e.ToString(), "foo.id>" + val);

            e = t.Id < val;
            Assert.AreEqual(e.ToString(), "foo.id<" + val);

            e = t.Id >= val;
            Assert.AreEqual(e.ToString(), "foo.id>=" + val);

            e = t.Id <= val;
            Assert.AreEqual(e.ToString(), "foo.id<=" + val);


            e = t.Name.Like("%");
            Assert.AreEqual(e.ToString(), "foo.name LIKE '%'");

            e = t.Name.NotLike("%");
            Assert.AreEqual(e.ToString(), "foo.name NOT LIKE '%'");

            e = t.Id.Between(100, 1000);
            Assert.AreEqual(e.ToString(), "foo.id BETWEEN 100 AND 1000");

            e = t.Id.NotBetween(100, 1000);
            Assert.AreEqual(e.ToString(), "foo.id NOT BETWEEN 100 AND 1000");

            e = t.Id.In(1, 2, 3, 4);
            Assert.AreEqual(e.ToString(), "foo.id IN (1,2,3,4)");

            e = t.Id.NotIn(1, 2, 3, 4);
            Assert.AreEqual(e.ToString(), "foo.id NOT IN (1,2,3,4)");

            e = t.Id.IsNull();
            Assert.AreEqual(e.ToString(), "foo.id IS NULL");

            e = t.Id.IsNotNull();
            Assert.AreEqual(e.ToString(), "foo.id IS NOT NULL");
        }

        /// <summary>
        /// 普通查询条件 参数化查询
        /// </summary>
        [TestMethod]
        public void ComparisonExpressionVarParam()
        {
            var t = TestDb.Foo;
            IExpression e = null;

            e = t.Id == t.Id.ToParam();
            Assert.AreEqual(e.ToString(), "foo.id=@id");

            e = t.Id > t.Id.ToParam();
            Assert.AreEqual(e.ToString(), "foo.id>@id");

            e = t.Id < t.Id.ToParam();
            Assert.AreEqual(e.ToString(), "foo.id<@id");

            e = t.Id >= t.Id.ToParam();
            Assert.AreEqual(e.ToString(), "foo.id>=@id");

            e = t.Id <= t.Id.ToParam();
            Assert.AreEqual(e.ToString(), "foo.id<=@id");


            e = t.Id.EqVarParam();
            Assert.AreEqual(e.ToString(), "foo.id=@id");

            e = t.Id.GtVarParam();
            Assert.AreEqual(e.ToString(), "foo.id>@id");

            e = t.Id.LtVarParam();
            Assert.AreEqual(e.ToString(), "foo.id<@id");

            e = t.Id.GtOrEqVarParam();
            Assert.AreEqual(e.ToString(), "foo.id>=@id");

            e = t.Id.LtOrEqVarParam();
            Assert.AreEqual(e.ToString(), "foo.id<=@id");


            e = t.Name.LikeVarParam();
            Assert.AreEqual(e.ToString(), "foo.name LIKE @name");

            e = t.Name.NotLikeVarParam();
            Assert.AreEqual(e.ToString(), "foo.name NOT LIKE @name");

            e = t.Id.BetweenVarParam();
            Assert.AreEqual(e.ToString(), "foo.id BETWEEN @idLower AND @idUpper");

            e = t.Id.NotBetweenVarParam();
            Assert.AreEqual(e.ToString(), "foo.id NOT BETWEEN @idLower AND @idUpper");
        }

        /// <summary>
        /// 逻辑条件
        /// </summary>
        [TestMethod]
        public void LogicExpression()
        {
            var t = TestDb.Foo;
            IExpression e = null;

            e = t.Id > 1 & t.Id < 100;
            Assert.AreEqual(e.ToString(), "foo.id>1 AND foo.id<100");

            e = (t.Id > 1).And(t.Id < 100);
            Assert.AreEqual(e.ToString(), "foo.id>1 AND foo.id<100");

            e = t.Id > 1 | t.Id < 100;
            Assert.AreEqual(e.ToString(), "foo.id>1 OR foo.id<100");

            e = (t.Id > 1).Or(t.Id < 100);
            Assert.AreEqual(e.ToString(), "foo.id>1 OR foo.id<100");

            e = t.Id > 1 & t.Id < 100 | t.Name.Like("%");
            Assert.AreEqual(e.ToString(), "foo.id>1 AND foo.id<100 OR foo.name LIKE '%'");

            e = t.Name.Like("%") | t.Id > 1 & t.Id < 100;
            Assert.AreEqual(e.ToString(), "foo.name LIKE '%' OR foo.id>1 AND foo.id<100");


            #region 根据调用顺序自动决定是否添加括号（结合SQL中 AND OR 的优先级）
            e = (t.Id > 1).And(t.Id < 100).Or(t.Name.Like("%"));
            Assert.AreEqual(e.ToString(), "foo.id>1 AND foo.id<100 OR foo.name LIKE '%'");

            e = t.Name.Like("%").Or((t.Id > 1).And(t.Id < 100));
            Assert.AreEqual(e.ToString(), "foo.name LIKE '%' OR foo.id>1 AND foo.id<100");

            e = t.Name.Like("%").Or(t.Id > 1).And(t.Id < 100);
            Assert.AreEqual(e.ToString(), "(foo.name LIKE '%' OR foo.id>1) AND foo.id<100");
            #endregion

            #region  根据C#表达式自动决定是否添加括号（结合运算符 & | 的优先级）
            e = (t.Id > 1 | t.Id < 100) & t.Name.Like("%");
            Assert.AreEqual(e.ToString(), "(foo.id>1 OR foo.id<100) AND foo.name LIKE '%'");

            e = t.Name.Like("%") & (t.Id > 1 | t.Id < 100);
            Assert.AreEqual(e.ToString(), "foo.name LIKE '%' AND (foo.id>1 OR foo.id<100)");

            e = t.Id > 1 | t.Id < 100 & t.Name.Like("%");
            Assert.AreEqual(e.ToString(), "foo.id>1 OR foo.id<100 AND foo.name LIKE '%'");
            #endregion

            #region 显式指定括号
            e = (t.Id > 1 | t.Id < 100).Bracket() & t.Name.Like("%");
            Assert.AreEqual(e.ToString(), "(foo.id>1 OR foo.id<100) AND foo.name LIKE '%'");

            e = t.Id > 1 | (t.Id < 100 & t.Name.Like("%")).Bracket();
            Assert.AreEqual(e.ToString(), "foo.id>1 OR (foo.id<100 AND foo.name LIKE '%')");

            e = (t.Name.Like("%") | t.Id > 1).Bracket() & t.Id < 100;
            Assert.AreEqual(e.ToString(), "(foo.name LIKE '%' OR foo.id>1) AND foo.id<100");
            #endregion
        }

        #endregion
    }
}
