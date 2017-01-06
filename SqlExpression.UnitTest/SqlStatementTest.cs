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
    public class SqlStatementTest
    {
        ISqlStatement statement;

        #region Filter

        [TestMethod]
        public void Filter()
        {

        }

        #endregion

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

        #region Select

        [TestMethod]
        public void Select()
        {
            statement = TestSchema.Select((sql, s) => sql.Get(s.All()));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.oid).Where(s.oid > 1));
            Assert.AreEqual("SELECT test.oid FROM test WHERE test.oid>1", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.age).OrderBy(s.age.Desc()));
            Assert.AreEqual("SELECT test.age FROM test ORDER BY test.age DESC", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.gender, s.age.Avg()).GroupBy(s.gender).Having(s.age.Avg() < 40));
            Assert.AreEqual("SELECT test.gender,AVG(test.age) FROM test GROUP BY test.gender HAVING AVG(test.age)<40", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.All()).Where(s.oid > 1).OrderBy(s.age.Desc()));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test WHERE test.oid>1 ORDER BY test.age DESC", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.age.Avg()).Where(s.oid > 1).GroupBy(s.gender).Having(s.age.Avg() > 18).OrderBy(s.age.Desc()));
            Assert.AreEqual("SELECT AVG(test.age) FROM test WHERE test.oid>1 GROUP BY test.gender HAVING AVG(test.age)>18 ORDER BY test.age DESC", statement.ToString());

            statement = TestSchema.Table.Select(TestSchema.Instance.oid);
            Assert.AreEqual("SELECT test.oid FROM test", statement.ToString());

            statement = TestSchema.Table.SelectVarCustomer("test.oid");
            Assert.AreEqual("SELECT test.oid FROM test", statement.ToString());

            statement = TestSchema.Table.Select().Get(TestSchema.Instance.oid);
            Assert.AreEqual("SELECT test.oid FROM test", statement.ToString());

            statement = TestSchema.Table.Select().GetVarCustomer("test.oid");
            Assert.AreEqual("SELECT test.oid FROM test", statement.ToString());

            statement = TestSchema.Table.Select().GetAs(TestSchema.Instance.oid, "a");
            Assert.AreEqual("SELECT test.oid AS a FROM test", statement.ToString());
        }

        [TestMethod]
        public void Join()
        {
            statement = TestSchema.Select((sql, s) => sql.Get(s.All()).InnerJoin(FooSchema.Table, FooSchema.Filter((foo) => s.oid == foo.oid)));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test INNER JOIN foo ON test.oid=foo.oid", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.All()).LeftJoin(FooSchema.Table, FooSchema.Filter((foo) => s.oid == foo.oid)));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test LEFT JOIN foo ON test.oid=foo.oid", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.All()).RightJoin(FooSchema.Table, FooSchema.Filter((foo) => s.oid == foo.oid)));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test RIGHT JOIN foo ON test.oid=foo.oid", statement.ToString());

            statement = TestSchema.Select((sql, s) => sql.Get(s.All()).FullJoin(FooSchema.Table, FooSchema.Filter((foo) => s.oid == foo.oid)));
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.gender,test.isdel FROM test FULL JOIN foo ON test.oid=foo.oid", statement.ToString());
        }

        [TestMethod]
        public void Union()
        {
            statement = FooSchema.Select((sql, s) => sql.Get(s.oid, s.oname))
                .Union(BarSchema.Select((sql, s) => sql.Get(s.oid, s.oname)));
            Assert.AreEqual("SELECT foo.oid,foo.oname FROM foo UNION SELECT bar.oid,bar.oname FROM bar", statement.ToString());
        }

        #endregion

        #region Update

        [TestMethod]
        public void Update()
        {
            statement = FooSchema.Update((sql, s) => sql.Set(s.oname, "foo2").Where(s.oid == 1));
            Assert.AreEqual("UPDATE foo SET foo.oname='foo2' WHERE foo.oid=1", statement.ToString());

            statement = FooSchema.Table.Update().Set(FooSchema.Instance.oname, "foo2").Where(FooSchema.Instance.oid == 1);
            Assert.AreEqual("UPDATE foo SET foo.oname='foo2' WHERE foo.oid=1", statement.ToString());

            statement = FooSchema.Table.Update().SetVarCustomer(FooSchema.Instance.oname, "@oname").SetC(FooSchema.Instance.isdel, "1").Where(FooSchema.Instance.oid == 1);
            Assert.AreEqual("UPDATE foo SET foo.oname=@oname,foo.isdel=1 WHERE foo.oid=1", statement.ToString());
               　
            statement = FooSchema.Table.Update().SetVarParam(FooSchema.Instance.oname).SetP(FooSchema.Instance.isdel).Where(FooSchema.Instance.oid == 1);
            Assert.AreEqual("UPDATE foo SET foo.oname=@oname,foo.isdel=@isdel WHERE foo.oid=1", statement.ToString());
        }

        #endregion

        #region Insert

        [TestMethod]
        public void Insert()
        {
            statement = FooSchema.Insert((sql, s) => sql.Set(s.oid, 1).Set(s.oname, "foo1"));
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(1,'foo1')", statement.ToString());

            statement = FooSchema.Table.Insert().Columns(FooSchema.Instance.oid, FooSchema.Instance.oname).Values(1, "foo1");
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(1,'foo1')", statement.ToString());

            statement = FooSchema.Table.Insert().Set(FooSchema.Instance.oid, 1).Set(FooSchema.Instance.oname, "foo1");
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(1,'foo1')", statement.ToString());

            statement = FooSchema.Table.Insert().SetC(FooSchema.Instance.oid, "1").SetVarCustomer(FooSchema.Instance.oname, "'foo1'");   
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(1,'foo1')", statement.ToString());

            statement = FooSchema.Table.Insert().SetP(FooSchema.Instance.oid).SetVarParam(FooSchema.Instance.oname);
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(@oid,@oname)", statement.ToString());

            statement = FooSchema.Table.Insert(FooSchema.Instance.oid, FooSchema.Instance.oname).Values(1, "foo1");
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(1,'foo1')", statement.ToString());

            statement = FooSchema.Table.Insert(FooSchema.Instance.oid, FooSchema.Instance.oname).ValuesVarCustomer("1", "'foo1'");
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(1,'foo1')", statement.ToString());

            statement = FooSchema.Table.Insert(FooSchema.Instance.oid, FooSchema.Instance.oname).ValuesVarParam();
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(@oid,@oname)", statement.ToString());

            statement = FooSchema.Table.Insert(FooSchema.Instance.oid, FooSchema.Instance.oname).ValuesFillNull();
            Assert.AreEqual("INSERT INTO foo(foo.oid,foo.oname) VALUES(NULL,NULL)", statement.ToString());
        }

        #endregion

        #region Delete

        [TestMethod]
        public void Delete()
        {
            statement = FooSchema.Delete((sql, s) => sql.Where(s.oid == 1));
            Assert.AreEqual("DELETE FROM foo WHERE foo.oid=1", statement.ToString());

            statement = FooSchema.Table.Delete().Where(FooSchema.Instance.oid == 1);
            Assert.AreEqual("DELETE FROM foo WHERE foo.oid=1", statement.ToString());
        }

        #endregion
    }
}
