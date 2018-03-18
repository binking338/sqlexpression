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

        public static Table T(string table)
        {
            return Table(table);
        }

        public static IEnumerable<Table> Ts(IEnumerable<string> tables)
        {
            return Tables(tables);
        }

        public static IEnumerable<Table> Ts(params string[] tables)
        {
            return Tables(tables);
        }

        public static Column Col(string column, string table = null)
        {
            return Column(column, table);
        }

        public static LiteralValue Val(object value)
        {
            return Value(value);
        }

        public static LiteralValue V(object value)
        {
            return Val(value);
        }

        public static Param P(string param)
        {
            return Param(param);
        }

        public static IEnumerable<Param> Ps(params string[] _params)
        {
            return Params(_params);
        }

        public static GroupByClause GB(IColumn column)
        {
            return GroupBy(column);
        }

        public static OrderByClause OB(IEnumerable<IOrderExpression> orders)
        {
            return OrderBy(orders);
        }

        public static OrderByClause OB(params IOrderExpression[] orders)
        {
            return OrderBy(orders);
        }

        public static CustomerExpression Cus(string expression)
        {
            return Customer(expression);
        }

        public static CustomerExpression C(string expression)
        {
            return Customer(expression);
        }

        #endregion

        public static ISelectStatement Select(IEnumerable<ISelectFieldExpression> items)
        {
            return new SelectStatement(null, items.ToArray(), null);
        }

        public static ISelectStatement Select(params ISelectFieldExpression[] items)
        {
            return Select(items.AsEnumerable());
        }

        public static ISelectStatement Select(params IColumn[] items)
        {
            return Select(items.Select(item => new SelectFieldExpression(item)));
        }

        public static ISelectStatement From(IEnumerable<ITable> tables)
        {
            return new SelectStatement(tables?.ToArray());
        }

        public static ISelectStatement From(params ITable[] tables)
        {
            return new SelectStatement(tables);
        }

        public static IInsertStatement Insert(ITable table = null)
        {
            return new InsertStatement(table);
        }

        public static IDeleteStatement Delete(ITable table = null)
        {
            return new DeleteStatement(table);
        }

        public static IUpdateStatement Update(ITable table)
        {
            return new UpdateStatement(table);
        }

        public static Table Table(string table)
        {
            return new Table(table);
        }

        public static IEnumerable<Table> Tables(IEnumerable<string> tables)
        {
            return tables.Select(t => new Table(t));
        }

        public static IEnumerable<Table> Tables(params string[] tables)
        {
            return tables.Select(t => new Table(t));
        }

        public static Column Column(string column, string table = null)
        {
            return new Column(column, string.IsNullOrWhiteSpace(table) ? null : Table(table));
        }

        public static LiteralValue Value(object value)
        {
            return new LiteralValue(value);
        }

        public static Param Param(string param)
        {
            return new Param(param);
        }

        public static IEnumerable<Param> Params(params string[] _params)
        {
            return _params.Select(p => Param(p));
        }

        public static ValueCollectionExpression Collection(IEnumerable<ILiteralValue> values)
        {
            return new ValueCollectionExpression(values.ToArray());
        }

        public static ValueCollectionExpression Collection(IEnumerable<object> values)
        {
            return new ValueCollectionExpression(values.Select(v => new LiteralValue(v)).ToArray());
        }

        public static ValueCollectionExpression Collection(params ILiteralValue[] values)
        {
            return new ValueCollectionExpression(values);
        }

        public static ValueCollectionExpression Collection(params object[] values)
        {
            return new ValueCollectionExpression(values.Select(v => new LiteralValue(v)).ToArray());
        }

        public static IValue And(IEnumerable<IValue> filters)
        {
            if (filters?.Count() == 0) return new LiteralValue(true);
            var filter = filters.First();
            for (var i = 1; i < filters.Count(); i++)
            {
                filter = new LogicExpression(filter, Operator.And, filters.ElementAt(i));
            }
            return filter;
        }

        public static IValue And(params IValue[] filters)
        {
            return And(filters.AsEnumerable());
        }

        public static IValue Or(IEnumerable<IValue> filters)
        {
            if (filters?.Count() == 0) return new LiteralValue(true);
            var filter = filters.First();
            for (var i = 1; i < filters.Count(); i++)
            {
                filter = new LogicExpression(filter, Operator.Or, filters.ElementAt(i));
            }
            return filter;
        }

        public static IValue Or(params IValue[] filters)
        {
            return Or(filters.AsEnumerable());
        }

        public static GroupByClause GroupBy(params IColumn[] columns)
        {
            return new GroupByClause(columns);
        }

        public static OrderByClause OrderBy(IEnumerable<IOrderExpression> orders)
        {
            return new OrderByClause(orders.ToArray());
        }

        public static OrderByClause OrderBy(params IOrderExpression[] orders)
        {
            return new OrderByClause(orders);
        }

        public static OrderExpression Desc(IColumn column)
        {
            return new OrderExpression(column, OrderEnum.Desc);
        }

        public static OrderExpression Desc(Column column)
        {
            return new OrderExpression(column, OrderEnum.Desc);
        }

        public static OrderExpression Asc(IColumn column)
        {
            return new OrderExpression(column);
        }

        public static OrderExpression Asc(Column column)
        {
            return new OrderExpression(column);
        }

        public static UnionStatement Union(IEnumerable<ISelectStatement> sqls)
        {
            var list = new List<IUnionExpression>();
            foreach (var sql in sqls.Skip(1))
            {
                list.Add(new UnionExpression(Operator.Union, sql));
            }
            return new UnionStatement(sqls.First(), list.ToArray(), null);
        }

        public static UnionStatement UnionAll(IEnumerable<ISelectStatement> sqls)
        {
            var list = new List<IUnionExpression>();
            foreach (var sql in sqls.Skip(1))
            {
                list.Add(new UnionExpression(Operator.UnionAll, sql));
            }
            return new UnionStatement(sqls.First(), list.ToArray(), null);
        }

        public static CustomerExpression Customer(string expression)
        {
            return new CustomerExpression(expression);
        }
    }
}
