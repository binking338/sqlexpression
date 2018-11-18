using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using SqlExpression.Extension;
using SqlExpression.Extension.Dapper;

namespace SqlExpression.UnitTest.Statement
{
    [TestClass]
    public class DbContextTest
    {
        [TestMethod]
        public void Test()
        {
            var connection = FakeItEasy.A.Fake<IDbConnection>();
            var command = FakeItEasy.A.Fake<IDbCommand>();
            var reader = FakeItEasy.A.Fake<IDataReader>();
            FakeItEasy.A.CallTo(() => connection.CreateCommand()).ReturnsLazily(args => command);
            FakeItEasy.A.CallTo(() => command.ExecuteNonQuery()).ReturnsLazily(args => 1);
            FakeItEasy.A.CallTo(() => command.ExecuteScalar()).ReturnsLazily(args => 1);
            FakeItEasy.A.CallTo(() => command.ExecuteReader()).ReturnsLazily((arg) => reader);
            FakeItEasy.A.CallTo(() => reader.NextResult()).ReturnsLazily(args => false);
            FakeItEasy.A.CallTo(() => reader.Read()).ReturnsLazily(args => false);

            var db = new TestDb(connection);
            var entity = new Entity.Foo() { Id = 0, Name = "hero", Age = 18 };

            db.Foo.Insert(entity);
            Assert.AreEqual("INSERT INTO foo(foo.id,foo.name,foo.age,foo.gender,foo.isdel) VALUES(@Id,@Name,@Age,@Gender,@Isdel)", DapperExtensions.LastSql);

            db.Foo.InsertPartial(entity);
            Assert.AreEqual("INSERT INTO foo(foo.id,foo.name,foo.age,foo.gender,foo.isdel) VALUES(@Id,@Name,@Age,@Gender,@Isdel)", DapperExtensions.LastSql);

            db.Foo.Insert(entity, foo => new { foo.Name, foo.Age });
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES(@Name,@Age)", DapperExtensions.LastSql);

            db.Foo.Insert(entity, foo => new List<IColumn>() { foo.Name, foo.Age });
            Assert.AreEqual("INSERT INTO foo(foo.name,foo.age) VALUES(@Name,@Age)", DapperExtensions.LastSql);

            entity = new Entity.Foo() { Id = 1 };

            db.Foo.Delete(entity);
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=@Id", DapperExtensions.LastSql);

            db.Foo.Delete(1);
            Assert.AreEqual("DELETE FROM foo WHERE foo.id=@Id", DapperExtensions.LastSql);

            entity = new Entity.Foo() { Id = 1, Name = "hero", Age = 18 };

            db.Foo.Update(entity);
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age,foo.gender=@Gender,foo.isdel=@Isdel WHERE foo.id=@Id", DapperExtensions.LastSql);

            db.Foo.UpdatePartial(entity);
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age,foo.gender=@Gender,foo.isdel=@Isdel WHERE foo.id=@Id", DapperExtensions.LastSql);

            db.Foo.UpdatePartial(entity, foo => foo.Age > 18);
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age,foo.gender=@Gender,foo.isdel=@Isdel WHERE foo.age>18", DapperExtensions.LastSql);

            db.Foo.Update(entity, foo => new { foo.Name, foo.Age });
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age WHERE foo.id=@Id", DapperExtensions.LastSql);

            db.Foo.Update(entity, foo => new List<IColumn> { foo.Name, foo.Age });
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age WHERE foo.id=@Id", DapperExtensions.LastSql);

            db.Foo.Update(entity, foo => new { foo.Name, foo.Age }, foo => foo.Age > 18);
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age WHERE foo.age>18", DapperExtensions.LastSql);

            db.Foo.Update(entity, foo => new { foo.Name, foo.Age }, foo => foo.Age.GtVarParam());
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age WHERE foo.age>@Age", DapperExtensions.LastSql);

            db.Foo.Update(entity, foo => new List<IColumn> { foo.Name, foo.Age }, foo => foo.Age > 18);
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age WHERE foo.age>18", DapperExtensions.LastSql);

            db.Foo.Update(entity, foo => new List<IColumn> { foo.Name, foo.Age }, foo => foo.Age.GtVarParam());
            Assert.AreEqual("UPDATE foo SET foo.name=@Name,foo.age=@Age WHERE foo.age>@Age", DapperExtensions.LastSql);

            db.Foo.Get(1);
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.id=@Id", DapperExtensions.LastSql);

            db.Foo.GetList(new List<object>() { 1 });
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.id IN (1)", DapperExtensions.LastSql);

            db.Foo.Query(foo => foo.Age > 18);
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.age>18", DapperExtensions.LastSql);

            db.Foo.Query(foo => foo.Id.EqVarParam(), new { Id = 1 });
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age,foo.gender,foo.isdel FROM foo WHERE foo.id=@Id", DapperExtensions.LastSql);

            db.Foo.QueryPartial(foo => new { foo.Id, foo.Name, foo.Age }, foo => foo.Age > 18);
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age FROM foo WHERE foo.age>18", DapperExtensions.LastSql);

            db.Foo.QueryPartial(foo => new { foo.Id, foo.Name, foo.Age }, foo => foo.Id.EqVarParam(), new { Id = 1 });
            Assert.AreEqual("SELECT foo.id,foo.name,foo.age FROM foo WHERE foo.id=@Id", DapperExtensions.LastSql);
        }
    }
}
