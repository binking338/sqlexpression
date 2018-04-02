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
            return new InsertStatement(table, columns.ToArray(), null as ICollection);
        }

        public static InsertStatement Insert(this ITable table, params IColumn[] columns)
        {
            return Insert(table, columns.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, IEnumerable<IColumn> columns)
        {
            var _params = columns.Select(col => col.ToParam());
            ValueCollectionExpression collection = new ValueCollectionExpression(_params.ToArray());
            return new InsertStatement(table, columns.ToArray(), collection );
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

        public static IInsertStatement Vals(this IInsertStatement insert, IEnumerable<ISimpleValue> values)
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

        public static IInsertStatement Values(this IInsertStatement insert, IEnumerable<ISimpleValue> values)
        {
            if (insert.Columns == null)
            {
                return insert;
            }
            else
            {
                values = values.Count() > insert.Columns.Length ? 
                               values.Take(insert.Columns.Length) : values.Concat(new ISimpleValue[insert.Columns.Length - values.Count()]);
                IValueCollectionExpression collection = new ValueCollectionExpression(values.ToArray());

                insert.Values = collection;
            }


            return insert;
        }

        public static IInsertStatement Values(this IInsertStatement insert, params object[] values)
        {
            var setableValues = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
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
            var vals = insert.Columns.Select(col => new Param(col.Name) as ISimpleValue);

            return Values(insert, vals);
        }
        public static IInsertStatement ValuesFillNull(this IInsertStatement insert)
        {
            if (insert.Columns == null) return insert;
            var vals = insert.Columns.Select(col => new LiteralValue(null) as ISimpleValue);

            return Values(insert, vals);
        }

        #endregion


        #region Set

        #region Shortcut

        public static IInsertStatement SetP(this IInsertStatement insert, IColumn column, string param = null)
        {
            return SetVarParam(insert, column, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, IColumn column, string customer)
        {
            return SetVarCustomer(insert, column, customer);
        }

        public static IInsertStatement SetP(this IInsertStatement insert, Column column, string param = null)
        {
            return SetVarParam(insert, column, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, Column column, string customer)
        {
            return SetVarCustomer(insert, column, customer);
        }

        #endregion

        public static IInsertStatement Set(this IInsertStatement insert, IColumn column, ISimpleValue value)
        {
            if(!(insert.Values is IValueCollectionExpression))
            {
                throw new SqlSyntaxException(insert, Error.SetValueError);
            }
            var cols = insert.Columns?.ToList() ?? new List<IColumn>();
            var valCollections = insert.Values as IValueCollectionExpression;
            var vals = valCollections.Values?.ToList() ?? new List<ISimpleValue>();
            cols.Add(column);
            vals.Add(value);
            insert.Columns = cols.ToArray();
            valCollections.Values = vals.ToArray();
            return insert;
        }
        public static IInsertStatement Set(this IInsertStatement insert, IColumn column, object value)
        {
            return Set(insert, column, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, IColumn column, string param = null)
        {
            return Set(insert, column, column.ToParam(param));
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, IColumn column, string customer)
        {
            return Set(insert, column, new CustomerExpression(customer));
        }

        public static IInsertStatement Set(this IInsertStatement insert, Column column, ISimpleValue value)
        {
            return Set(insert, column as IColumn, value);
        }
        public static IInsertStatement Set(this IInsertStatement insert, Column column, object value)
        {
            return Set(insert, column as IColumn, value);
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, Column column, string param = null)
        {
            return SetVarParam(insert, column as IColumn, param);
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, Column column, string customer)
        {
            return SetVarCustomer(insert, column as IColumn, customer);
        }

        #endregion
    }
}
