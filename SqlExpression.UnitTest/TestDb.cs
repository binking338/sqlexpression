using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SqlExpression;
using SqlExpression.Extension;
using SqlExpression.UnitTest.Schema;

namespace SqlExpression.UnitTest
{
    public static class TestDb
    {
        public static Foo Foo { get; set; } = new Foo();
        public static Bar Bar { get; set; } = new Bar();
    }
}

namespace SqlExpression.UnitTest.Schema
{
    public abstract class _SchemaExpression<T> : IAliasTableExpression
        where T : class
    {
        protected Column[] _allColumns = null;
        protected Column[] _pkColumns = null;
        protected SelectItemExpression[] _allItems = null;
        protected SelectItemExpression[] _pkItems = null;
        public Column[] All()
        {
            return _allColumns;
        }
        public Column[] PK()
        {
            return _pkColumns;
        }
        public SelectItemExpression[] AllMapped()
        {
            return _allItems;
        }
        public SelectItemExpression[] PKMapped()
        {
            return _pkItems;
        }

        public T As(string alias)
        {
            T t = Activator.CreateInstance(typeof(T), alias) as T;
            return t;
        }

        #region IAliasTableExpression
        public override string ToString()
        {
            if (string.IsNullOrEmpty((this as IAliasTableExpression).Alias))
            {
                return (this as IAliasTableExpression).Table.ToString();
            }
            else
            {
                return string.Format("{0} AS {2}{1}{3}", (this as IAliasTableExpression).Table.ToString(), (this as IAliasTableExpression).Alias, Expression.OpenQuotationMark, Expression.CloseQuotationMark);
            }
        }

        ITable IAliasTableExpression.Table { get; set; }

        string IAliasDataset.Alias { get; set; }
        #endregion
    }

    public class Foo : _SchemaExpression<Foo>
    {
        public Foo(string alias = "foo")
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

        public Column Id { get; set; }

        public Column Name { get; set; }

        public Column Age { get; set; }

        public Column Gender { get; set; }

        public Column Isdel { get; set; }
    }

    public class Bar : _SchemaExpression<Bar>
    {
        public Bar(string alias = "bar")
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

        public Column Id { get; set; }

        public Column Name { get; set; }

        public Column Age { get; set; }

        public Column Gender { get; set; }

        public Column Isdel { get; set; }
    }
}
