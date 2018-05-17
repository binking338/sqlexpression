using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsUpdateExpression
    {
        #region Update

        #region ShortCut

        public static UpdateStatement UpdateP(this ITable table, IEnumerable<IField> fields)
        {
            return UpdateVarParam(table, fields);
        }

        public static UpdateStatement UpdateP(this ITable table, params IField[] fields)
        {
            return UpdateVarParam(table, fields);
        }

        public static UpdateStatement UpdateP(this ITable table, params Field[] fields)
        {
            return UpdateVarParam(table, fields);
        }

        #endregion

        public static UpdateStatement Update(this ITable table)
        {
            return new UpdateStatement(new TableAliasExpression(table, null));
        }

        public static UpdateStatement Update(this ITable table, IEnumerable<IField> fields)
        {
            var set = new SetClause(fields.Select(c => new SetFieldExpression(c, null) as ISetFieldExpression).ToArray());
            return new UpdateStatement(new TableAliasExpression(table, null), set, null);
        }

        public static UpdateStatement Update(this ITable table, params IField[] fields)
        {
            return Update(table, fields.AsEnumerable());
        }

        public static UpdateStatement Update(this ITable table, params Field[] fields)
        {
            return Update(table, fields.AsEnumerable());
        }

        public static UpdateStatement UpdateVarParam(this ITable table, IEnumerable<IField> fields)
        {
            var set = new SetClause(fields.Select(field => field.SetVarParam() as ISetFieldExpression).ToArray());
            return new UpdateStatement(new TableAliasExpression(table, null), set, null);
        }

        public static UpdateStatement UpdateVarParam(this ITable table, params IField[] fields)
        {
            return UpdateVarParam(table, fields.AsEnumerable());
        }

        public static UpdateStatement UpdateVarParam(this ITable table, params Field[] fields)
        {
            return UpdateVarParam(table, fields.AsEnumerable());
        }

        #endregion

        #region Values

        #region Shortcut

        public static IUpdateStatement Vals(this IUpdateStatement update, params object[] values)
        {
            return Values(update, values);
        }
        public static IUpdateStatement ValsP(this IUpdateStatement update)
        {
            return ValuesVarParam(update);
        }
        public static IUpdateStatement ValsC(this IUpdateStatement update, params string[] customers)
        {
            return ValuesVarCustomer(update, customers);
        }

        public static IUpdateStatement ValuesP(this IUpdateStatement update)
        {
            return ValuesVarParam(update);
        }

        public static IUpdateStatement ValuesC(this IUpdateStatement update, params string[] customers)
        {
            return ValuesVarCustomer(update, customers);
        }

        #endregion

        public static IUpdateStatement Values(this IUpdateStatement update, params object[] values)
        {
            update.Set = update.Set.Values(values);
            return update;
        }

        public static IUpdateStatement ValuesVarParam(this IUpdateStatement update)
        {
            update.Set = update.Set.ValuesVarParam();
            return update;
        }

        public static IUpdateStatement ValuesVarCustomer(this IUpdateStatement update, params string[] customers)
        {
            update.Set = update.Set.ValuesVarCustomer(customers);
            return update;
        }

        #endregion

        #region Set

        #region Shortcut

        public static IUpdateStatement SetP(this IUpdateStatement update, IField field, string param = null)
        {
            return SetVarParam(update, field, param);
        }

        public static IUpdateStatement SetC(this IUpdateStatement update, IField field, string customer)
        {
            return SetVarCustomer(update, field, customer);
        }

        public static IUpdateStatement SetP(this IUpdateStatement update, Field field, string param = null)
        {
            return SetVarParam(update, field, param);
        }

        public static IUpdateStatement SetC(this IUpdateStatement update, Field field, string customer)
        {
            return SetVarCustomer(update, field, customer);
        }

        #endregion

        public static IUpdateStatement Set(this IUpdateStatement update, ISetFieldExpression setItem)
        {
            update.Set = update.Set.SetItem(setItem);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, IField field, ISimpleValue value)
        {
            update.Set = update.Set.SetItem(field, value);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, IField field, object value)
        {
            update.Set = update.Set.SetItem(field, value);
            return update;
        }

        public static IUpdateStatement SetVarParam(this IUpdateStatement update, IField field, string param = null)
        {
            update.Set = update.Set.SetItemVarParam(field, param);
            return update;
        }

        public static IUpdateStatement SetVarCustomer(this IUpdateStatement update, IField field, string customer)
        {
            update.Set = update.Set.SetItemVarCustomer(field, customer);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, Field field, ISimpleValue value)
        {
            update.Set = update.Set.SetItem(field, value);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, Field field, object value)
        {
            update.Set = update.Set.SetItem(field, value);
            return update;
        }

        public static IUpdateStatement SetVarParam(this IUpdateStatement update, Field field, string param = null)
        {
            update.Set = update.Set.SetItemVarParam(field, param);
            return update;
        }

        public static IUpdateStatement SetVarCustomer(this IUpdateStatement update, Field field, string customer)
        {
            update.Set = update.Set.SetItemVarCustomer(field, customer);
            return update;
        }

        #endregion

        #region Where
        
        public static IUpdateStatement Where(this IUpdateStatement update, ISimpleValue filter)
        {
            update.Where = new WhereClause(filter);
            return update;
        }

        #endregion

        #region ISetClause

        #region Shortcut
        public static ISetClause Vals(this ISetClause set, IEnumerable<ISimpleValue> values)
        {
            return Values(set, values);
        }
        public static ISetClause Vals(this ISetClause set, params object[] values)
        {
            return Values(set, values);
        }
        public static ISetClause ValsP(this ISetClause set)
        {
            return ValuesVarParam(set);
        }
        public static ISetClause ValsC(this ISetClause set, params string[] customrs)
        {
            return ValuesVarCustomer(set, customrs);
        }

        public static ISetClause ValuesP(this ISetClause set)
        {
            return ValuesVarParam(set);
        }

        public static ISetClause ValuesC(this ISetClause set, params string[] customers)
        {
            return ValuesVarCustomer(set, customers);
        }

        public static ISetClause SetP(this ISetClause set, IField field, string param = null)
        {
            return SetItemVarParam(set, field, param);
        }

        public static ISetClause SetC(this ISetClause set, IField field, string customer)
        {
            return SetItemVarCustomer(set, field, customer);
        }

        public static ISetClause SetP(this ISetClause set, Field field, string param = null)
        {
            return SetItemVarParam(set, field, param);
        }

        public static ISetClause SetC(this ISetClause set, Field field, string customer)
        {
            return SetItemVarCustomer(set, field, customer);
        }

        #endregion

        public static ISetClause Values(this ISetClause set, IEnumerable<ISimpleValue> values)
        {
            var list = values.ToList();
            for (int i = 0, j = 0; i < set.SetFields.Length && j < list.Count;)
            {
                var setitem = set.SetFields[i++];
                // 避免覆盖
                if (setitem.Value == null) setitem.Value = list[j++];
            }
            set.SetFields = set.SetFields;
            return set;
        }

        public static ISetClause Values(this ISetClause set, params object[] values)
        {
            return Values(set, values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val)));
        }

        public static ISetClause ValuesVarParam(this ISetClause set)
        {
            return Values(set, set.SetFields.ToList().FindAll(s => s.Value == null).Select(s => s.Field.ToParam()));
        }

        public static ISetClause ValuesVarCustomer(this ISetClause set, params string[] customrs)
        {
            return Values(set, customrs.Select(val => new CustomerExpression(val)));
        }

        public static ISetClause SetItem(this ISetClause set, ISetFieldExpression setItem)
        {
            var list = (set.SetFields?.ToList() ?? new List<ISetFieldExpression>());
            list.RemoveAll(s => s.Field.Name == setItem.Field.Name);
            list.Add(setItem);
            set.SetFields = list.ToArray();
            return set;
        }

        public static ISetClause SetItem(this ISetClause set, IField field, ISimpleValue value)
        {
            return SetItem(set, field.Set(value));
        }

        public static ISetClause SetItem(this ISetClause set, IField field, object value)
        {
            return SetItem(set, field.Set(value));
        }

        public static ISetClause SetItemVarParam(this ISetClause set, IField field, string param = null)
        {
            return SetItem(set, field.SetVarParam(param));
        }

        public static ISetClause SetItemVarCustomer(this ISetClause set, IField field, string customer)
        {
            return SetItem(set, field.SetVarCustomer(customer));
        }

        public static ISetClause SetItem(this ISetClause set, Field field, ISimpleValue value)
        {
            return SetItem(set, field.Set(value));
        }

        public static ISetClause SetItem(this ISetClause set, Field field, object value)
        {
            return SetItem(set, field.Set(value));
        }

        public static ISetClause SetItemVarParam(this ISetClause set, Field field, string param = null)
        {
            return SetItem(set, field.SetVarParam(param));
        }

        public static ISetClause SetItemVarCustomer(this ISetClause set, Field field, string customer)
        {
            return SetItem(set, field.SetVarCustomer(customer));
        }

        #endregion

        #region ISetItemExpression

        #region Shortcut

        public static ISetFieldExpression ValObj(this ISetFieldExpression setItem, object value)
        {
            return ValueVarObject(setItem, value);
        }

        public static ISetFieldExpression ValP(this ISetFieldExpression setItem, string param = null)
        {
            return ValueVarParam(setItem, param);
        }

        public static ISetFieldExpression ValC(this ISetFieldExpression setItem, string customer)
        {
            return ValueVarCustomer(setItem, customer);
        }

        public static ISetFieldExpression ValueObj(this ISetFieldExpression setItem, object value)
        {
            return ValueVarObject(setItem, value);
        }

        public static ISetFieldExpression ValueP(this ISetFieldExpression setItem, string param = null)
        {
            return ValueVarParam(setItem, param);
        }

        public static ISetFieldExpression ValueC(this ISetFieldExpression setItem, string customer)
        {
            return ValueVarCustomer(setItem, customer);
        }

        #endregion

        public static ISetFieldExpression ValueVarObject(this ISetFieldExpression setItem, object value)
        {
            setItem.Value = new LiteralValue(value);
            return setItem;
        }

        public static ISetFieldExpression ValueVarParam(this ISetFieldExpression setItem, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = setItem.Field.Name;
            setItem.Value = new Param(param);
            return setItem;
        }

        public static ISetFieldExpression ValueVarCustomer(this ISetFieldExpression setItem, string customer)
        {
            setItem.Value = new CustomerExpression(customer);
            return setItem;
        }

        #endregion
    }
}
