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

        public static SelectStatement SelectC(this ITable table, IEnumerable<string> customers)
        {
            return SelectVarCustomer(table, customers);
        }

        public static SelectStatement SelectC(this ITable table, params string[] customers)
        {
            return SelectVarCustomer(table, customers);
        }

        public static SelectStatement SelectC(this IEnumerable<ITable> tables, IEnumerable<string> customers)
        {
            return SelectVarCustomer(tables, customers);
        }

        public static SelectStatement SelectC(this IEnumerable<ITable> tables, params string[] customers)
        {
            return SelectVarCustomer(tables, customers);
        }

        #endregion

        public static SelectStatement Select(this ITable table)
        {
            return new SelectStatement(new ITable[] { table });
        }

        public static SelectStatement Select(this ITable table, IEnumerable<ISelectFieldExpression> items)
        {
            return new SelectStatement(new ITable[] { table }, items.ToArray(), null, null);
        }

        public static SelectStatement Select(this ITable table, params ISelectFieldExpression[] items)
        {
            return Select(table, items.Cast<ISelectFieldExpression>());
        }

        public static SelectStatement Select(this ITable table, params IValue[] items)
        {
            return Select(table, items.Select(item => new SelectFieldExpression(item)));
        }

        public static SelectStatement SelectVarCustomer(this ITable table, IEnumerable<string> customers)
        {
            var columns = customers.Select(c => new SelectFieldExpression(new CustomerExpression(c)));
            return Select(table, columns);
        }

        public static SelectStatement SelectVarCustomer(this ITable table, params string[] customers)
        {
            return SelectVarCustomer(table, customers.AsEnumerable());
        }

        public static SelectStatement Select(this IEnumerable<ITable> tables)
        {
            return new SelectStatement(tables.ToArray());
        }

        public static SelectStatement Select(this IEnumerable<ITable> tables, IEnumerable<ISelectFieldExpression> items)
        {
            return new SelectStatement(tables.ToArray(), items.ToArray(), null, null);
        }

        public static SelectStatement Select(this IEnumerable<ITable> tables, params ISelectFieldExpression[] items)
        {
            return Select(tables, items.Cast<ISelectFieldExpression>());
        }

        public static SelectStatement Select(this IEnumerable<ITable> tables, params IValue[] items)
        {
            return Select(tables, items.Select(item => new SelectFieldExpression(item)));
        }

        public static SelectStatement SelectVarCustomer(this IEnumerable<ITable> tables, IEnumerable<string> customers)
        {
            var columns = customers.Select(c => new SelectFieldExpression(new CustomerExpression(c)));
            return new SelectStatement(tables.ToArray(), columns.ToArray(), null, null);
        }

        public static SelectStatement SelectVarCustomer(this IEnumerable<ITable> tables, params string[] customers)
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

        public static ISelectStatement GetAs(this ISelectStatement select, IValue item, ISelectFieldAlias alias)
        {
            ISelectFieldExpression selectField = new SelectFieldExpression(item, alias);
            return Get(select, selectField);
        }

        public static ISelectStatement GetAs(this ISelectStatement select, IValue item, string alias)
        {
            return GetAs(select, item, new SelectFieldAlias(alias));
        }

        public static ISelectStatement Get(this ISelectStatement select, IEnumerable<ISelectFieldExpression> items)
        {
            var list = (select.Fields?.ToList() ?? new List<ISelectFieldExpression>());
            list.AddRange(items);
            select.Fields = list.ToArray();
            return select;
        }

        public static ISelectStatement Get(this ISelectStatement select, params ISelectFieldExpression[] items)
        {
            return Get(select, items.Cast<ISelectFieldExpression>());
        }

        public static ISelectStatement Get(this ISelectStatement select, params IValue[] items)
        {
            return Get(select, items.Select(item => new SelectFieldExpression(item)));
        }

        public static ISelectStatement GetVarCustomer(this ISelectStatement select, IEnumerable<string> items)
        {
            return Get(select, items.Select(item => new SelectFieldExpression(new CustomerExpression(item))));
        }

        public static ISelectStatement GetVarCustomer(this ISelectStatement select, params string[] items)
        {
            return Get(select, items.Select(item => new SelectFieldExpression(new CustomerExpression(item))));
        }

        #endregion

        #region From

        public static ISelectStatement From(this ISelectStatement select, IEnumerable<ITable> tables)
        {
            select.Tables = tables.ToArray();
            return select;
        }

        public static ISelectStatement From(this ISelectStatement select, params Table[] tables)
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
        public static ISelectStatement J(this ISelectStatement select, ITable table, IFilterExpression on, IJoinOperator joinOperator)
        {
            return Join(select, table, on, joinOperator);
        }
        public static ISelectStatement J(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            return Join(select, table, on);
        }
        public static ISelectStatement IJ(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            return IJ(select, table, on);
        }
        public static ISelectStatement LJ(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            return LeftJoin(select, table, on);
        }
        public static ISelectStatement RJ(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            return RightJoin(select, table, on);
        }
        public static ISelectStatement FJ(this ISelectStatement select, ITable table, IFilterExpression on)
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

        public static ISelectStatement Join(this ISelectStatement select, ITable table, IFilterExpression on, IJoinOperator joinOperator)
        {
            IJoinExpression join = new JoinExpression(joinOperator, table, on);
            return Join(select, join);
        }

        public static ISelectStatement Join(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            return Join(select, table, on, Operator.InnerJoin);
        }

        public static ISelectStatement InnerJoin(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            IJoinExpression join = new JoinExpression(Operator.InnerJoin, table, on);
            return Join(select, join);
        }

        public static ISelectStatement LeftJoin(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            IJoinExpression join = new JoinExpression(Operator.LeftJoin, table, on);
            return Join(select, join);
        }

        public static ISelectStatement RightJoin(this ISelectStatement select, ITable table, IFilterExpression on)
        {
            IJoinExpression join = new JoinExpression(Operator.RightJoin, table, on);
            return Join(select, join);
        }

        public static ISelectStatement FullJoin(this ISelectStatement select, ITable table, IFilterExpression on)
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
            var list = new List<IUnionExpression>();
            foreach (var s in otherselects)
            {
                list.Add(new UnionExpression(Operator.Union, s));
            }
            return new UnionStatement(select, list.ToArray(), null);
        }

        public static UnionStatement Union(this ISelectStatement select, params ISelectStatement[] otherselects)
        {
            return Union(select, otherselects.AsEnumerable());
        }

        public static UnionStatement UnionAll(this ISelectStatement select, IEnumerable<ISelectStatement> otherselects)
        {
            var list = new List<IUnionExpression>();
            foreach (var s in otherselects)
            {
                list.Add(new UnionExpression(Operator.UnionAll, s));
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
        public static ISelectStatement GB(this ISelectStatement select, IColumn column)
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
            return GroupBy(select, new Column(column));
        }

        public static ISelectStatement GroupBy(this ISelectStatement select, IColumn column)
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
