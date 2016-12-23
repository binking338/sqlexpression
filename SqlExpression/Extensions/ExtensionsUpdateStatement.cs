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

        public static IUpdateStatement UpdateP(this ITableExpression table, IEnumerable<string> properties)
        {
            return UpdateVarParam(table, properties);
        }

        public static IUpdateStatement UpdateP(this ITableExpression table, IEnumerable<IPropertyExpression> properties)
        {
            return UpdateVarParam(table, properties);
        }

        public static IUpdateStatement UpdateP(this ITableExpression table, IEnumerable<PropertyExpression> properties)
        {
            return UpdateVarParam(table, properties);
        }

        public static IUpdateStatement UpdateP(this ITableExpression table, params PropertyExpression[] properties)
        {
            return UpdateVarParam(table, properties);
        }

        public static IUpdateStatement ValuesP(this IUpdateStatement update)
        {
            return ValuesVarParam(update);
        }

        public static IUpdateStatement ValuesC(this IUpdateStatement update, params string[] customers)
        {
            return ValuesVarCustomer(update, customers);
        }

        public static IUpdateStatement SetP(this IUpdateStatement update, IPropertyExpression property, string param = null)
        {
            return SetVarParam(update, property, param);
        }

        public static IUpdateStatement SetC(this IUpdateStatement update, IPropertyExpression property, string customer)
        {
            return SetVarCustomer(update, property, customer);
        }

        public static IUpdateStatement SetP(this IUpdateStatement update, PropertyExpression property, string param = null)
        {
            return SetVarParam(update, property, param);
        }

        public static IUpdateStatement SetC(this IUpdateStatement update, PropertyExpression property, string customer)
        {
            return SetVarCustomer(update, property, customer);
        }

        public static ISetClause ValuesP(this ISetClause set)
        {
            return ValuesVarParam(set);
        }

        public static ISetClause ValuesC(this ISetClause set, params string[] customers)
        {
            return ValuesVarCustomer(set, customers);
        }

        public static ISetClause SetP(this ISetClause set, IPropertyExpression property, string param = null)
        {
            return SetItemVarParam(set, property, param);
        }

        public static ISetClause SetC(this ISetClause set, IPropertyExpression property, string customer)
        {
            return SetItemVarCustomer(set, property, customer);
        }

        public static ISetClause SetP(this ISetClause set, PropertyExpression property, string param = null)
        {
            return SetItemVarParam(set, property, param);
        }

        public static ISetClause SetC(this ISetClause set, PropertyExpression property, string customer)
        {
            return SetItemVarCustomer(set, property, customer);
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

        public static UpdateStatement Update(this ITableExpression table, IEnumerable<IPropertyExpression> properties)
        {
            var set = new SetClause(properties.Select(c => new SetItemExpression(c, null) as ISetItemExpression).ToArray());
            return new UpdateStatement(table, set, null);
        }

        public static UpdateStatement Update(this ITableExpression table, IEnumerable<string> properties)
        {
            var columns = properties.Select(prop => new PropertyExpression(prop) as IPropertyExpression);
            return Update(table, columns);
        }

        public static UpdateStatement Update(this ITableExpression table, IEnumerable<PropertyExpression> properties)
        {
            var columns = properties.Select(prop => prop as IPropertyExpression);
            return Update(table, columns);
        }

        public static UpdateStatement Update(this ITableExpression table, params PropertyExpression[] properties)
        {
            return Update(table, properties.AsEnumerable());
        }

        public static UpdateStatement UpdateVarParam(this ITableExpression table, IEnumerable<IPropertyExpression> properties)
        {
            var set = new SetClause(properties.Select(prop => prop.SetVarParam() as ISetItemExpression).ToArray());
            return new UpdateStatement(table, set, null);
        }

        public static UpdateStatement UpdateVarParam(this ITableExpression table, IEnumerable<string> properties)
        {
            var columns = properties.Select(prop => new PropertyExpression(prop) as IPropertyExpression);
            return UpdateVarParam(table, columns);
        }

        public static UpdateStatement UpdateVarParam(this ITableExpression table, IEnumerable<PropertyExpression> properties)
        {
            var columns = properties.Select(prop => prop as IPropertyExpression);
            return UpdateVarParam(table, columns);
        }

        public static UpdateStatement UpdateVarParam(this ITableExpression table, params PropertyExpression[] properties)
        {
            return UpdateVarParam(table, properties.AsEnumerable());
        }

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

        public static IUpdateStatement Set(this IUpdateStatement update, ISetItemExpression setItem)
        {
            update.Set = update.Set.SetItem(setItem);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, IPropertyExpression property, IValueExpression value)
        {
            update.Set = update.Set.SetItem(property, value);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, IPropertyExpression property, object value)
        {
            update.Set = update.Set.SetItem(property, value);
            return update;
        }

        public static IUpdateStatement SetVarParam(this IUpdateStatement update, IPropertyExpression property, string param = null)
        {
            update.Set = update.Set.SetItemVarParam(property, param);
            return update;
        }

        public static IUpdateStatement SetVarCustomer(this IUpdateStatement update, IPropertyExpression property, string customer)
        {
            update.Set = update.Set.SetItemVarCustomer(property, customer);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, PropertyExpression property, IValueExpression value)
        {
            update.Set = update.Set.SetItem(property, value);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, PropertyExpression property, object value)
        {
            update.Set = update.Set.SetItem(property, value);
            return update;
        }

        public static IUpdateStatement SetVarParam(this IUpdateStatement update, PropertyExpression property, string param = null)
        {
            update.Set = update.Set.SetItemVarParam(property, param);
            return update;
        }

        public static IUpdateStatement SetVarCustomer(this IUpdateStatement update, PropertyExpression property, string customer)
        {
            update.Set = update.Set.SetItemVarCustomer(property, customer);
            return update;
        }

        public static IUpdateStatement Where(this IUpdateStatement update, IFilterExpression filter)
        {
            update.Where = new WhereClause(filter);
            return update;
        }

        #region ISetExpression

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
            return Values(set, set.Sets.ToList().FindAll(s => s.Value == null).Select(s => s.Property.ToParam()));
        }

        public static ISetClause ValuesVarCustomer(this ISetClause set, params string[] customrs)
        {
            return Values(set, customrs.Select(val => new CustomerExpression(val)));
        }

        public static ISetClause SetItem(this ISetClause set, ISetItemExpression setItem)
        {
            var list = (set.Sets?.ToList() ?? new List<ISetItemExpression>());
            list.RemoveAll(s => s.Property.Name == setItem.Property.Name);
            list.Add(setItem);
            set.Sets = list.ToArray();
            return set;
        }

        public static ISetClause SetItem(this ISetClause set, IPropertyExpression property, IValueExpression value)
        {
            return SetItem(set, property.Set(value));
        }

        public static ISetClause SetItem(this ISetClause set, IPropertyExpression property, object value)
        {
            return SetItem(set, property.Set(value));
        }

        public static ISetClause SetItemVarParam(this ISetClause set, IPropertyExpression property, string param = null)
        {
            return SetItem(set, property.SetVarParam(param));
        }

        public static ISetClause SetItemVarCustomer(this ISetClause set, IPropertyExpression property, string customer)
        {
            return SetItem(set, property.SetVarCustomer(customer));
        }

        public static ISetClause SetItem(this ISetClause set, PropertyExpression property, IValueExpression value)
        {
            return SetItem(set, property.Set(value));
        }

        public static ISetClause SetItem(this ISetClause set, PropertyExpression property, object value)
        {
            return SetItem(set, property.Set(value));
        }

        public static ISetClause SetItemVarParam(this ISetClause set, PropertyExpression property, string param = null)
        {
            return SetItem(set, property.SetVarParam(param));
        }

        public static ISetClause SetItemVarCustomer(this ISetClause set, PropertyExpression property, string customer)
        {
            return SetItem(set, property.SetVarCustomer(customer));
        }

        #endregion

        #region ISetItemExpression

        public static ISetItemExpression ValueVarObject(this ISetItemExpression setItem, object value)
        {
            setItem.Value = new LiteralValueExpression(value);
            return setItem;
        }

        public static ISetItemExpression ValueVarParam(this ISetItemExpression setItem, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = setItem.Property.Name;
            setItem.Value = new ParamExpression(param);
            return setItem;
        }

        public static ISetItemExpression ValueVarCustomer(this ISetItemExpression setItem, string customer)
        {
            setItem.Value = new CustomerExpression(customer);
            return setItem;
        }

        #endregion

        #region OrderBy

        public static IUpdateStatement OrderBy(this IUpdateStatement update, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(update, orderby);
        }
        public static IUpdateStatement OrderBy(this IUpdateStatement update, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(update, orderby);
        }

        public static IUpdateStatement OrderBy(this IUpdateStatement update, IOrderByClause orderby)
        {
            update.OrderBy = orderby;
            return update;
        }

        #endregion

        #region Limit

        public static IUpdateStatement Limit(this IUpdateStatement update, int count)
        {
            update.Limit = new LimitClause(0, count);
            return update;
        }

        public static IUpdateStatement Limit(this IUpdateStatement update, IValueExpression count)
        {
            update.Limit = new LimitClause(null, count);
            return update;
        }

        #endregion

        #endregion
    }
}
