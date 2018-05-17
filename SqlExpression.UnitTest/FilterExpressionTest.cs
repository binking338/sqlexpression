using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression;
using SqlExpression.MySql;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class FilterExpressionTest
    {
        #region Filter

        /// <summary>
        /// 普通查询条件
        /// </summary>
        [TestMethod]
        public void CommonFilter()
        {
            var t = TestSchema.Instance;
            IExpression e;

            var oid = 1000;
            e = t.oid.Eq(oid);
            Assert.AreEqual(e.Expression, "test.oid=" + oid);

            e = t.oid.Gt(oid);
            Assert.AreEqual(e.Expression, "test.oid>" + oid);

            e = t.oid.Lt(oid);
            Assert.AreEqual(e.Expression, "test.oid<" + oid);

            e = t.oid.GtOrEq(oid);
            Assert.AreEqual(e.Expression, "test.oid>=" + oid);

            e = t.oid.LtOrEq(oid);
            Assert.AreEqual(e.Expression, "test.oid<=" + oid);


            e = t.oid == oid;
            Assert.AreEqual(e.Expression, "test.oid=" + oid);

            e = t.oid > oid;
            Assert.AreEqual(e.Expression, "test.oid>" + oid);

            e = t.oid < oid;
            Assert.AreEqual(e.Expression, "test.oid<" + oid);

            e = t.oid >= oid;
            Assert.AreEqual(e.Expression, "test.oid>=" + oid);

            e = t.oid <= oid;
            Assert.AreEqual(e.Expression, "test.oid<=" + oid);


            e = t.oname.Like("%");
            Assert.AreEqual(e.Expression, "test.oname LIKE '%'");

            e = t.oname.NotLike("%");
            Assert.AreEqual(e.Expression, "test.oname NOT LIKE '%'");

            e = t.oid.Between(100, 1000);
            Assert.AreEqual(e.Expression, "test.oid BETWEEN 100 AND 1000");

            e = t.oid.NotBetween(100, 1000);
            Assert.AreEqual(e.Expression, "test.oid NOT BETWEEN 100 AND 1000");

            e = t.oid.In(1, 2, 3, 4);
            Assert.AreEqual(e.Expression, "test.oid IN (1,2,3,4)");

            e = t.oid.NotIn(1, 2, 3, 4);
            Assert.AreEqual(e.Expression, "test.oid NOT IN (1,2,3,4)");

            e = t.oid.IsNull();
            Assert.AreEqual(e.Expression, "test.oid IS NULL");

            e = t.oid.IsNotNull();
            Assert.AreEqual(e.Expression, "test.oid IS NOT NULL");
        }

        /// <summary>
        /// 普通查询条件 参数化查询
        /// </summary>
        [TestMethod]
        public void CommonFilterVarParam()
        {
            var t = TestSchema.Instance;
            IExpression e;

            e = t.oid.EqVarParam();
            Assert.AreEqual(e.Expression, "test.oid=@oid");

            e = t.oid.GtVarParam();
            Assert.AreEqual(e.Expression, "test.oid>@oid");

            e = t.oid.LtVarParam();
            Assert.AreEqual(e.Expression, "test.oid<@oid");

            e = t.oid.GtOrEqVarParam();
            Assert.AreEqual(e.Expression, "test.oid>=@oid");

            e = t.oid.LtOrEqVarParam();
            Assert.AreEqual(e.Expression, "test.oid<=@oid");

            e = t.oname.LikeVarParam();
            Assert.AreEqual(e.Expression, "test.oname LIKE @oname");

            e = t.oname.NotLikeVarParam();
            Assert.AreEqual(e.Expression, "test.oname NOT LIKE @oname");

            e = t.oid.BetweenVarParam();
            Assert.AreEqual(e.Expression, "test.oid BETWEEN @oidLower AND @oidUpper");

            e = t.oid.NotBetweenVarParam();
            Assert.AreEqual(e.Expression, "test.oid NOT BETWEEN @oidLower AND @oidUpper");
        }

        [TestMethod]
        public void LogicFilter()
        {
            var t = TestSchema.Instance;
            IExpression e;

            e = t.oid > 1 & t.oid < 100;
            Assert.AreEqual(e.Expression, "test.oid>1 AND test.oid<100");

            e = (t.oid > 1).And(t.oid < 100);
            Assert.AreEqual(e.Expression, "test.oid>1 AND test.oid<100");

            e = t.oid > 1 | t.oid < 100;
            Assert.AreEqual(e.Expression, "test.oid>1 OR test.oid<100");

            e = (t.oid > 1).Or(t.oid < 100);
            Assert.AreEqual(e.Expression, "test.oid>1 OR test.oid<100");

            e = t.oid > 1 & t.oid < 100 | t.oname.Like("%");
            Assert.AreEqual(e.Expression, "test.oid>1 AND test.oid<100 OR test.oname LIKE '%'");

            e = t.oname.Like("%") | t.oid > 1 & t.oid < 100;
            Assert.AreEqual(e.Expression, "test.oname LIKE '%' OR test.oid>1 AND test.oid<100");


            // 扩展方法会自动加括号 加括号的规则会有C#语法约束
            e = (t.oid > 1).And(t.oid < 100).Or(t.oname.Like("%"));
            Assert.AreEqual(e.Expression, "(test.oid>1 AND test.oid<100) OR test.oname LIKE '%'");

            e = t.oname.Like("%").Or((t.oid > 1).And(t.oid < 100));
            Assert.AreEqual(e.Expression, "test.oname LIKE '%' OR (test.oid>1 AND test.oid<100)");

            e = t.oname.Like("%").Or(t.oid > 1).And(t.oid < 100);
            Assert.AreEqual(e.Expression, "(test.oname LIKE '%' OR test.oid>1) AND test.oid<100");


            // 运算符表达式不会自动加括号 需要自行决定
            e = (t.oid > 1 | t.oid < 100).WithBracket() & t.oname.Like("%");
            Assert.AreEqual(e.Expression, "(test.oid>1 OR test.oid<100) AND test.oname LIKE '%'");

            e = t.oid > 1 | (t.oid < 100 & t.oname.Like("%")).WithBracket();
            Assert.AreEqual(e.Expression, "test.oid>1 OR (test.oid<100 AND test.oname LIKE '%')");

            e = (t.oname.Like("%") | t.oid > 1).WithBracket() & t.oid < 100;
            Assert.AreEqual(e.Expression, "(test.oname LIKE '%' OR test.oid>1) AND test.oid<100");
        }

        #endregion

    }
}
