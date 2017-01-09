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
    public class ParamsTest
    {
        ISqlStatement statement;

        #region Params

        [TestMethod]
        public void Params()
        {
            statement = FooSchema.Insert((sql, s) => sql.Set(s.oid, 1).Set(s.oname, "foo1"));
            Assert.AreEqual(SortedJoin(statement.Params), "");
            statement = FooSchema.Table.Insert().SetP(FooSchema.Instance.oid).SetVarParam(FooSchema.Instance.oname);
            Assert.AreEqual(SortedJoin(statement.Params), "oid,oname");
            statement = FooSchema.Table.Insert().SetP(FooSchema.Instance.oid).SetVarParam(FooSchema.Instance.oname);
            Assert.AreEqual(SortedJoin(statement.Params), "oid,oname");

            statement = FooSchema.Delete((sql, s) => sql);
            Assert.AreEqual(SortedJoin(statement.Params), "");
            statement = FooSchema.Delete((sql, s) => sql.Where(s.oid.EqVarParam()));
            Assert.AreEqual(SortedJoin(statement.Params), "oid");

            statement = FooSchema.Update((sql, s) => sql.Set(s.oname, "foo2"));
            Assert.AreEqual(SortedJoin(statement.Params), "");
            statement = FooSchema.Table.Update().SetVarParam(FooSchema.Instance.oname).SetP(FooSchema.Instance.isdel).Where(FooSchema.Instance.oid.EqVarParam());
            Assert.AreEqual(SortedJoin(statement.Params), "isdel,oid,oname");

            statement = TestSchema.Select((sql, s) => sql.Get(s.age.Avg()).Where(s.oid.GtVarParam()).GroupBy(s.gender).Having(18 < s.age.Avg()).OrderBy(s.age.Desc()));
            Assert.AreEqual(SortedJoin(statement.Params), "oid");
        }

        private string SortedJoin(IEnumerable<string> strs, string splitter = ",")
        {
            var list = strs.ToList();
            list.Sort();
            return list.Join(splitter);
        }

        #endregion

    }
}
