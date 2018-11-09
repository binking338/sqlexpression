using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression.Extension
{
    public static class InsertStatementExtensions
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

        #endregion

        public static InsertStatement Insert(this ITable table)
        {
            return new InsertStatement(table, null, null);
        }

        public static InsertStatement Insert(this ITable table, IEnumerable<IColumn> columns)
        {
            return new InsertStatement(table, columns.ToList(), null as ICollection);
        }

        public static InsertStatement Insert(this ITable table, params IColumn[] columns)
        {
            return Insert(table, columns.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, IEnumerable<IColumn> columns)
        {
            var _params = columns.Select(column => column.ToParam());
            ValueCollectionExpression collection = new ValueCollectionExpression(_params.Cast<ISimpleValue>().ToList());
            return new InsertStatement(table, columns.ToList(), collection);
        }

        public static InsertStatement InsertVarParam(this ITable table, params IColumn[] columns)
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

        #endregion

        public static IInsertStatement Columns(this IInsertStatement insert, IEnumerable<IColumn> columns)
        {
            insert.Columns = columns.ToList();
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, params IColumn[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            insert.Columns = columns.ToList();
            return insert;
        }

        #endregion

        #region Values

        #region Shortcut

        public static IInsertStatement Vals(this IInsertStatement insert, IEnumerable<ISimpleValue> values)
        {
            return Values(insert, values);
        }
        public static IInsertStatement ValsL(this IInsertStatement insert, params object[] values)
        {
            return ValuesVarLiteral(insert, values);
        }
        public static IInsertStatement ValsC(this IInsertStatement insert, IEnumerable<string> customValues)
        {
            return ValuesVarCustom(insert, customValues);
        }
        public static IInsertStatement ValsC(this IInsertStatement insert, params string[] customValues)
        {
            return ValuesVarCustom(insert, customValues);
        }
        public static IInsertStatement ValsP(this IInsertStatement insert)
        {
            return ValuesVarParam(insert);
        }

        public static IInsertStatement ValuesL(this IInsertStatement insert, params object[] values)
        {
            return ValuesVarLiteral(insert, values);
        }

        public static IInsertStatement ValuesC(this IInsertStatement insert, IEnumerable<string> customValues)
        {
            return ValuesVarCustom(insert, customValues);
        }

        public static IInsertStatement ValuesC(this IInsertStatement insert, params string[] customValues)
        {
            return ValuesVarCustom(insert, customValues);
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
                values = values.Count() > insert.Columns.Count ?
                               values.Take(insert.Columns.Count) : values.Concat(new ISimpleValue[insert.Columns.Count - values.Count()]);
                IValueCollectionExpression collection = new ValueCollectionExpression(values.ToList());

                insert.Values = collection;
            }


            return insert;
        }

        public static IInsertStatement ValuesVarLiteral(this IInsertStatement insert, params object[] values)
        {
            var setableValues = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
            return Values(insert, setableValues);
        }

        public static IInsertStatement ValuesVarCustom(this IInsertStatement insert, IEnumerable<string> customValues)
        {
            return Values(insert, customValues.Select(v => new CustomExpression(v)));
        }

        public static IInsertStatement ValuesVarCustom(this IInsertStatement insert, params string[] customValues)
        {
            return Values(insert, customValues.Select(v => new CustomExpression(v)));
        }

        public static IInsertStatement ValuesVarParam(this IInsertStatement insert)
        {
            if (insert.Columns == null) return insert;
            var vals = insert.Columns.Select(column => column.ToParam() as ISimpleValue);

            return Values(insert, vals);
        }
        public static IInsertStatement ValuesFillNull(this IInsertStatement insert)
        {
            if (insert.Columns == null) return insert;
            var vals = insert.Columns.Select(column => new LiteralValue(null) as ISimpleValue);

            return Values(insert, vals);
        }

        #endregion

        #region Set

        #region Shortcut
        public static IInsertStatement SetL(this IInsertStatement insert, IColumn column, object value)
        {

            return SetVarLiteral(insert, column, value);
        }

        public static IInsertStatement SetP(this IInsertStatement insert, IColumn column, string param = null)
        {
            return SetVarParam(insert, column, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, IColumn column, string customValue)
        {
            return SetVarCustom(insert, column, customValue);
        }

        #endregion

        public static IInsertStatement Set(this IInsertStatement insert, IColumn column, ISimpleValue value)
        {
            var cols = insert.Columns ?? new List<IColumn>();
            var valCollection = insert.Values as IValueCollectionExpression;
            var vals = valCollection.Values ?? new List<ISimpleValue>();
            cols.Add(column);
            vals.Add(value);
            insert.Columns = cols.ToList();
            valCollection.Values = vals.ToList();
            return insert;
        }
        public static IInsertStatement SetVarLiteral(this IInsertStatement insert, IColumn column, object value)
        {
            return Set(insert, column, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, IColumn column, string param = null)
        {
            return Set(insert, column, column.ToParam(param));
        }
        public static IInsertStatement SetVarCustom(this IInsertStatement insert, IColumn column, string customValue)
        {
            return Set(insert, column, new CustomExpression(customValue));
        }

        #endregion
    }

}
