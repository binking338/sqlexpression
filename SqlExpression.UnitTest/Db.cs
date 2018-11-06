using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SqlExpression;
using SqlExpression.Extension;
using SqlExpression.UnitTest.Schema;

namespace SqlExpression.UnitTest
{
    public static class Db
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
        protected Field[] _allFields = null;
        protected Field[] _primaryKey = null;
        protected SelectItemExpression[] _selectItems = null;
        public Field[] GetAllFields()
        {
            return _allFields;
        }
        public Field[] GetPrimaryKey()
        {
            return _primaryKey;
        }
        public SelectItemExpression[] GetAllFieldsMapping()
        {
            return _selectItems;
        }

        public T As(string alias)
        {
            T t = Activator.CreateInstance(typeof(T), alias) as T;
            return t;
        }

        #region IAliasTableExpression
        string IExpression.Exp
        {
            get
            {
                if (string.IsNullOrEmpty((this as IAliasTableExpression).Alias))
                {
                    return (this as IAliasTableExpression).Table.Exp;
                }
                else
                {
                    return string.Format("{0} AS {2}{1}{3}", (this as IAliasTableExpression).Table.Exp, (this as IAliasTableExpression).Alias, Expression.OpenQuotationMark, Expression.CloseQuotationMark);
                }
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

            Id = new Field("id", alias);
            Name = new Field("name", alias);
            Age = new Field("age", alias);
            Gender = new Field("gender", alias);
            Isdel = new Field("isdel", alias);

            _allFields = new Field[] { Id, Name, Age, Gender, Isdel };
            _primaryKey = new Field[] { Id };
            _selectItems = new SelectItemExpression[] { Id.As("Id"), Name.As("Name"), Age.As("Age"), Gender.As("Gender"), Isdel.As("Isdel") };
        }

        public Field Id { get; set; }

        public Field Name { get; set; }

        public Field Age { get; set; }

        public Field Gender { get; set; }

        public Field Isdel { get; set; }
    }

    public class Bar : _SchemaExpression<Bar>
    {
        public Bar(string alias = "bar")
        {
            (this as IAliasTableExpression).Alias = alias == "bar" ? null : alias;
            (this as IAliasTableExpression).Table = new Table("bar");

            Id = new Field("id", alias);
            Name = new Field("name", alias);
            Age = new Field("age", alias);
            Gender = new Field("gender", alias);
            Isdel = new Field("isdel", alias);

            _allFields = new Field[] { Id, Name, Age, Gender, Isdel };
            _primaryKey = new Field[] { Id };
            _selectItems = new SelectItemExpression[] { Id.As("Id"), Name.As("Name"), Age.As("Age"), Gender.As("Gender"), Isdel.As("Isdel") };
        }

        public Field Id { get; set; }

        public Field Name { get; set; }

        public Field Age { get; set; }

        public Field Gender { get; set; }

        public Field Isdel { get; set; }
    }
}
