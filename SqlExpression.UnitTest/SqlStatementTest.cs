﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExpression;
using SqlExpression.MySql;

namespace SqlExpression.UnitTest
{
    [TestClass]
    public class SqlStatementTest
    {
        [TestMethod]
        public void Select()
        {
            ISelectStatement statement;
            statement = TestSchema.Select((sql, s) => { return sql.Get(s.All()); });
            Assert.AreEqual("SELECT test.oid,test.oname,test.age,test.isdel FROM test", statement.ToString());

            statement = TestSchema.Select((sql, s) => { return sql.Get(s.oid).Where(s.oid > 1); });
            Assert.AreEqual("SELECT test.oid FROM test WHERE test.oid>1", statement.ToString());
            
        }
    }
}
