using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression.Extension
{
    public static class SelectStatementExtensions
    {
        #region Select

        #region ShortCut

        public static SimpleQueryStatement SelectC(this ITableFilterExpression tableFilter, params IEnumerable<string>[] customs)
        {
            return SelectVarCustom(tableFilter, customs);
        }

        public static SimpleQueryStatement SelectC(this ITableFilterExpression tableFilter, params string[] customs)
        {
            return SelectVarCustom(tableFilter, customs);
        }

        public static SimpleQueryStatement SelectC(this IDataset dataset, params IEnumerable<string>[] customs)
        {
            return SelectVarCustom(dataset, customs);
        }

        public static SimpleQueryStatement SelectC(this IDataset dataset, params string[] customs)
        {
            return SelectVarCustom(dataset, customs);
        }

        public static ISimpleQueryStatement SelectC(this ISimpleQueryStatement query, params IEnumerable<string>[] customs)
        {
            return SelectVarCustom(query, customs);
        }

        public static ISimpleQueryStatement SelectC(this ISimpleQueryStatement query, params string[] customs)
        {
            return SelectVarCustom(query, customs);
        }

        #endregion

        public static SimpleQueryStatement Select(this ITableFilterExpression tableFilter, params IEnumerable<ISelectItemExpression>[] items)
        {
            var flatenItems = items.Aggregate((IEnumerable<ISelectItemExpression> a, IEnumerable<ISelectItemExpression> b) => a.Concat(b));
            return new SimpleQueryStatement(new SelectClause(flatenItems.ToList()), new FromClause(tableFilter.Table), tableFilter.Where);
        }

        public static SimpleQueryStatement Select(this ITableFilterExpression tableFilter, params ISelectItemExpression[] items)
        {
            return Select(tableFilter, items.Cast<ISelectItemExpression>());
        }

        public static SimpleQueryStatement Select(this ITableFilterExpression tableFilter, params IEnumerable<ISimpleValue>[] items)
        {
            var flatenItems = items.Aggregate((IEnumerable<ISimpleValue> a, IEnumerable<ISimpleValue> b) => a.Concat(b));
            return Select(tableFilter, flatenItems.Select(item => new SelectItemExpression(item, null) as ISelectItemExpression));
        }

        public static SimpleQueryStatement Select(this ITableFilterExpression tableFilter, params ISimpleValue[] items)
        {
            return Select(tableFilter, items.AsEnumerable());
        }

        public static SimpleQueryStatement SelectVarCustom(this ITableFilterExpression tableFilter, params IEnumerable<string>[] customs)
        {
            var flatenItems = customs.Aggregate((IEnumerable<string> a, IEnumerable<string> b) => a.Concat(b));
            return Select(tableFilter, flatenItems.Select(c => new SelectItemExpression(new CustomExpression(c), null) as ISelectItemExpression));
        }

        public static SimpleQueryStatement SelectVarCustom(this ITableFilterExpression tableFilter, params string[] customs)
        {
            return SelectVarCustom(tableFilter, customs.AsEnumerable());
        }

        public static SimpleQueryStatement Select(this IDataset dataset, params IEnumerable<ISelectItemExpression>[] items)
        {
            var flatenItems = items.Aggregate((IEnumerable<ISelectItemExpression> a, IEnumerable<ISelectItemExpression> b) => a.Concat(b));
            return new SimpleQueryStatement(new SelectClause(flatenItems.ToList()), new FromClause(dataset));
        }

        public static SimpleQueryStatement Select(this IDataset dataset, params ISelectItemExpression[] items)
        {
            return Select(dataset, items.Cast<ISelectItemExpression>());
        }

        public static SimpleQueryStatement Select(this IDataset dataset, params IEnumerable<ISimpleValue>[] items)
        {
            var flatenItems = items.Aggregate((IEnumerable<ISimpleValue> a, IEnumerable<ISimpleValue> b) => a.Concat(b));
            return Select(dataset, flatenItems.Select(item => new SelectItemExpression(item, null) as ISelectItemExpression));
        }

        public static SimpleQueryStatement Select(this IDataset dataset, params ISimpleValue[] items)
        {
            return Select(dataset, items.AsEnumerable());
        }

        public static SimpleQueryStatement SelectVarCustom(this IDataset dataset, params IEnumerable<string>[] customs)
        {
            var flatenItems = customs.Aggregate((IEnumerable<string> a, IEnumerable<string> b) => a.Concat(b));
            return Select(dataset, flatenItems.Select(c => new SelectItemExpression(new CustomExpression(c), null) as ISelectItemExpression));
        }

        public static SimpleQueryStatement SelectVarCustom(this IDataset dataset, params string[] customs)
        {
            return SelectVarCustom(dataset, customs.AsEnumerable());
        }

        public static ISimpleQueryStatement Select(this ISimpleQueryStatement query, params IEnumerable<ISelectItemExpression>[] items)
        {
            var flatenItems = items.Aggregate((IEnumerable<ISelectItemExpression> a, IEnumerable<ISelectItemExpression> b) => a.Concat(b));
            query.Select = new SelectClause(flatenItems.ToList());
            return query;
        }

        public static ISimpleQueryStatement Select(this ISimpleQueryStatement query, params ISelectItemExpression[] items)
        {
            return Select(query, items.Cast<ISelectItemExpression>());
        }

        public static ISimpleQueryStatement Select(this ISimpleQueryStatement query, params IEnumerable<ISimpleValue>[] items)
        {
            var flatenItems = items.Aggregate((IEnumerable<ISimpleValue> a, IEnumerable<ISimpleValue> b) => a.Concat(b));
            return Select(query, flatenItems.Select(item => new SelectItemExpression(item, null) as ISelectItemExpression));
        }

        public static ISimpleQueryStatement Select(this ISimpleQueryStatement query, params ISimpleValue[] items)
        {
            return Select(query, items.AsEnumerable());
        }

        public static ISimpleQueryStatement SelectVarCustom(this ISimpleQueryStatement query, params IEnumerable<string>[] customs)
        {
            var flatenCustoms = customs.Aggregate((IEnumerable<string> a, IEnumerable<string> b) => a.Concat(b));
            return Select(query, flatenCustoms.Select(c => new SelectItemExpression(new CustomExpression(c), null) as ISelectItemExpression));
        }

        public static ISimpleQueryStatement SelectVarCustom(this ISimpleQueryStatement query, params string[] customs)
        {
            return SelectVarCustom(query, customs.AsEnumerable());
        }

        #endregion

        #region Join

        #region Shortcut
        public static JoinExpression J(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return Join(left, right, on);
        }
        public static JoinExpression IJ(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return InnerJoin(left, right, on);
        }
        public static JoinExpression LJ(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return LeftJoin(left, right, on);
        }
        public static JoinExpression RJ(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return RightJoin(left, right, on);
        }
        public static JoinExpression FJ(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return FullJoin(left, right, on);
        }
        #endregion

        public static JoinExpression Join(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return new JoinExpression(left, Operator.Join, right, on);
        }

        public static JoinExpression InnerJoin(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return new JoinExpression(left, Operator.InnerJoin, right, on);
        }

        public static JoinExpression LeftJoin(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return new JoinExpression(left, Operator.LeftJoin, right, on);
        }

        public static JoinExpression RightJoin(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return new JoinExpression(left, Operator.RightJoin, right, on);
        }

        public static JoinExpression FullJoin(this IDataset left, IDataset right, ISimpleValue on = null)
        {
            return new JoinExpression(left, Operator.FullJoin, right, on);
        }

        public static IJoinExpression On(this IJoinExpression join, ISimpleValue condition)
        {
            join.On = condition;
            return join;
        }

        #endregion

        #region Where

        #region Shortcut
        public static SimpleQueryStatement WhereC(this IDataset dataset, string customFilter)
        {
            return WhereVarCustom(dataset, customFilter);
        }
        public static ISimpleQueryStatement WhereC(this ISimpleQueryStatement select, string customFilter)
        {
            return WhereVarCustom(select, customFilter);
        }
        public static ISimpleQueryStatement OrWhereC(this ISimpleQueryStatement select, string customFilter)
        {
            return OrWhereVarCustom(select, customFilter);
        }
        #endregion

        public static SimpleQueryStatement Where(this IDataset dataset, ISimpleValue filter)
        {
            return new SimpleQueryStatement(null, new FromClause(dataset), new WhereClause(filter));
        }

        public static SimpleQueryStatement WhereVarCustom(this IDataset dataset, string customFilter)
        {
            return Where(dataset, new CustomExpression(customFilter));
        }

        public static ISimpleQueryStatement Where(this ISimpleQueryStatement select, ISimpleValue filter)
        {
            if (select.Where == null)
            {
                select.Where = new WhereClause(filter);
            }
            else
            {
                select.Where.Filter = new LogicExpression(select.Where.Filter, Operator.And, filter);
            }
            return select;
        }

        public static ISimpleQueryStatement WhereVarCustom(this ISimpleQueryStatement select, string customFilter)
        {
            return Where(select, new CustomExpression(customFilter));
        }

        public static ISimpleQueryStatement OrWhere(this ISimpleQueryStatement select, ISimpleValue filter)
        {
            if (select.Where == null)
            {
                select.Where = new WhereClause(filter);
            }
            else
            {
                select.Where.Filter = new LogicExpression(select.Where.Filter, Operator.Or, filter);
            }
            return select;
        }

        public static ISimpleQueryStatement OrWhereVarCustom(this ISimpleQueryStatement select, string customFilter)
        {
            return OrWhere(select, new CustomExpression(customFilter));
        }

        #endregion

        #region Union

        #region Shortcut
        public static UnionQueryStatement U(this IQueryStatement query, ISimpleQueryStatement otherQuery)
        {
            return Union(query, otherQuery);
        }
        public static UnionQueryStatement UD(this IQueryStatement query, ISimpleQueryStatement otherQuery)
        {
            return UnionDistinct(query, otherQuery);
        }
        public static UnionQueryStatement UA(this IQueryStatement query, ISimpleQueryStatement otherQuery)
        {
            return UnionAll(query, otherQuery);
        }
        #endregion

        public static UnionQueryStatement Union(this IQueryStatement query, ISimpleQueryStatement otherQuery)
        {
            return new UnionQueryStatement(query, Operator.Union, otherQuery);
        }

        public static UnionQueryStatement UnionDistinct(this IQueryStatement query, ISimpleQueryStatement otherQuery)
        {
            return new UnionQueryStatement(query, Operator.UnionDistinct, otherQuery);
        }

        public static UnionQueryStatement UnionAll(this IQueryStatement query, ISimpleQueryStatement otherQuery)
        {
            return new UnionQueryStatement(query, Operator.UnionAll, otherQuery);
        }

        #endregion

        #region GroupBy

        #region Shortcut
        public static ISimpleQueryStatement GB(this ISimpleQueryStatement select, IEnumerable<IColumn> columns)
        {
            return GroupBy(select, columns);
        }
        public static ISimpleQueryStatement GB(this ISimpleQueryStatement select, params IColumn[] columns)
        {
            return GroupBy(select, columns);
        }
        public static ISimpleQueryStatement GB(this ISimpleQueryStatement select, IEnumerable<string> columns)
        {
            return GroupBy(select, columns);
        }
        public static ISimpleQueryStatement GB(this ISimpleQueryStatement select, params string[] columns)
        {
            return GroupBy(select, columns);
        }
        public static ISimpleQueryStatement H(this ISimpleQueryStatement select, ISimpleValue filter)
        {
            return Having(select, filter);
        }
        public static ISimpleQueryStatement HC(this ISimpleQueryStatement select, string customFilter)
        {
            return HavingVarCustom(select, customFilter);
        }
        #endregion

        public static ISimpleQueryStatement GroupBy(this ISimpleQueryStatement select, IEnumerable<IColumn> columns)
        {
            select.GroupBy = new GroupByClause(columns.Cast<ISimpleValue>().ToList());
            return select;
        }

        public static ISimpleQueryStatement GroupBy(this ISimpleQueryStatement select, params IColumn[] columns)
        {
            return GroupBy(select, columns.AsEnumerable());
        }

        public static ISimpleQueryStatement GroupBy(this ISimpleQueryStatement select, IEnumerable<string> columns)
        {
            return GroupBy(select, columns.Select(column => new Column(column) as IColumn));
        }

        public static ISimpleQueryStatement GroupBy(this ISimpleQueryStatement select, params string[]
            columns)
        {
            return GroupBy(select, columns.AsEnumerable());
        }

        public static ISimpleQueryStatement Having(this ISimpleQueryStatement select, ISimpleValue filter)
        {
            select.GroupBy.Having = filter;
            return select;
        }

        public static ISimpleQueryStatement HavingVarCustom(this ISimpleQueryStatement select, string customFilter)
        {
            return Having(select, new CustomExpression(customFilter));
        }

        #endregion

        #region OrderBy

        #region Shortcut
        public static SelectStatement OB(this ISimpleQueryStatement select, IOrderByClause orderby)
        {
            return OrderBy(select, orderby);
        }
        public static SelectStatement OB(this ISimpleQueryStatement select, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(select, orders);
        }
        public static SelectStatement OB(this ISimpleQueryStatement select, params IOrderExpression[] orders)
        {
            return OrderBy(select, orders);
        }
        public static SelectStatement OB(this IUnionQueryStatement select, IOrderByClause orderby)
        {
            return OrderBy(select, orderby);
        }
        public static SelectStatement OB(this IUnionQueryStatement select, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(select, orders);
        }
        public static SelectStatement OB(this IUnionQueryStatement select, params IOrderExpression[] orders)
        {
            return OrderBy(select, orders);
        }
        #endregion

        public static SelectStatement OrderBy(this ISimpleQueryStatement query, IOrderByClause orderby)
        {
            var select = new SelectStatement(query);
            select.OrderBy = orderby;
            return select;
        }

        public static SelectStatement OrderBy(this ISimpleQueryStatement query, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(query, orderby);
        }

        public static SelectStatement OrderBy(this ISimpleQueryStatement query, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(query, orderby);
        }

        public static SelectStatement OrderBy(this IUnionQueryStatement query, IOrderByClause orderby)
        {
            var select = new SelectStatement(query);
            select.OrderBy = orderby;
            return select;
        }

        public static SelectStatement OrderBy(this IUnionQueryStatement query, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(query, orderby);
        }

        public static SelectStatement OrderBy(this IUnionQueryStatement query, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(query, orderby);
        }

        #endregion

        #region Exists

        public static ExistsExpression Exists(this ISelectStatement select)
        {
            return new ExistsExpression(new SubQueryExpression(select.Query));
        }

        public static ExistsExpression Exists(this IQueryStatement query)
        {
            return new ExistsExpression(new SubQueryExpression(query));
        }

        public static NotExistsExpression NotExists(this ISelectStatement select)
        {
            return new NotExistsExpression(new SubQueryExpression(select.Query));
        }

        public static NotExistsExpression NotExists(this IQueryStatement query)
        {
            return new NotExistsExpression(new SubQueryExpression(query));
        }

        #endregion

        #region SQL

        /// <summary>
        /// 返回计数sql语句
        /// SELECT COUNT(*) AS __totalcount__ FROM ({0})
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static SimpleQueryStatement ToCountSql(this ISelectStatement select)
        {
            return ToCountSql(select.Query);
        }

        /// <summary>
        /// 返回计数sql语句
        /// SELECT COUNT(*) AS __totalcount__ FROM ({0})
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static SimpleQueryStatement ToCountSql(this IQueryStatement query)
        {
            return new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { AggregateFunctionExpression.Count(new AllColumns(null)).As("__totalcount__") }),
                                            new FromClause(new SubQueryExpression(query)));
        }

        /// <summary>
        /// 判断是否存在sql语句
        /// SELECT EXISTS({0}) AS __exists__
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static SimpleQueryStatement ToExistsSql(this ISelectStatement select)
        {
            return new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { (new ExistsExpression(new SubQueryExpression(select.Query))).As("__exists__") }));
        }

        /// <summary>
        /// 判断是否存在sql语句
        /// SELECT EXISTS({0}) AS __exists__
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static SimpleQueryStatement ToExistsSql(this IQueryStatement query)
        {
            return new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { (new ExistsExpression(new SubQueryExpression(query))).As("__exists__") }));
        }

        /// <summary>
        /// 判断是否存在sql语句
        /// SELECT NOT EXISTS({0}) AS __notexists__
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static SimpleQueryStatement ToNotExistsSql(this ISelectStatement select)
        {
            return new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { (new NotExistsExpression(new SubQueryExpression(select.Query))).As("__notexists__") }));
        }

        /// <summary>
        /// 判断是否存在sql语句
        /// SELECT NOT EXISTS({0}) AS __notexists__
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static SimpleQueryStatement ToNotExistsSql(this IQueryStatement query)
        {
            return new SimpleQueryStatement(new SelectClause(new List<ISelectItemExpression>() { (new NotExistsExpression(new SubQueryExpression(query))).As("__notexists__") }));
        }

        #endregion
    }
}