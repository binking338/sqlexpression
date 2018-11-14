using System;
namespace SqlExpression
{
    public abstract class TableSchema<Schema> : Expression, IAliasTableExpression
        where Schema : class
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
        public virtual SelectItemExpression[] AllMapped()
        {
            return _allItems;
        }
        public virtual SelectItemExpression[] PKMapped()
        {
            return _pkItems;
        }

        public virtual Schema As(string alias)
        {
            Schema t = Activator.CreateInstance(typeof(Schema), alias) as Schema;
            return t;
        }

        #region IAliasTableExpression

        protected override string Build()
        {
            if (string.IsNullOrEmpty((this as IAliasTableExpression).Alias))
            {
                return (this as IAliasTableExpression).Table.ToString();
            }
            else
            {
                return string.Format("{0} AS {2}{1}{3}", (this as IAliasTableExpression).Table.ToString(), (this as IAliasTableExpression).Alias, Option.OpenQuotationMark, Option.CloseQuotationMark);
            }
        }

        ITable IAliasTableExpression.Table { get; set; }

        string IAliasDataset.Alias { get; set; }

        #endregion
    }

    public class TableColumn<T> : Column
    {
        public TableColumn(string name, string dataset = null, T def = default(T)) : base(name, dataset)
        {
            Default = def;
        }

        public T Default { get; set; } = default(T);
    }
}
