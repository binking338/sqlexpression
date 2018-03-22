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

        public static SelectStatement SelectFrom(IEnumerable<ITableAliasExpression> tables)
        {
            if(tables == null){
                throw new ArgumentNullException(nameof(tables));
            }
            if (tables.Count() == 0)
            {
                throw new ArgumentException(nameof(tables));
            }
            return new SelectStatement(tables.ToArray());
        }

        public static SelectStatement SelectFrom(params ITableAliasExpression[] tables)
        {
            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables));
            }
            if (tables.Length == 0)
            {
                throw new ArgumentException(nameof(tables));
            }
            return SelectFrom(tables.AsEnumerable());
        }

        public static SelectStatement SelectFrom(params ITable[] tables)
        {
            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables));
            }
            if (tables.Length == 0)
            {
                throw new ArgumentException(nameof(tables));
            }
            return SelectFrom(tables.Select(table => new TableAliasExpression(table, null)));
        }

        public static InsertStatement Insert(ITable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return new InsertStatement(table);
        }

        public static InsertStatement Insert(string table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return Insert(Table(table));
        }

        public static DeleteStatement Delete(ITable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return new DeleteStatement(table);
        }

        public static DeleteStatement Delete(string table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return Delete(Table(table));
        }

        public static UpdateStatement Update(ITableAliasExpression table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return new UpdateStatement(table);
        }

        public static UpdateStatement Update(ITable table, ITableAlias alias = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return Update(TableAlias(table, alias));
        }

        public static UpdateStatement Update(ITable table, string alias)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return Update(TableAlias(table, alias));
        }

        public static UpdateStatement Update(string table, ITableAlias alias = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return Update(TableAlias(table, alias));
        }

        public static UpdateStatement Update(string table, string alias)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return Update(TableAlias(table, alias));
        }

        public static Table Table(string table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return new Table(table);
        }

        public static TableAliasExpression TableAlias(ITable table, ITableAlias alias = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return new TableAliasExpression(table, alias);
        }

        public static TableAliasExpression TableAlias(ITable table, string alias)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return TableAlias(table, string.IsNullOrWhiteSpace(alias) ? null : new TableAlias(alias));
        }

        public static TableAliasExpression TableAlias(string table, ITableAlias alias = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return TableAlias(Table(table), alias);
        }

        public static TableAliasExpression TableAlias(string table, string alias)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            return TableAlias(Table(table), string.IsNullOrWhiteSpace(alias) ? null : new TableAlias(alias));
        }

        public static IEnumerable<Table> Tables(IEnumerable<string> tables)
        {
            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables));
            }
            if (tables.Count() == 0)
            {
                throw new ArgumentException(nameof(tables));
            }
            return tables.Select(t => new Table(t));
        }

        public static IEnumerable<Table> Tables(params string[] tables)
        {
            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables));
            }
            if (tables.Length == 0)
            {
                throw new ArgumentException(nameof(tables));
            }
            return tables.Select(t => new Table(t));
        }

        public static Column Column(string column, string table = null)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }
            return new Column(column, string.IsNullOrWhiteSpace(table) ? null : Table(table));
        }

        public static LiteralValue Value(object value)
        {
            return new LiteralValue(value);
        }

        public static Param Param(string param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            return new Param(param);
        }

        public static IEnumerable<Param> Params(IEnumerable<string> _params)
        {
            if (_params == null)
            {
                throw new ArgumentNullException(nameof(_params));
            }
            if (_params.Count() == 0)
            {
                throw new ArgumentException(nameof(_params));
            }
            return _params.Select(p => Param(p));
        }

        public static IEnumerable<Param> Params(params string[] _params)
        {
            if (_params == null)
            {
                throw new ArgumentNullException(nameof(_params));
            }
            if (_params.Length == 0)
            {
                throw new ArgumentException(nameof(_params));
            }
            return _params.Select(p => Param(p));
        }

        public static ValueCollectionExpression Collection(IEnumerable<ILiteralValue> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (values.Count() == 0)
            {
                throw new ArgumentException(nameof(values));
            }
            return new ValueCollectionExpression(values.ToArray());
        }

        public static ValueCollectionExpression Collection(IEnumerable<object> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (values.Count() == 0)
            {
                throw new ArgumentException(nameof(values));
            }
            return new ValueCollectionExpression(values.Select(v => new LiteralValue(v)).ToArray());
        }

        public static ValueCollectionExpression Collection(params ILiteralValue[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (values.Length == 0)
            {
                throw new ArgumentException(nameof(values));
            }
            return new ValueCollectionExpression(values);
        }

        public static ValueCollectionExpression Collection(params object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (values.Length == 0)
            {
                throw new ArgumentException(nameof(values));
            }
            return new ValueCollectionExpression(values.Select(v => new LiteralValue(v)).ToArray());
        }

        public static IValue And(IEnumerable<IValue> filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }
            if (filters.Count() == 0)
            {
                throw new ArgumentException(nameof(filters));
            }
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
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }
            if (filters.Length == 0)
            {
                throw new ArgumentException(nameof(filters));
            }
            return And(filters.AsEnumerable());
        }

        public static IValue Or(IEnumerable<IValue> filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }
            if (filters.Count() == 0)
            {
                throw new ArgumentException(nameof(filters));
            }
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
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }
            if (filters.Length == 0)
            {
                throw new ArgumentException(nameof(filters));
            }
            return Or(filters.AsEnumerable());
        }

        public static GroupByClause GroupBy(IEnumerable<IValue> columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }
            if (columns.Count() == 0)
            {
                throw new ArgumentException(nameof(columns));
            }
            return new GroupByClause(columns.ToArray());
        }

        public static GroupByClause GroupBy(params IValue[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }
            if (columns.Length == 0)
            {
                throw new ArgumentException(nameof(columns));
            }
            return new GroupByClause(columns);
        }

        public static OrderByClause OrderBy(IEnumerable<IOrderExpression> orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }
            if (orders.Count() == 0)
            {
                throw new ArgumentException(nameof(orders));
            }
            return new OrderByClause(orders.ToArray());
        }

        public static OrderByClause OrderBy(params IOrderExpression[] orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }
            if (orders.Length == 0)
            {
                throw new ArgumentException(nameof(orders));
            }
            return new OrderByClause(orders);
        }

        public static OrderExpression Desc(IColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }
            return new OrderExpression(column, OrderEnum.Desc);
        }

        public static OrderExpression Asc(IColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }
            return new OrderExpression(column);
        }

        public static UnionStatement Union(IEnumerable<ISelectStatement> sqls)
        {
            if (sqls == null)
            {
                throw new ArgumentNullException(nameof(sqls));
            }
            if (sqls.Count() == 0)
            {
                throw new ArgumentException(nameof(sqls));
            }
            var list = new List<IUnionExpression>();
            foreach (var sql in sqls.Skip(1))
            {
                list.Add(new UnionExpression(Operator.Union, sql));
            }
            return new UnionStatement(sqls.First(), list.ToArray(), null);
        }

        public static UnionStatement Union(params ISelectStatement[] sqls)
        {
            if (sqls == null)
            {
                throw new ArgumentNullException(nameof(sqls));
            }
            if (sqls.Length == 0)
            {
                throw new ArgumentException(nameof(sqls));
            }
            return Union(sqls.AsEnumerable());
        }

        public static UnionStatement UnionAll(IEnumerable<ISelectStatement> sqls)
        {
            if (sqls == null)
            {
                throw new ArgumentNullException(nameof(sqls));
            }
            if (sqls.Count() == 0)
            {
                throw new ArgumentException(nameof(sqls));
            }
            var list = new List<IUnionExpression>();
            foreach (var sql in sqls.Skip(1))
            {
                list.Add(new UnionExpression(Operator.UnionAll, sql));
            }
            return new UnionStatement(sqls.First(), list.ToArray(), null);
        }

        public static UnionStatement UnionAll(params ISelectStatement[] sqls)
        {
            if (sqls == null)
            {
                throw new ArgumentNullException(nameof(sqls));
            }
            if(sqls.Length == 0){
                throw new ArgumentException(nameof(sqls));
            }
            return UnionAll(sqls.AsEnumerable());
        }

        public static CustomerExpression Customer(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }
            return new CustomerExpression(expression);
        }
    }
}
