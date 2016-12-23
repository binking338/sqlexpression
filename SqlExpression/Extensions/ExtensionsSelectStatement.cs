using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsSelectStatement
    {
        #region Select

        #region ShortCut

        public static SelectStatement SelectC(this ITableExpression table, IEnumerable<string> customers)
        {
            return SelectVarCustomer(table, customers);
        }

        public static SelectStatement SelectC(this ITableExpression table, params string[] customers)
        {
            return SelectVarCustomer(table, customers);
        }

        public static SelectStatement SelectC(this IEnumerable<ITableExpression> tables, IEnumerable<string> customers)
        {
            return SelectVarCustomer(tables, customers);
        }

        public static SelectStatement SelectC(this IEnumerable<ITableExpression> tables, params string[] customers)
        {
            return SelectVarCustomer(tables, customers);
        }

        public static ISelectStatement GetC(this ISelectStatement select, string item, IPropertyExpression asProperty = null)
        {
            return GetVarCustomer(select, item, asProperty);
        }

        public static ISelectStatement GetC(this ISelectStatement select, string item, string asProperty)
        {
            return GetVarCustomer(select, item, asProperty);
        }

        #endregion

        public static SelectStatement Select(this ITableExpression table, IEnumerable<ISelectItemExpression> items)
        {
            return new SelectStatement(new ITableExpression[] { table }, items.ToArray(), null, null);
        }

        public static SelectStatement Select(this ITableExpression table, IEnumerable<string> properties)
        {
            var items = properties.Select(prop => new PropertyExpression(prop) as ISelectItemExpression);
            return Select(table, items.ToArray());
        }

        public static SelectStatement Select(this ITableExpression table, IEnumerable<PropertyExpression> items)
        {
            return new SelectStatement(new ITableExpression[] { table }, items.ToArray(), null, null);
        }

        public static SelectStatement Select(this ITableExpression table, params PropertyExpression[] items)
        {
            return new SelectStatement(new ITableExpression[] { table }, items, null, null);
        }

        public static SelectStatement SelectVarCustomer(this ITableExpression table, IEnumerable<string> customers)
        {
            var properties = customers.Select(c => new CustomerExpression(c));
            return Select(table, properties);
        }

        public static SelectStatement SelectVarCustomer(this ITableExpression table, params string[] customers)
        {
            return SelectVarCustomer(table, customers.AsEnumerable());
        }

        public static SelectStatement Select(this IEnumerable<ITableExpression> tables, IEnumerable<ISelectItemExpression> items)
        {
            return new SelectStatement(tables.ToArray(), items.ToArray(), null, null);
        }

        public static SelectStatement Select(this IEnumerable<ITableExpression> tables, IEnumerable<string> properties)
        {
            var items = properties.Select(prop => new PropertyExpression(prop));
            return new SelectStatement(tables.ToArray(), items.ToArray(), null, null);
        }

        public static SelectStatement Select(this IEnumerable<ITableExpression> tables, IEnumerable<PropertyExpression> items)
        {
            return new SelectStatement(tables.ToArray(), items.ToArray(), null, null);
        }

        public static SelectStatement Select(this IEnumerable<ITableExpression> tables, params PropertyExpression[] items)
        {
            return new SelectStatement(tables.ToArray(), items, null, null);
        }

        public static SelectStatement SelectVarCustomer(this IEnumerable<ITableExpression> tables, IEnumerable<string> customers)
        {
            var properties = customers.Select(c => new CustomerExpression(c));
            return new SelectStatement(tables.ToArray(), properties.ToArray(), null, null);
        }

        public static SelectStatement SelectVarCustomer(this IEnumerable<ITableExpression> tables, params string[] customers)
        {
            return SelectVarCustomer(tables, customers.AsEnumerable());
        }

        public static ISelectStatement Get(this ISelectStatement select, ISelectItemExpression item, IPropertyExpression asProperty = null)
        {
            ISelectItemExpression selectItem = asProperty == null ? asProperty : new AsExpression(item, asProperty) as ISelectItemExpression;
            var list = (select.Items?.ToList() ?? new List<ISelectItemExpression>());
            list.Add(selectItem);
            select.Items = list.ToArray();
            return select;
        }

        public static ISelectStatement Get(this ISelectStatement select, ISelectItemExpression item, string asProperty)
        {
            return Get(select, item, new PropertyExpression(asProperty));
        }

        public static ISelectStatement Get(this ISelectStatement select, string item, IPropertyExpression asProperty = null)
        {
            return Get(select, new PropertyExpression(item), asProperty);
        }

        public static ISelectStatement Get(this ISelectStatement select, string item, string asProperty)
        {
            return Get(select, item, new PropertyExpression(asProperty));
        }

        public static ISelectStatement GetVarCustomer(this ISelectStatement select, string item, IPropertyExpression asProperty = null)
        {
            return Get(select, new CustomerExpression(item), asProperty);
        }

        public static ISelectStatement GetVarCustomer(this ISelectStatement select, string item, string asProperty)
        {
            return Get(select, new CustomerExpression(item), asProperty);
        }

        public static ISelectStatement Join(this ISelectStatement select, IJoinExpression join)
        {
            var joins = select?.Joins.ToList() ?? new List<IJoinExpression>();
            joins.Add(join);
            select.Joins = joins.ToArray();
            return select;
        }

        public static ISelectStatement InnerJoin(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            IJoinExpression join = new JoinExpression(Operator.InnerJoin, table, on);
            return Join(select, join);
        }

        public static ISelectStatement LeftJoin(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            IJoinExpression join = new JoinExpression(Operator.LeftJoin, table, on);
            return Join(select, join);
        }

        public static ISelectStatement RightJoin(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            IJoinExpression join = new JoinExpression(Operator.RightJoin, table, on);
            return Join(select, join);
        }

        public static ISelectStatement Where(this ISelectStatement select, IFilterExpression filter)
        {
            select.Where = new WhereClause(filter);
            return select;
        }

        public static UnionStatement Union(this ISelectStatement select, IEnumerable<ISelectStatement> otherselects)
        {
            var list = new List<IUnionItemExpression>();
            foreach (var s in otherselects)
            {
                list.Add(new UnionItemExpression(Operator.Union, s));
            }
            return new UnionStatement(select, list.ToArray(), null);
        }

        public static UnionStatement Union(this ISelectStatement select, params ISelectStatement[] otherselects)
        {
            return Union(select, otherselects.AsEnumerable());
        }

        public static UnionStatement UnionAll(this ISelectStatement select, IEnumerable<ISelectStatement> otherselects)
        {
            var list = new List<IUnionItemExpression>();
            foreach (var s in otherselects)
            {
                list.Add(new UnionItemExpression(Operator.UnionAll, s));
            }
            return new UnionStatement(select, list.ToArray(), null);
        }

        public static UnionStatement UnionAll(this ISelectStatement select, params ISelectStatement[] otherselects)
        {
            return UnionAll(select, otherselects.AsEnumerable());
        }

        #region GroupBy

        public static ISelectStatement GroupBy(this ISelectStatement select, string property)
        {
            return GroupBy(select, new PropertyExpression(property));
        }

        public static ISelectStatement GroupBy(this ISelectStatement select, IPropertyExpression property)
        {
            select.GroupBy = new GroupByClause(property);
            return select;
        }

        public static ISelectStatement Having(this ISelectStatement select, IFilterExpression filter)
        {
            select.Having = new HavingClause(filter);
            return select;
        }

        #endregion

        #region OrderBy

        public static ISelectStatement OrderBy(this ISelectStatement select, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(select, orderby);
        }

        public static ISelectStatement OrderBy(this ISelectStatement select, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(select, orderby);
        }

        public static ISelectStatement OrderBy(this ISelectStatement select, IOrderByClause orderby)
        {
            select.OrderBy = orderby;
            return select;
        }

        public static IUnionStatement OrderBy(this IUnionStatement union, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(union, orderby);
        }

        public static IUnionStatement OrderBy(this IUnionStatement union, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(union, orderby);
        }

        public static IUnionStatement OrderBy(this IUnionStatement union, IOrderByClause orderby)
        {
            union.OrderBy = orderby;
            return union;
        }

        #endregion


        #endregion
    }
}
