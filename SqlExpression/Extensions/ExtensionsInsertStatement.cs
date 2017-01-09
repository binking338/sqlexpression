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

        public static InsertStatement InsertP(this ITableExpression table, IEnumerable<IColumnExpression> columns)
        {
            return InsertVarParam(table, columns);
        }

        public static InsertStatement InsertP(this ITableExpression table, params IColumnExpression[] columns)
        {
            return InsertVarParam(table, columns);
        }

        public static InsertStatement InsertP(this ITableExpression table, params ColumnExpression[] columns)
        {
            return InsertVarParam(table, columns);
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

        public static IInsertStatement SetP(this IInsertStatement insert, IColumnExpression column, string param = null)
        {
            return SetVarParam(insert, column, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, IColumnExpression column, string customer)
        {
            return SetVarCustomer(insert, column, customer);
        }

        public static IInsertStatement SetP(this IInsertStatement insert, ColumnExpression column, string param = null)
        {
            return SetVarParam(insert, column, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, ColumnExpression column, string customer)
        {
            return SetVarCustomer(insert, column, customer);
        }

        #endregion

        public static InsertStatement Insert(this ITableExpression table)
        {
            return new InsertStatement(table);
        }

        public static InsertStatement Insert(this ITableExpression table, IEnumerable<IColumnExpression> columns)
        {
            return new InsertStatement(table, columns.ToArray(), new IValueExpression[columns.Count()]);
        }

        public static InsertStatement Insert(this ITableExpression table, params IColumnExpression[] columns)
        {
            return Insert(table, columns.AsEnumerable());
        }

        public static InsertStatement Insert(this ITableExpression table, params ColumnExpression[] columns)
        {
            return Insert(table, columns.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITableExpression table, IEnumerable<IColumnExpression> columns)
        {
            var _params = columns.Select(col => col.ToParam());
            return new InsertStatement(table, columns.ToArray(), _params.ToArray());
        }

        public static InsertStatement InsertVarParam(this ITableExpression table, params IColumnExpression[] columns)
        {
            return InsertVarParam(table, columns.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITableExpression table, params ColumnExpression[] columns)
        {
            return InsertVarParam(table, columns.AsEnumerable());
        }

        public static IInsertStatement Into(this IInsertStatement insert, ITableExpression table)
        {
            insert.Table = table;
            return insert;
        }

        public static IInsertStatement Into(this IInsertStatement insert, TableExpression table)
        {
            insert.Table = table;
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, IEnumerable<IColumnExpression> columns)
        {
            insert.Columns = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, params IColumnExpression[] columns)
        {
            insert.Columns = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, params ColumnExpression[] columns)
        {
            insert.Columns = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Values(this IInsertStatement insert, IEnumerable<IValueExpression> values)
        {
            if (insert.Columns == null)
            {
                return insert;
            }
            else if (insert.Values == null)
            {
                insert.Values = values.Count() > insert.Columns.Length
                    ? values.Take(insert.Columns.Length).ToArray()
                    : values.Concat(new IValueExpression[insert.Columns.Length - values.Count()]).ToArray();
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
                    vals.AddRange(new IValueExpression[cols.Count - vals.Count]);
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
            var setableValues = values.Select(val => val is IValueExpression ? val as IValueExpression : new LiteralValueExpression(val));
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
            var vals = insert.Columns.Select(col => new ParamExpression(col.Name) as IValueExpression).ToArray();
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
            var vals = insert.Columns.Select(col => new LiteralValueExpression(null) as IValueExpression).ToArray();
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

        public static IInsertStatement Set(this IInsertStatement insert, IColumnExpression column, IValueExpression value)
        {
            var cols = insert.Columns.ToList();
            var vals = insert.Values.ToList();
            cols.Add(column);
            vals.Add(value);
            insert.Columns = cols.ToArray();
            insert.Values = vals.ToArray();
            return insert;
        }
        public static IInsertStatement Set(this IInsertStatement insert, IColumnExpression column, object value)
        {
            return Set(insert, column, value is IValueExpression ? value as IValueExpression : new LiteralValueExpression(value));
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, IColumnExpression column, string param = null)
        {
            return Set(insert, column, column.ToParam(param));
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, IColumnExpression column, string customer)
        {
            return Set(insert, column, new CustomerExpression(customer));
        }

        public static IInsertStatement Set(this IInsertStatement insert, ColumnExpression column, IValueExpression value)
        {
            return Set(insert, column as IColumnExpression, value);
        }
        public static IInsertStatement Set(this IInsertStatement insert, ColumnExpression column, object value)
        {
            return Set(insert, column as IColumnExpression, value);
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, ColumnExpression column, string param = null)
        {
            return SetVarParam(insert, column as IColumnExpression, param);
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, ColumnExpression column, string customer)
        {
            return SetVarCustomer(insert, column as IColumnExpression, customer);
        }

        #endregion
    }
}
