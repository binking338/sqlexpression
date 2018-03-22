using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsInsertStatement
    {
        #region Insert

        #region ShortCut

        public static InsertStatement InsertP(this ITable table, IEnumerable<IColumn> columns)
        {
            return InsertVarParam(table, columns);
        }

        public static InsertStatement InsertP(this ITable table, params IColumn[] columns)
        {
            return InsertVarParam(table, columns);
        }

        public static InsertStatement InsertP(this ITable table, params Column[] columns)
        {
            return InsertVarParam(table, columns);
        }

        #endregion

        public static InsertStatement Insert(this ITable table)
        {
            return new InsertStatement(table);
        }

        public static InsertStatement Insert(this ITable table, IEnumerable<IColumn> columns)
        {
            return new InsertStatement(table, columns.ToArray(), null);
        }

        public static InsertStatement Insert(this ITable table, params IColumn[] columns)
        {
            return Insert(table, columns.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, IEnumerable<IColumn> columns)
        {
            var _params = columns.Select(col => col.ToParam());
            ValueCollectionExpression collection = new ValueCollectionExpression(_params.ToArray());
            return new InsertStatement(table, columns.ToArray(), new ICollection[] { collection });
        }

        public static InsertStatement InsertVarParam(this ITable table, params IColumn[] columns)
        {
            return InsertVarParam(table, columns.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, params Column[] columns)
        {
            return InsertVarParam(table, columns.AsEnumerable());
        }

        #endregion

        #region Into

        public static IInsertStatement Into(this IInsertStatement insert, ITable table)
        {
            insert.Table = table;
            return insert;
        }

        public static IInsertStatement Into(this IInsertStatement insert, Table table)
        {
            insert.Table = table;
            return insert;
        }

        #endregion

        #region Columns

        #region ShortCut

        public static IInsertStatement Cols(this IInsertStatement insert, IEnumerable<IColumn> columns)
        {
            return Columns(insert, columns);
        }
        public static IInsertStatement Cols(this IInsertStatement insert, params IColumn[] columns)
        {
            return Columns(insert, columns);
        }
        public static IInsertStatement Cols(this IInsertStatement insert, params Column[] columns)
        {
            return Columns(insert, columns);
        }

        #endregion

        public static IInsertStatement Columns(this IInsertStatement insert, IEnumerable<IColumn> columns)
        {
            insert.Columns = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, params IColumn[] columns)
        {
            insert.Columns = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, params Column[] columns)
        {
            insert.Columns = columns.ToArray();
            return insert;
        }

        #endregion

        #region Values

        #region Shortcut

        public static IInsertStatement Vals(this IInsertStatement insert, IEnumerable<IValue> values)
        {
            return Values(insert, values);
        }
        public static IInsertStatement Vals(this IInsertStatement insert, params object[] values)
        {
            return Values(insert, values);
        }
        public static IInsertStatement ValsC(this IInsertStatement insert, IEnumerable<string> values)
        {
            return ValuesVarCustomer(insert, values);
        }
        public static IInsertStatement ValsC(this IInsertStatement insert, params string[] values)
        {
            return ValuesVarCustomer(insert, values);
        }
        public static IInsertStatement ValsP(this IInsertStatement insert)
        {
            return ValuesVarParam(insert);
        }

        public static IInsertStatement ValuesC(this IInsertStatement insert, IEnumerable<string> values)
        {
            return ValuesVarCustomer(insert, values);
        }

        public static IInsertStatement ValuesC(this IInsertStatement insert, params string[] values)
        {
            return ValuesVarCustomer(insert, values);
        }

        public static IInsertStatement ValuesP(this IInsertStatement insert)
        {
            return ValuesVarParam(insert);
        }

        #endregion

        public static IInsertStatement Values(this IInsertStatement insert, IEnumerable<IValue> values)
        {
            if (insert.Columns == null)
            {
                return insert;
            }
            else
            {
                values = values.Count() > insert.Columns.Length ? 
                               values.Take(insert.Columns.Length) : values.Concat(new IValue[insert.Columns.Length - values.Count()]);
                IValueCollectionExpression collection = new ValueCollectionExpression(values.ToArray());
                List<ICollection> collections = new List<ICollection>();
                if (insert.Values != null)
                {
                    collections.AddRange(insert.Values); 
                }
                insert.Values = collections.ToArray();
            }


            return insert;
        }

        public static IInsertStatement Values(this IInsertStatement insert, params object[] values)
        {
            var setableValues = values.Select(val => val is IValue ? val as IValue : new LiteralValue(val));
            return Values(insert, setableValues);
        }

        public static IInsertStatement ValuesVarCustomer(this IInsertStatement insert, IEnumerable<string> values)
        {
            return Values(insert, values.Select(v=>new CustomerExpression(v)));
        }

        public static IInsertStatement ValuesVarCustomer(this IInsertStatement insert, params string[] values)
        {
            return Values(insert, values.Select(v => new CustomerExpression(v)));
        }

        public static IInsertStatement ValuesVarParam(this IInsertStatement insert)
        {
            if (insert.Columns == null) return insert;
            var vals = insert.Columns.Select(col => new Param(col.Name) as IValue);

            return Values(insert, vals);
        }

        #endregion
    }
}
