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
            return new InsertStatement(table, columns.ToArray(), new IValue[columns.Count()]);
        }

        public static InsertStatement Insert(this ITable table, params IColumn[] columns)
        {
            return Insert(table, columns.AsEnumerable());
        }

        public static InsertStatement Insert(this ITable table, params Column[] columns)
        {
            return Insert(table, columns.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, IEnumerable<IColumn> columns)
        {
            var _params = columns.Select(col => col.ToParam());
            return new InsertStatement(table, columns.ToArray(), _params.ToArray());
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
        public static IInsertStatement ValsNull(this IInsertStatement insert)
        {
            return ValuesFillNull(insert);
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

        public static IInsertStatement ValuesNull(this IInsertStatement insert)
        {
            return ValuesFillNull(insert);
        }

        #endregion

        public static IInsertStatement Values(this IInsertStatement insert, IEnumerable<IValue> values)
        {
            if (insert.Columns == null)
            {
                return insert;
            }
            else if (insert.Values == null)
            {
                insert.Values = values.Count() > insert.Columns.Length
                    ? values.Take(insert.Columns.Length).ToArray()
                    : values.Concat(new IValue[insert.Columns.Length - values.Count()]).ToArray();
            }
            else
            {
                var vals = insert.Values.ToList();
                var cols = insert.Columns.ToList();
                if (vals.Count > cols.Count)
                {
                    vals.RemoveRange(cols.Count, vals.Count - cols.Count);
                }
                else if (vals.Count < cols.Count)
                {
                    vals.AddRange(new IValue[cols.Count - vals.Count]);
                }
                for (int i = 0, j = 0; i < cols.Count && j < values.Count(); i++)
                {
                    // 避免覆盖
                    if (vals[i] == null) vals[i] = values.ElementAt(j++);
                }
                insert.Values = vals.ToArray();
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
            var vals = insert.Columns.Select(col => new Param(col.Name) as IValue).ToArray();
            if (insert.Values != null)
            {
                for (var i = 0; i < vals.Length && i < insert.Values.Length; i++)
                {
                    if (insert.Values[i] != null) vals[i] = insert.Values[i];
                }
            }
            insert.Values = vals.ToArray();
            return insert;
        }

        public static IInsertStatement ValuesFillNull(this IInsertStatement insert)
        {
            if (insert.Columns == null) return insert;
            var vals = insert.Columns.Select(col => new LiteralValue(null) as IValue).ToArray();
            if (insert.Values != null)
            {
                for (var i = 0; i < vals.Length && i < insert.Values.Length; i++)
                {
                    if (insert.Values[i] != null) vals[i] = insert.Values[i];
                }
            }
            insert.Values = vals.ToArray();
            return insert;
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

        public static IInsertStatement Set(this IInsertStatement insert, IColumn column, IValue value)
        {
            var cols = insert.Columns.ToList();
            var vals = insert.Values.ToList();
            cols.Add(column);
            vals.Add(value);
            insert.Columns = cols.ToArray();
            insert.Values = vals.ToArray();
            return insert;
        }
        public static IInsertStatement Set(this IInsertStatement insert, IColumn column, object value)
        {
            return Set(insert, column, value is IValue ? value as IValue : new LiteralValue(value));
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, IColumn column, string param = null)
        {
            return Set(insert, column, column.ToParam(param));
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, IColumn column, string customer)
        {
            return Set(insert, column, new CustomerExpression(customer));
        }

        public static IInsertStatement Set(this IInsertStatement insert, Column column, IValue value)
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
