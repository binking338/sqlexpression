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

        public static InsertStatement InsertP(this ITable table, IEnumerable<IField> fields)
        {
            return InsertVarParam(table, fields);
        }

        public static InsertStatement InsertP(this ITable table, params IField[] fields)
        {
            return InsertVarParam(table, fields);
        }

        #endregion

        public static InsertStatement Insert(this ITable table)
        {
            return new InsertStatement(table, null, null);
        }

        public static InsertStatement Insert(this ITable table, IEnumerable<IField> fields)
        {
            return new InsertStatement(table, fields.ToList(), null as ICollection);
        }

        public static InsertStatement Insert(this ITable table, params IField[] fields)
        {
            return Insert(table, fields.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, IEnumerable<IField> fields)
        {
            var _params = fields.Select(field => field.ToParam());
            ValueCollectionExpression collection = new ValueCollectionExpression(_params.Cast<ISimpleValue>().ToList());
            return new InsertStatement(table, fields.ToList(), collection);
        }

        public static InsertStatement InsertVarParam(this ITable table, params IField[] fields)
        {
            return InsertVarParam(table, fields.AsEnumerable());
        }

        #endregion

        #region Into

        public static IInsertStatement Into(this IInsertStatement insert, ITable table)
        {
            insert.Table = table;
            return insert;
        }

        #endregion

        #region Fields

        #region ShortCut

        public static IInsertStatement Flds(this IInsertStatement insert, IEnumerable<IField> fields)
        {
            return Fields(insert, fields);
        }
        public static IInsertStatement Flds(this IInsertStatement insert, params IField[] fields)
        {
            return Fields(insert, fields);
        }

        #endregion

        public static IInsertStatement Fields(this IInsertStatement insert, IEnumerable<IField> fields)
        {
            insert.Fields = fields.ToList();
            return insert;
        }

        public static IInsertStatement Fields(this IInsertStatement insert, params IField[] fields)
        {
            insert.Fields = fields.ToList();
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
            if (insert.Fields == null)
            {
                return insert;
            }
            else
            {
                values = values.Count() > insert.Fields.Count ?
                               values.Take(insert.Fields.Count) : values.Concat(new ISimpleValue[insert.Fields.Count - values.Count()]);
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
            if (insert.Fields == null) return insert;
            var vals = insert.Fields.Select(field => new Param(field.Name) as ISimpleValue);

            return Values(insert, vals);
        }
        public static IInsertStatement ValuesFillNull(this IInsertStatement insert)
        {
            if (insert.Fields == null) return insert;
            var vals = insert.Fields.Select(field => new LiteralValue(null) as ISimpleValue);

            return Values(insert, vals);
        }

        #endregion

        #region Set

        #region Shortcut
        public static IInsertStatement SetL(this IInsertStatement insert, IField field, object value)
        {

            return SetVarLiteral(insert, field, value);
        }

        public static IInsertStatement SetP(this IInsertStatement insert, IField field, string param = null)
        {
            return SetVarParam(insert, field, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, IField field, string customValue)
        {
            return SetVarCustom(insert, field, customValue);
        }

        #endregion

        public static IInsertStatement Set(this IInsertStatement insert, IField field, ISimpleValue value)
        {
            var cols = insert.Fields ?? new List<IField>();
            var valCollection = insert.Values as IValueCollectionExpression;
            var vals = valCollection.Values ?? new List<ISimpleValue>();
            cols.Add(field);
            vals.Add(value);
            insert.Fields = cols.ToList();
            valCollection.Values = vals.ToList();
            return insert;
        }
        public static IInsertStatement SetVarLiteral(this IInsertStatement insert, IField field, object value)
        {
            return Set(insert, field, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, IField field, string param = null)
        {
            return Set(insert, field, field.ToParam(param));
        }
        public static IInsertStatement SetVarCustom(this IInsertStatement insert, IField field, string customValue)
        {
            return Set(insert, field, new CustomExpression(customValue));
        }

        #endregion
    }

}
