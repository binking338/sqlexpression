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

        public static IUpdateStatement UpdateP(this ITableExpression table, IEnumerable<IColumnExpression> columns)
        {
            return UpdateVarParam(table, columns);
        }

        public static IUpdateStatement UpdateP(this ITableExpression table, params IColumnExpression[] columns)
        {
            return UpdateVarParam(table, columns);
        }

        public static IUpdateStatement UpdateP(this ITableExpression table, params ColumnExpression[] columns)
        {
            return UpdateVarParam(table, columns);
        }

        #endregion

        public static UpdateStatement Update(this ITableExpression table)
        {
            return new UpdateStatement(table);
        }

        public static UpdateStatement Update(this ITableExpression table, IEnumerable<IColumnExpression> columns)
        {
            var set = new SetClause(columns.Select(c => new SetItemExpression(c, null) as ISetItemExpression).ToArray());
            return new UpdateStatement(table, set, null);
        }

        public static UpdateStatement Update(this ITableExpression table, params IColumnExpression[] columns)
        {
            return Update(table, columns.AsEnumerable());
        }

        public static UpdateStatement Update(this ITableExpression table, params ColumnExpression[] columns)
        {
            return Update(table, columns.AsEnumerable());
        }

        public static UpdateStatement UpdateVarParam(this ITableExpression table, IEnumerable<IColumnExpression> columns)
        {
            var set = new SetClause(columns.Select(col => col.SetVarParam() as ISetItemExpression).ToArray());
            return new UpdateStatement(table, set, null);
        }

        public static UpdateStatement UpdateVarParam(this ITableExpression table, params IColumnExpression[] columns)
        {
            return UpdateVarParam(table, columns.AsEnumerable());
        }

        public static UpdateStatement UpdateVarParam(this ITableExpression table, params ColumnExpression[] columns)
        {
            return UpdateVarParam(table, columns.AsEnumerable());
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

        public static IUpdateStatement SetP(this IUpdateStatement update, IColumnExpression column, string param = null)
        {
            return SetVarParam(update, column, param);
        }

        public static IUpdateStatement SetC(this IUpdateStatement update, IColumnExpression column, string customer)
        {
            return SetVarCustomer(update, column, customer);
        }

        public static IUpdateStatement SetP(this IUpdateStatement update, ColumnExpression column, string param = null)
        {
            return SetVarParam(update, column, param);
        }

        public static IUpdateStatement SetC(this IUpdateStatement update, ColumnExpression column, string customer)
        {
            return SetVarCustomer(update, column, customer);
        }

        #endregion

        public static IUpdateStatement Set(this IUpdateStatement update, ISetItemExpression setItem)
        {
            update.Set = update.Set.SetItem(setItem);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, IColumnExpression column, IValueExpression value)
        {
            update.Set = update.Set.SetItem(column, value);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, IColumnExpression column, object value)
        {
            update.Set = update.Set.SetItem(column, value);
            return update;
        }

        public static IUpdateStatement SetVarParam(this IUpdateStatement update, IColumnExpression column, string param = null)
        {
            update.Set = update.Set.SetItemVarParam(column, param);
            return update;
        }

        public static IUpdateStatement SetVarCustomer(this IUpdateStatement update, IColumnExpression column, string customer)
        {
            update.Set = update.Set.SetItemVarCustomer(column, customer);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, ColumnExpression column, IValueExpression value)
        {
            update.Set = update.Set.SetItem(column, value);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, ColumnExpression column, object value)
        {
            update.Set = update.Set.SetItem(column, value);
            return update;
        }

        public static IUpdateStatement SetVarParam(this IUpdateStatement update, ColumnExpression column, string param = null)
        {
            update.Set = update.Set.SetItemVarParam(column, param);
            return update;
        }

        public static IUpdateStatement SetVarCustomer(this IUpdateStatement update, ColumnExpression column, string customer)
        {
            update.Set = update.Set.SetItemVarCustomer(column, customer);
            return update;
        }

        #endregion

        #region Where

        public static IUpdateStatement W(this IUpdateStatement update, IFilterExpression filter)
        {
            return Where(update, filter);
        }

        public static IUpdateStatement Where(this IUpdateStatement update, IFilterExpression filter)
        {
            update.Where = new WhereClause(filter);
            return update;
        }

        #endregion

        #region ISetClause

        #region Shortcut
        public static ISetClause Vals(this ISetClause set, IEnumerable<IValueExpression> values)
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

        public static ISetClause SetP(this ISetClause set, IColumnExpression column, string param = null)
        {
            return SetItemVarParam(set, column, param);
        }

        public static ISetClause SetC(this ISetClause set, IColumnExpression column, string customer)
        {
            return SetItemVarCustomer(set, column, customer);
        }

        public static ISetClause SetP(this ISetClause set, ColumnExpression column, string param = null)
        {
            return SetItemVarParam(set, column, param);
        }

        public static ISetClause SetC(this ISetClause set, ColumnExpression column, string customer)
        {
            return SetItemVarCustomer(set, column, customer);
        }

        #endregion

        public static ISetClause Values(this ISetClause set, IEnumerable<IValueExpression> values)
        {
            var list = values.ToList();
            for (int i = 0, j = 0; i < set.Sets.Length && j < list.Count;)
            {
                var setitem = set.Sets[i++];
                // 避免覆盖
                if (setitem.Value == null) setitem.Value = list[j++];
            }
            set.Sets = set.Sets;
            return set;
        }

        public static ISetClause Values(this ISetClause set, params object[] values)
        {
            return Values(set, values.Select(val => val is IValueExpression ? val as IValueExpression : new LiteralValueExpression(val)));
        }

        public static ISetClause ValuesVarParam(this ISetClause set)
        {
            return Values(set, set.Sets.ToList().FindAll(s => s.Value == null).Select(s => s.Column.ToParam()));
        }

        public static ISetClause ValuesVarCustomer(this ISetClause set, params string[] customrs)
        {
            return Values(set, customrs.Select(val => new CustomerExpression(val)));
        }

        public static ISetClause SetItem(this ISetClause set, ISetItemExpression setItem)
        {
            var list = (set.Sets?.ToList() ?? new List<ISetItemExpression>());
            list.RemoveAll(s => s.Column.Name == setItem.Column.Name);
            list.Add(setItem);
            set.Sets = list.ToArray();
            return set;
        }

        public static ISetClause SetItem(this ISetClause set, IColumnExpression column, IValueExpression value)
        {
            return SetItem(set, column.Set(value));
        }

        public static ISetClause SetItem(this ISetClause set, IColumnExpression column, object value)
        {
            return SetItem(set, column.Set(value));
        }

        public static ISetClause SetItemVarParam(this ISetClause set, IColumnExpression column, string param = null)
        {
            return SetItem(set, column.SetVarParam(param));
        }

        public static ISetClause SetItemVarCustomer(this ISetClause set, IColumnExpression column, string customer)
        {
            return SetItem(set, column.SetVarCustomer(customer));
        }

        public static ISetClause SetItem(this ISetClause set, ColumnExpression column, IValueExpression value)
        {
            return SetItem(set, column.Set(value));
        }

        public static ISetClause SetItem(this ISetClause set, ColumnExpression column, object value)
        {
            return SetItem(set, column.Set(value));
        }

        public static ISetClause SetItemVarParam(this ISetClause set, ColumnExpression column, string param = null)
        {
            return SetItem(set, column.SetVarParam(param));
        }

        public static ISetClause SetItemVarCustomer(this ISetClause set, ColumnExpression column, string customer)
        {
            return SetItem(set, column.SetVarCustomer(customer));
        }

        #endregion

        #region ISetItemExpression

        #region Shortcut

        public static ISetItemExpression ValObj(this ISetItemExpression setItem, object value)
        {
            return ValueVarObject(setItem, value);
        }

        public static ISetItemExpression ValP(this ISetItemExpression setItem, string param = null)
        {
            return ValueVarParam(setItem, param);
        }

        public static ISetItemExpression ValC(this ISetItemExpression setItem, string customer)
        {
            return ValueVarCustomer(setItem, customer);
        }

        public static ISetItemExpression ValueObj(this ISetItemExpression setItem, object value)
        {
            return ValueVarObject(setItem, value);
        }

        public static ISetItemExpression ValueP(this ISetItemExpression setItem, string param = null)
        {
            return ValueVarParam(setItem, param);
        }

        public static ISetItemExpression ValueC(this ISetItemExpression setItem, string customer)
        {
            return ValueVarCustomer(setItem, customer);
        }

        #endregion

        public static ISetItemExpression ValueVarObject(this ISetItemExpression setItem, object value)
        {
            setItem.Value = new LiteralValueExpression(value);
            return setItem;
        }

        public static ISetItemExpression ValueVarParam(this ISetItemExpression setItem, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = setItem.Column.Name;
            setItem.Value = new ParamExpression(param);
            return setItem;
        }

        public static ISetItemExpression ValueVarCustomer(this ISetItemExpression setItem, string customer)
        {
            setItem.Value = new CustomerExpression(customer);
            return setItem;
        }

        #endregion
    }
}
