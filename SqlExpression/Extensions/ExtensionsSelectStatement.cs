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

        #endregion

        public static SelectStatement Select(this ITableExpression table)
        {
            return new SelectStatement(new ITableExpression[] { table });
        }

        public static SelectStatement Select(this ITableExpression table, IEnumerable<ISelectItemExpression> items)
        {
            return new SelectStatement(new ITableExpression[] { table }, items.ToArray(), null, null);
        }

        public static SelectStatement Select(this ITableExpression table, params ISelectItemExpression[] items)
        {
            return Select(table, items.Cast<ISelectItemExpression>());
        }

        public static SelectStatement Select(this ITableExpression table, params ColumnExpression[] items)
        {
            return Select(table, items.Cast<ISelectItemExpression>());
        }

        public static SelectStatement SelectVarCustomer(this ITableExpression table, IEnumerable<string> customers)
        {
            var columns = customers.Select(c => new CustomerExpression(c));
            return Select(table, columns);
        }

        public static SelectStatement SelectVarCustomer(this ITableExpression table, params string[] customers)
        {
            return SelectVarCustomer(table, customers.AsEnumerable());
        }
        public static SelectStatement Select(this IEnumerable<ITableExpression> tables)
        {
            return new SelectStatement(tables.ToArray());
        }

        public static SelectStatement Select(this IEnumerable<ITableExpression> tables, IEnumerable<ISelectItemExpression> items)
        {
            return new SelectStatement(tables.ToArray(), items.ToArray(), null, null);
        }

        public static SelectStatement Select(this IEnumerable<ITableExpression> tables, params ISelectItemExpression[] items)
        {
            return Select(tables, items.Cast<ISelectItemExpression>());
        }

        public static SelectStatement Select(this IEnumerable<ITableExpression> tables, params ColumnExpression[] items)
        {
            return Select(tables, items.Cast<ISelectItemExpression>());
        }

        public static SelectStatement SelectVarCustomer(this IEnumerable<ITableExpression> tables, IEnumerable<string> customers)
        {
            var columns = customers.Select(c => new CustomerExpression(c));
            return new SelectStatement(tables.ToArray(), columns.ToArray(), null, null);
        }

        public static SelectStatement SelectVarCustomer(this IEnumerable<ITableExpression> tables, params string[] customers)
        {
            return SelectVarCustomer(tables, customers.AsEnumerable());
        }

        #endregion

        #region Get

        #region Shortcut

        public static ISelectStatement GetC(this ISelectStatement select, string item)
        {
            return GetVarCustomer(select, item);
        }
        public static ISelectStatement GetC(this ISelectStatement select, IEnumerable<string> items)
        {
            return GetVarCustomer(select, items);
        }

        public static ISelectStatement GetC(this ISelectStatement select, params string[] items)
        {
            return GetVarCustomer(select, items);
        }

        #endregion

        public static ISelectStatement GetAs(this ISelectStatement select, ISelectItemExpression item, IColumnExpression asProperty)
        {
            ISelectItemExpression selectItem = asProperty == null ? item : new AsExpression(item, asProperty) as ISelectItemExpression;
            var list = (select.Items?.ToList() ?? new List<ISelectItemExpression>());
            list.Add(selectItem);
            select.Items = list.ToArray();
            return select;
        }

        public static ISelectStatement GetAs(this ISelectStatement select, ISelectItemExpression item, ColumnExpression asProperty)
        {
            return GetAs(select, item as ISelectItemExpression, asProperty as IColumnExpression);
        }

        public static ISelectStatement GetAs(this ISelectStatement select, ColumnExpression item, IColumnExpression asProperty)
        {
            return GetAs(select, item as ISelectItemExpression, asProperty);
        }

        public static ISelectStatement GetAs(this ISelectStatement select, ColumnExpression item, ColumnExpression asProperty)
        {
            return GetAs(select, item as ISelectItemExpression, asProperty as IColumnExpression);
        }

        public static ISelectStatement Get(this ISelectStatement select, ISelectItemExpression item)
        {
            return GetAs(select, item, null as IColumnExpression);
        }

        public static ISelectStatement Get(this ISelectStatement select, ColumnExpression item)
        {
            return Get(select, item as ISelectItemExpression);
        }

        public static ISelectStatement Get(this ISelectStatement select, IEnumerable<ISelectItemExpression> items)
        {
            var list = (select.Items?.ToList() ?? new List<ISelectItemExpression>());
            list.AddRange(items);
            select.Items = list.ToArray();
            return select;
        }

        public static ISelectStatement Get(this ISelectStatement select, params ISelectItemExpression[] items)
        {
            return Get(select, items.Cast<ISelectItemExpression>());
        }

        public static ISelectStatement Get(this ISelectStatement select, params ColumnExpression[] items)
        {
            return Get(select, items.Cast<ISelectItemExpression>());
        }

        public static ISelectStatement GetVarCustomer(this ISelectStatement select, string item)
        {
            return Get(select, new CustomerExpression(item));
        }

        public static ISelectStatement GetVarCustomer(this ISelectStatement select, IEnumerable<string> items)
        {
            return Get(select, items.Select(i => new CustomerExpression(i)));
        }

        public static ISelectStatement GetVarCustomer(this ISelectStatement select, params string[] items)
        {
            return Get(select, items.Select(i => new CustomerExpression(i)));
        }

        #endregion

        #region From

        public static ISelectStatement From(this ISelectStatement select, IEnumerable<ITableExpression> tables)
        {
            select.Tables = tables.ToArray();
            return select;
        }

        public static ISelectStatement From(this ISelectStatement select, params TableExpression[] tables)
        {
            select.Tables = tables;
            return select;
        }

        #endregion

        #region Join

        #region Shortcut
        public static ISelectStatement J(this ISelectStatement select, IJoinExpression join)
        {
            return Join(select, join);
        }
        public static ISelectStatement J(this ISelectStatement select, ITableExpression table, IFilterExpression on, IJoinOperator joinOperator)
        {
            return Join(select, table, on, joinOperator);
        }
        public static ISelectStatement J(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            return Join(select, table, on);
        }
        public static ISelectStatement IJ(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            return IJ(select, table, on);
        }
        public static ISelectStatement LJ(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            return LeftJoin(select, table, on);
        }
        public static ISelectStatement RJ(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            return RightJoin(select, table, on);
        }
        public static ISelectStatement FJ(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            return FullJoin(select, table, on);
        }
        #endregion

        public static ISelectStatement Join(this ISelectStatement select, IJoinExpression join)
        {
            var joins = select?.Joins?.ToList() ?? new List<IJoinExpression>();
            joins.Add(join);
            select.Joins = joins.ToArray();
            return select;
        }

        public static ISelectStatement Join(this ISelectStatement select, ITableExpression table, IFilterExpression on, IJoinOperator joinOperator)
        {
            IJoinExpression join = new JoinExpression(joinOperator, table, on);
            return Join(select, join);
        }

        public static ISelectStatement Join(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            return Join(select, table, on, Operator.InnerJoin);
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

        public static ISelectStatement FullJoin(this ISelectStatement select, ITableExpression table, IFilterExpression on)
        {
            IJoinExpression join = new JoinExpression(Operator.FullJoin, table, on);
            return Join(select, join);
        }

        #endregion

        #region Where

        public static ISelectStatement Where(this ISelectStatement select, IFilterExpression filter)
        {
            select.Where = new WhereClause(filter);
            return select;
        }

        #endregion

        #region Union

        #region Shortcut
        public static UnionStatement U(this ISelectStatement select, IEnumerable<ISelectStatement> otherselects)
        {
            return Union(select, otherselects);
        }
        public static UnionStatement U(this ISelectStatement select, params ISelectStatement[] otherselects)
        {
            return Union(select, otherselects);
        }
        public static UnionStatement UA(this ISelectStatement select, IEnumerable<ISelectStatement> otherselects)
        {
            return UnionAll(select, otherselects);
        }
        public static UnionStatement UA(this ISelectStatement select, params ISelectStatement[] otherselects)
        {
            return UnionAll(select, otherselects);
        }
        #endregion

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

        #endregion

        #region GroupBy

        #region Shortcut
        public static ISelectStatement GB(this ISelectStatement select, string column)
        {
            return GroupBy(select, column);
        }
        public static ISelectStatement GB(this ISelectStatement select, IColumnExpression column)
        {
            return GroupBy(select, column);
        }
        public static ISelectStatement H(this ISelectStatement select, IFilterExpression filter)
        {
            return Having(select, filter);
        }
        #endregion

        public static ISelectStatement GroupBy(this ISelectStatement select, string column)
        {
            return GroupBy(select, new ColumnExpression(column));
        }

        public static ISelectStatement GroupBy(this ISelectStatement select, IColumnExpression column)
        {
            select.GroupBy = new GroupByClause(column);
            return select;
        }

        public static ISelectStatement Having(this ISelectStatement select, IFilterExpression filter)
        {
            select.Having = new HavingClause(filter);
            return select;
        }

        #endregion

        #region OrderBy

        #region Shortcut
        public static ISelectStatement OB(this ISelectStatement select, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(select, orders);
        }
        public static ISelectStatement OB(this ISelectStatement select, params IOrderExpression[] orders)
        {
            return OrderBy(select, orders);
        }
        public static ISelectStatement OB(this ISelectStatement select, IOrderByClause orderby)
        {
            return OrderBy(select, orderby);
        }
        public static IUnionStatement OB(this IUnionStatement union, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(union, orders);
        }
        public static IUnionStatement OB(this IUnionStatement union, params IOrderExpression[] orders)
        {
            return OrderBy(union, orders);
        }
        public static IUnionStatement OB(this IUnionStatement union, IOrderByClause orderby)
        {
            return OrderBy(union, orderby);
        }
        #endregion


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
    }
}
