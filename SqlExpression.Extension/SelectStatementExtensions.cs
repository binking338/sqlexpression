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

        public static ISelectStatement SelectC(this ISelectStatement select, params IEnumerable<string>[] customs)
        {
            return SelectVarCustom(select, customs);
        }

        public static ISelectStatement SelectC(this ISelectStatement select, params string[] customs)
        {
            return SelectVarCustom(select, customs);
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

        public static ISelectStatement Select(this ISelectStatement select, params IEnumerable<ISelectItemExpression>[] items)
        {
            var query = select.Query;
            if(query is ISimpleQueryStatement)
            {
                Select(query as ISimpleQueryStatement, items);
            }
            return select;
        }

        public static ISelectStatement Select(this ISelectStatement select, params ISelectItemExpression[] items)
        {
            var query = select.Query;
            if (query is ISimpleQueryStatement)
            {
                Select(query as ISimpleQueryStatement, items);
            }
            return select;
        }

        public static ISelectStatement Select(this ISelectStatement select, params IEnumerable<ISimpleValue>[] items)
        {
            var query = select.Query;
            if (query is ISimpleQueryStatement)
            {
                Select(query as ISimpleQueryStatement, items);
            }
            return select;
        }

        public static ISelectStatement Select(this ISelectStatement select, params ISimpleValue[] items)
        {
            var query = select.Query;
            if (query is ISimpleQueryStatement)
            {
                Select(query as ISimpleQueryStatement, items);
            }
            return select;
        }

        public static ISelectStatement SelectVarCustom(this ISelectStatement select, params IEnumerable<string>[] customs)
        {
            var query = select.Query;
            if (query is ISimpleQueryStatement)
            {
                SelectVarCustom(query as ISimpleQueryStatement, customs);
            }
            return select;
        }

        public static ISelectStatement SelectVarCustom(this ISelectStatement select, params string[] customs)
        {
            var query = select.Query;
            if (query is ISimpleQueryStatement)
            {
                SelectVarCustom(query as ISimpleQueryStatement, customs);
            }
            return select;
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

        public static ISimpleQueryStatement Where(this ISimpleQueryStatement query, ISimpleValue filter)
        {
            if (query.Where == null)
            {
                query.Where = new WhereClause(filter);
            }
            else
            {
                query.Where.Filter = new LogicExpression(query.Where.Filter, Operator.And, filter);
            }
            return query;
        }

        public static ISimpleQueryStatement WhereVarCustom(this ISimpleQueryStatement query, string customFilter)
        {
            return Where(query, new CustomExpression(customFilter));
        }

        public static ISimpleQueryStatement OrWhere(this ISimpleQueryStatement query, ISimpleValue filter)
        {
            if (query.Where == null)
            {
                query.Where = new WhereClause(filter);
            }
            else
            {
                query.Where.Filter = new LogicExpression(query.Where.Filter, Operator.Or, filter);
            }
            return query;
        }

        public static ISimpleQueryStatement OrWhereVarCustom(this ISimpleQueryStatement query, string customFilter)
        {
            return OrWhere(query, new CustomExpression(customFilter));
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
        public static ISimpleQueryStatement GB(this IDataset dataset, IEnumerable<IColumn> items)
        {
            return GroupBy(dataset, items);
        }
        public static ISimpleQueryStatement GB(this IDataset dataset, params IColumn[] items)
        {
            return GroupBy(dataset, items);
        }
        public static ISimpleQueryStatement GBC(this IDataset dataset, IEnumerable<string> customs)
        {
            return GroupByCustom(dataset, customs);
        }
        public static ISimpleQueryStatement GBC(this IDataset dataset, params string[] customs)
        {
            return GroupByCustom(dataset, customs);
        }
        public static ISimpleQueryStatement GB(this ITableFilterExpression tableFilter, IEnumerable<IColumn> items)
        {
            return GroupBy(tableFilter, items);
        }
        public static ISimpleQueryStatement GB(this ITableFilterExpression tableFilter, params IColumn[] items)
        {
            return GroupBy(tableFilter, items);
        }
        public static ISimpleQueryStatement GBC(this ITableFilterExpression tableFilter, IEnumerable<string> customs)
        {
            return GroupByCustom(tableFilter, customs);
        }
        public static ISimpleQueryStatement GBC(this ITableFilterExpression tableFilter, params string[] customs)
        {
            return GroupByCustom(tableFilter, customs);
        }
        public static ISimpleQueryStatement GB(this ISimpleQueryStatement query, IEnumerable<IColumn> items)
        {
            return GroupBy(query, items);
        }
        public static ISimpleQueryStatement GB(this ISimpleQueryStatement query, params IColumn[] items)
        {
            return GroupBy(query, items);
        }
        public static ISimpleQueryStatement GBC(this ISimpleQueryStatement query, IEnumerable<string> customs)
        {
            return GroupByCustom(query, customs);
        }
        public static ISimpleQueryStatement GBC(this ISimpleQueryStatement query, params string[] customs)
        {
            return GroupByCustom(query, customs);
        }
        public static ISimpleQueryStatement H(this ISimpleQueryStatement query, ISimpleValue filter)
        {
            return Having(query, filter);
        }
        public static ISimpleQueryStatement HC(this ISimpleQueryStatement query, string customFilter)
        {
            return HavingVarCustom(query, customFilter);
        }
        #endregion

        public static ISimpleQueryStatement GroupBy(this IDataset dataset, IEnumerable<ISimpleValue> items)
        {
            var query = Select(dataset, new List<ISimpleValue>());
            return GroupBy(query, items);
        }

        public static ISimpleQueryStatement GroupBy(this IDataset dataset, params ISimpleValue[] items)
        {
            var query = Select(dataset, new List<ISimpleValue>());
            return GroupBy(query, items.AsEnumerable());
        }

        public static ISimpleQueryStatement GroupByCustom(this IDataset dataset, IEnumerable<string> custom)
        {
            var query = Select(dataset, new List<ISimpleValue>());
            return GroupByCustom(query, custom);
        }

        public static ISimpleQueryStatement GroupByCustom(this IDataset dataset, params string[] custom)
        {
            var query = Select(dataset, new List<ISimpleValue>());
            return GroupByCustom(query, custom.AsEnumerable());
        }

        public static ISimpleQueryStatement GroupBy(this ITableFilterExpression tableFilter, IEnumerable<ISimpleValue> items)
        {
            var query = Select(tableFilter, new List<ISimpleValue>());
            return GroupBy(query, items);
        }

        public static ISimpleQueryStatement GroupBy(this ITableFilterExpression tableFilter, params ISimpleValue[] items)
        {
            var query = Select(tableFilter, new List<ISimpleValue>());
            return GroupBy(query, items.AsEnumerable());
        }

        public static ISimpleQueryStatement GroupByCustom(this ITableFilterExpression tableFilter, IEnumerable<string> custom)
        {
            var query = Select(tableFilter, new List<ISimpleValue>());
            return GroupByCustom(query, custom);
        }

        public static ISimpleQueryStatement GroupByCustom(this ITableFilterExpression tableFilter, params string[] custom)
        {
            var query = Select(tableFilter, new List<ISimpleValue>());
            return GroupByCustom(query, custom.AsEnumerable());
        }

        public static ISimpleQueryStatement GroupBy(this ISimpleQueryStatement query, IEnumerable<ISimpleValue> items)
        {
            query.GroupBy = new GroupByClause(items.ToList());
            return query;
        }

        public static ISimpleQueryStatement GroupBy(this ISimpleQueryStatement query, params ISimpleValue[] items)
        {
            return GroupBy(query, items.AsEnumerable());
        }

        public static ISimpleQueryStatement GroupByCustom(this ISimpleQueryStatement query, IEnumerable<string> customs)
        {
            return GroupBy(query, customs.Select(custom => new CustomExpression(custom) as ISimpleValue));
        }

        public static ISimpleQueryStatement GroupByCustom(this ISimpleQueryStatement query, params string[]
            customs)
        {
            return GroupByCustom(query, customs.AsEnumerable());
        }

        public static ISimpleQueryStatement Having(this ISimpleQueryStatement query, ISimpleValue filter)
        {
            query.GroupBy.Having = filter;
            return query;
        }

        public static ISimpleQueryStatement HavingVarCustom(this ISimpleQueryStatement query, string customFilter)
        {
            return Having(query, new CustomExpression(customFilter));
        }

        #endregion

        #region OrderBy

        #region Shortcut
        public static SelectStatement OB(this IDataset dataset, IOrderByClause orderby)
        {
            return OrderBy(dataset, orderby);
        }
        public static SelectStatement OB(this IDataset dataset, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(dataset, orders);
        }
        public static SelectStatement OB(this IDataset dataset, params IOrderExpression[] orders)
        {
            return OrderBy(dataset, orders);
        }
        public static SelectStatement OB(this ITableFilterExpression tableFilter, IOrderByClause orderby)
        {
            return OrderBy(tableFilter, orderby);
        }
        public static SelectStatement OB(this ITableFilterExpression tableFilter, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(tableFilter, orders);
        }
        public static SelectStatement OB(this ITableFilterExpression tableFilter, params IOrderExpression[] orders)
        {
            return OrderBy(tableFilter, orders);
        }
        public static SelectStatement OB(this ISimpleQueryStatement query, IOrderByClause orderby)
        {
            return OrderBy(query, orderby);
        }
        public static SelectStatement OB(this ISimpleQueryStatement query, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(query, orders);
        }
        public static SelectStatement OB(this ISimpleQueryStatement query, params IOrderExpression[] orders)
        {
            return OrderBy(query, orders);
        }
        public static SelectStatement OB(this IUnionQueryStatement query, IOrderByClause orderby)
        {
            return OrderBy(query, orderby);
        }
        public static SelectStatement OB(this IUnionQueryStatement query, IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(query, orders);
        }
        public static SelectStatement OB(this IUnionQueryStatement query, params IOrderExpression[] orders)
        {
            return OrderBy(query, orders);
        }
        #endregion

        public static SelectStatement OrderBy(this IDataset dataset, IOrderByClause orderby)
        {
            var query = Select(dataset, new List<ISelectItemExpression>());
            return OrderBy(query, orderby);
        }

        public static SelectStatement OrderBy(this IDataset dataset, IEnumerable<IOrderExpression> orders)
        {
            var query = Select(dataset, new List<ISelectItemExpression>());
            return OrderBy(query, orders);
        }

        public static SelectStatement OrderBy(this IDataset dataset, params IOrderExpression[] orders)
        {
            var query = Select(dataset, new List<ISelectItemExpression>());
            return OrderBy(query, orders);
        }

        public static SelectStatement OrderBy(this ITableFilterExpression tableFilter, IOrderByClause orderby)
        {
            var query = Select(tableFilter, new List<ISelectItemExpression>());
            return OrderBy(query, orderby);
        }

        public static SelectStatement OrderBy(this ITableFilterExpression tableFilter, IEnumerable<IOrderExpression> orders)
        {
            var query = Select(tableFilter, new List<ISelectItemExpression>());
            return OrderBy(query, orders);
        }

        public static SelectStatement OrderBy(this ITableFilterExpression tableFilter, params IOrderExpression[] orders)
        {
            var query = Select(tableFilter, new List<ISelectItemExpression>());
            return OrderBy(query, orders);
        }

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