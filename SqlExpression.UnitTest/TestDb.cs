using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SqlExpression;
using SqlExpression.Extension;
using SqlExpression.UnitTest.Schema;
using SqlExpression.UnitTest.Entity;
using System.Data;

namespace SqlExpression.UnitTest
{
    public class TestDb : DbContext
    {
        public TestDb() : this(null) { }

        public TestDb(IDbConnection connection)
        {
            Connection = connection;
            foo = new Lazy<Respository<FooSchema, Foo>>(() => new Respository<FooSchema, Foo>(Connection));
            bar = new Lazy<Respository<BarSchema, Bar>>(() => new Respository<BarSchema, Bar>(Connection));

            Expression.DefaultOption.Column2ParamContractHandler = ToUpperCamalCase;
            // SqlExpression.Extension.Dialect.Mysql.Config.EnableDialect();
        }

        private Lazy<Respository<FooSchema, Foo>> foo;
        private Lazy<Respository<BarSchema, Bar>> bar;

        public Respository<FooSchema, Foo> Foo { get { return foo.Value; } }
        public Respository<BarSchema, Bar> Bar { get { return bar.Value; } }
    }
}

namespace SqlExpression.UnitTest.Entity
{
    public class Foo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Gender { get; set; }

        public bool Isdel { get; set; }
    }

    public class Bar
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Gender { get; set; }

        public bool Isdel { get; set; }
    }
}

namespace SqlExpression.UnitTest.Schema
{
    public class FooSchema : SchemaExpression<FooSchema>
    {
        public FooSchema() : this("foo") { }

        public FooSchema(string alias)
        {
            (this as IAliasTableExpression).Alias = alias == "foo" ? null : alias;
            (this as IAliasTableExpression).Table = new Table("foo");

            Id = new Column("id", alias);
            Name = new Column("name", alias);
            Age = new Column("age", alias);
            Gender = new Column("gender", alias);
            Isdel = new Column("isdel", alias);

            _allColumns = new Column[] { Id, Name, Age, Gender, Isdel };
            _pkColumns = new Column[] { Id };
            _allItems = new SelectItemExpression[] { Id.As("Id"), Name.As("Name"), Age.As("Age"), Gender.As("Gender"), Isdel.As("Isdel") };
            _pkItems = new SelectItemExpression[] { Id.As("Id") };
        }

        public Column Id { get; }

        public Column Name { get; }

        public Column Age { get; }

        public Column Gender { get; }

        public Column Isdel { get; }
    }

    public class BarSchema : SchemaExpression<BarSchema>
    {
        public BarSchema() : this("bar") { }

        public BarSchema(string alias)
        {
            (this as IAliasTableExpression).Alias = alias == "bar" ? null : alias;
            (this as IAliasTableExpression).Table = new Table("bar");

            Id = new Column("id", alias);
            Name = new Column("name", alias);
            Age = new Column("age", alias);
            Gender = new Column("gender", alias);
            Isdel = new Column("isdel", alias);

            _allColumns = new Column[] { Id, Name, Age, Gender, Isdel };
            _pkColumns = new Column[] { Id };
            _allItems = new SelectItemExpression[] { Id.As("Id"), Name.As("Name"), Age.As("Age"), Gender.As("Gender"), Isdel.As("Isdel") };
            _pkItems = new SelectItemExpression[] { Id.As("Id") };
        }

        public Column Id { get; }

        public Column Name { get; }

        public Column Age { get; }

        public Column Gender { get; }

        public Column Isdel { get; }
    }
}