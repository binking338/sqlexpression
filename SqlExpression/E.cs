﻿using System;
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

        public static PropertyExpression Prop(string property, string table = null)
        {
            return Property(property, table);
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

        public static  CollectionExpression Cols(IEnumerable<ILiteralValueExpression> values)
        {
            return Collection(values);
        }

        public static CollectionExpression Cols(IEnumerable<object> values)
        {
            return Collection(values);
        }

        public static CollectionExpression Cols(params ILiteralValueExpression[] values)
        {
            return Collection(values);
        }

        public static CollectionExpression Cols(params object[] values)
        {
            return Collection(values);
        }

        public static CustomerExpression C(string expression)
        {
            return Customer(expression);
        }

        #endregion

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

        public static PropertyExpression Property(string property, string table = null)
        {
            return new PropertyExpression(property, string.IsNullOrWhiteSpace(table) ? null : Table(table));
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

        public static GroupByClause GroupBy(IPropertyExpression property)
        {
            return new GroupByClause(property);
        }

        public static OrderByClause OrderBy(IEnumerable<IOrderExpression> orders)
        {
            return new OrderByClause(orders.ToArray());
        }

        public static OrderByClause OrderBy(params IOrderExpression[] orders)
        {
            return new OrderByClause(orders);
        }

        public static OrderExpression Desc(IPropertyExpression property)
        {
            return new OrderExpression(property, OrderEnum.Desc);
        }

        public static OrderExpression Desc(PropertyExpression property)
        {
            return new OrderExpression(property, OrderEnum.Desc);
        }

        public static OrderExpression Asc(IPropertyExpression property)
        {
            return new OrderExpression(property);
        }

        public static OrderExpression Asc(PropertyExpression property)
        {
            return new OrderExpression(property);
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
