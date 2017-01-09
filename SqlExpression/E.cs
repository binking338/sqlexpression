using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    /// <summary>
    /// SqlExpression Entry Class
    /// Sql表达式入口类
    /// </summary>
    public static class E
    {
        #region ShortCut

        public static TableExpression T(string table)
        {
            return Table(table);
        }

        public static IEnumerable<TableExpression> Ts(IEnumerable<string> tables)
        {
            return Tables(tables);
        }

        public static IEnumerable<TableExpression> Ts(params string[] tables)
        {
            return Tables(tables);
        }

        public static ColumnExpression Col(string column, string table = null)
        {
            return Column(column, table);
        }

        public static LiteralValueExpression Val(object value)
        {
            return Value(value);
        }

        public static LiteralValueExpression V(object value)
        {
            return Val(value);
        }

        public static ParamExpression P(string param)
        {
            return Param(param);
        }

        public static IEnumerable<ParamExpression> Ps(params string[] _params)
        {
            return Params(_params);
        }

        public static CustomerExpression C(string expression)
        {
            return Customer(expression);
        }

        #endregion

        public static ISelectStatement Select()
        {
            return new SelectStatement();
        }

        public static ISelectStatement Select(IEnumerable<ITableExpression> tables)
        {
            return new SelectStatement(tables?.ToArray());
        }

        public static ISelectStatement Select(params TableExpression[] tables)
        {
            return new SelectStatement(tables);
        }

        public static ISelectStatement Select(IEnumerable<ISelectItemExpression> items)
        {
            return new SelectStatement(null, items.ToArray(), null);
        }

        public static ISelectStatement Select(params ISelectItemExpression[] items)
        {
            return Select(items.AsEnumerable());
        }

        public static ISelectStatement Select(params ColumnExpression[] items)
        {
            return Select(items.AsEnumerable());
        }

        public static IInsertStatement Insert(ITableExpression table = null)
        {
            return new InsertStatement(table);
        }

        public static IInsertStatement Insert(TableExpression table)
        {
            return new InsertStatement(table);
        }

        public static IDeleteStatement Delete(ITableExpression table = null)
        {
            return new DeleteStatement(table);
        }

        public static IDeleteStatement Delete(TableExpression table)
        {
            return new DeleteStatement(table);
        }

        public static IUpdateStatement Update(ITableExpression table)
        {
            return new UpdateStatement(table);
        }

        public static IUpdateStatement Update(TableExpression table)
        {
            return new UpdateStatement(table);
        }

        public static TableExpression Table(string table)
        {
            return new TableExpression(table);
        }

        public static IEnumerable<TableExpression> Tables(IEnumerable<string> tables)
        {
            return tables.Select(t => new TableExpression(t));
        }

        public static IEnumerable<TableExpression> Tables(params string[] tables)
        {
            return tables.Select(t => new TableExpression(t));
        }

        public static ColumnExpression Column(string column, string table = null)
        {
            return new ColumnExpression(column, string.IsNullOrWhiteSpace(table) ? null : Table(table));
        }

        public static LiteralValueExpression Value(object value)
        {
            return new LiteralValueExpression(value);
        }

        public static ParamExpression Param(string param)
        {
            return new ParamExpression(param);
        }

        public static IEnumerable<ParamExpression> Params(params string[] _params)
        {
            return _params.Select(p => Param(p));
        }

        public static CollectionExpression Collection(IEnumerable<ILiteralValueExpression> values)
        {
            return new CollectionExpression(values.ToArray());
        }

        public static CollectionExpression Collection(IEnumerable<object> values)
        {
            return new CollectionExpression(values.Select(v => new LiteralValueExpression(v)).ToArray());
        }

        public static CollectionExpression Collection(params ILiteralValueExpression[] values)
        {
            return new CollectionExpression(values);
        }

        public static CollectionExpression Collection(params object[] values)
        {
            return new CollectionExpression(values.Select(v => new LiteralValueExpression(v)).ToArray());
        }

        public static BetweenValueExpression Between(IValueExpression min, IValueExpression max)
        {
            return new BetweenValueExpression(min, max);
        }

        public static BetweenValueExpression Between(object min, object max)
        {
            return new BetweenValueExpression(new LiteralValueExpression(min), new LiteralValueExpression(max));
        }

        public static IFilterExpression And(IEnumerable<IFilterExpression> filters)
        {
            if (filters?.Count() == 0) return null;
            var filter = filters.First();
            for (var i = 1; i < filters.Count(); i++)
            {
                filter = new LogicExpression(filter, Operator.And, filters.ElementAt(i));
            }
            return filter;
        }

        public static IFilterExpression And(params IFilterExpression[] filters)
        {
            return And(filters.AsEnumerable());
        }

        public static IFilterExpression Or(IEnumerable<IFilterExpression> filters)
        {
            if (filters?.Count() == 0) return null;
            var filter = filters.First();
            for (var i = 1; i < filters.Count(); i++)
            {
                filter = new LogicExpression(filter, Operator.Or, filters.ElementAt(i));
            }
            return filter;
        }

        public static IFilterExpression Or(params IFilterExpression[] filters)
        {
            return Or(filters.AsEnumerable());
        }

        public static GroupByClause GroupBy(IColumnExpression column)
        {
            return new GroupByClause(column);
        }

        public static OrderByClause OrderBy(IEnumerable<IOrderExpression> orders)
        {
            return new OrderByClause(orders.ToArray());
        }

        public static OrderByClause OrderBy(params IOrderExpression[] orders)
        {
            return new OrderByClause(orders);
        }

        public static OrderExpression Desc(IColumnExpression column)
        {
            return new OrderExpression(column, OrderEnum.Desc);
        }

        public static OrderExpression Desc(ColumnExpression column)
        {
            return new OrderExpression(column, OrderEnum.Desc);
        }

        public static OrderExpression Asc(IColumnExpression column)
        {
            return new OrderExpression(column);
        }

        public static OrderExpression Asc(ColumnExpression column)
        {
            return new OrderExpression(column);
        }

        public static UnionStatement Union(IEnumerable<ISelectStatement> sqls)
        {
            var list = new List<IUnionItemExpression>();
            foreach (var sql in sqls.Skip(1))
            {
                list.Add(new UnionItemExpression(Operator.Union, sql));
            }
            return new UnionStatement(sqls.First(), list.ToArray(), null);
        }

        public static UnionStatement UnionAll(IEnumerable<ISelectStatement> sqls)
        {
            var list = new List<IUnionItemExpression>();
            foreach (var sql in sqls.Skip(1))
            {
                list.Add(new UnionItemExpression(Operator.UnionAll, sql));
            }
            return new UnionStatement(sqls.First(), list.ToArray(), null);
        }

        public static CustomerExpression Customer(string expression)
        {
            return new CustomerExpression(expression);
        }
    }
}
