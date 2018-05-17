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

        public static InsertStatement InsertP(this ITable table, IEnumerable<IField> fields)
        {
            return InsertVarParam(table, fields);
        }

        public static InsertStatement InsertP(this ITable table, params IField[] fields)
        {
            return InsertVarParam(table, fields);
        }

        public static InsertStatement InsertP(this ITable table, params Field[] fields)
        {
            return InsertVarParam(table, fields);
        }

        #endregion

        public static InsertStatement Insert(this ITable table)
        {
            return new InsertStatement(table);
        }

        public static InsertStatement Insert(this ITable table, IEnumerable<IField> fields)
        {
            return new InsertStatement(table, fields.ToArray(), null as ICollection);
        }

        public static InsertStatement Insert(this ITable table, params IField[] fields)
        {
            return Insert(table, fields.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, IEnumerable<IField> fields)
        {
            var _params = fields.Select(field => field.ToParam());
            ValueCollectionExpression collection = new ValueCollectionExpression(_params.ToArray());
            return new InsertStatement(table, fields.ToArray(), collection );
        }

        public static InsertStatement InsertVarParam(this ITable table, params IField[] fields)
        {
            return InsertVarParam(table, fields.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITable table, params Field[] fields)
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

        public static IInsertStatement Into(this IInsertStatement insert, Table table)
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
        public static IInsertStatement Flds(this IInsertStatement insert, params Field[] fields)
        {
            return Fields(insert, fields);
        }

        #endregion

        public static IInsertStatement Fields(this IInsertStatement insert, IEnumerable<IField> fields)
        {
            insert.Fields = fields.ToArray();
            return insert;
        }

        public static IInsertStatement Fields(this IInsertStatement insert, params IField[] fields)
        {
            insert.Fields = fields.ToArray();
            return insert;
        }

        public static IInsertStatement Fields(this IInsertStatement insert, params Field[] fields)
        {
            insert.Fields = fields.ToArray();
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
            if (insert.Fields == null)
            {
                return insert;
            }
            else
            {
                values = values.Count() > insert.Fields.Length ? 
                               values.Take(insert.Fields.Length) : values.Concat(new ISimpleValue[insert.Fields.Length - values.Count()]);
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

        public static IInsertStatement SetP(this IInsertStatement insert, IField field, string param = null)
        {
            return SetVarParam(insert, field, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, IField field, string customer)
        {
            return SetVarCustomer(insert, field, customer);
        }

        public static IInsertStatement SetP(this IInsertStatement insert, Field field, string param = null)
        {
            return SetVarParam(insert, field, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, Field field, string customer)
        {
            return SetVarCustomer(insert, field, customer);
        }

        #endregion

        public static IInsertStatement Set(this IInsertStatement insert, IField field, ISimpleValue value)
        {
            if(!(insert.Values is IValueCollectionExpression))
            {
                throw new SqlSyntaxException(insert, Error.SetValueError);
            }
            var cols = insert.Fields?.ToList() ?? new List<IField>();
            var valCollections = insert.Values as IValueCollectionExpression;
            var vals = valCollections.Values?.ToList() ?? new List<ISimpleValue>();
            cols.Add(field);
            vals.Add(value);
            insert.Fields = cols.ToArray();
            valCollections.Values = vals.ToArray();
            return insert;
        }
        public static IInsertStatement Set(this IInsertStatement insert, IField field, object value)
        {
            return Set(insert, field, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, IField field, string param = null)
        {
            return Set(insert, field, field.ToParam(param));
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, IField field, string customer)
        {
            return Set(insert, field, new CustomerExpression(customer));
        }

        public static IInsertStatement Set(this IInsertStatement insert, Field field, ISimpleValue value)
        {
            return Set(insert, field as IField, value);
        }
        public static IInsertStatement Set(this IInsertStatement insert, Field field, object value)
        {
            return Set(insert, field as IField, value);
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, Field field, string param = null)
        {
            return SetVarParam(insert, field as IField, param);
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, Field field, string customer)
        {
            return SetVarCustomer(insert, field as IField, customer);
        }

        #endregion
    }
}
