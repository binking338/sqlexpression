using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsExpression
    {
        public static IBatchSqlStatement Concat(this ISqlStatement sql, params ISqlStatement[] otherSqls)
        {
            List<ISqlStatement> sqls = new List<ISqlStatement>();
            sqls.Add(sql);
            sqls.AddRange(otherSqls);
            return new BatchSqlStatement(sqls.ToArray());
        }

        #region Property

        #region ShortCut

        public static IParamExpression ToP(this IPropertyExpression property, string param = null)
        {
            return ToParam(property, param);
        }

        public static ISetItemExpression SetC(this IPropertyExpression property, string customer)
        {
            return SetVarCustomer(property, customer);
        }

        public static ISetItemExpression SetP(this IPropertyExpression property, string param = null)
        {
            return SetVarParam(property, param);
        }

        #endregion

        public static IParamExpression ToParam(this IPropertyExpression property, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = property.Name;
            return new ParamExpression(param);
        }

        public static ISetItemExpression SetVarCustomer(this IPropertyExpression property, string customer)
        {
            return Set(property, new CustomerExpression(customer));
        }

        public static ISetItemExpression SetVarParam(this IPropertyExpression property, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = property.Name;
            return Set(property, property.ToParam(param));
        }

        public static ISetItemExpression Set(this IPropertyExpression property, object value = null)
        {
            return Set(property, value is IValueExpression ? value as IValueExpression : new LiteralValueExpression(value));
        }

        public static ISetItemExpression Set(this IPropertyExpression property, IValueExpression value)
        {
            return new SetItemExpression(property, value);
        }

        public static IAsExpression As(this IPropertyExpression property, string asName)
        {
            return new AsExpression(property, new PropertyExpression(asName));
        }

        public static IOrderExpression Desc(this IPropertyExpression prop)
        {
            return new OrderExpression(prop, OrderEnum.Desc);
        }

        public static IOrderExpression Asc(this IPropertyExpression prop)
        {
            return new OrderExpression(prop, OrderEnum.Asc);
        }

        #region 聚合函数 AggregateFunctionExpression

        public static AggregateFunctionExpression Sum(this IPropertyExpression property)
        {
            return FunctionExpression.Sum(property);
        }

        public static AggregateFunctionExpression Count(this IPropertyExpression property)
        {
            return FunctionExpression.Count(property);
        }

        public static AggregateFunctionExpression Avg(this IPropertyExpression property)
        {
            return FunctionExpression.Avg(property);
        }

        public static AggregateFunctionExpression Min(this IPropertyExpression property)
        {
            return FunctionExpression.Min(property);
        }

        public static AggregateFunctionExpression Max(this IPropertyExpression property)
        {
            return FunctionExpression.Max(property);
        }

        #endregion

        #region 算术表达式 ArithmeticExpression

        public static ArithmeticExpression Add(this IPropertyExpression property, IValueExpression value)
        {
            return new ArithmeticExpression(property, Operator.Add, value);
        }

        public static ArithmeticExpression Sub(this IPropertyExpression property, IValueExpression value)
        {
            return new ArithmeticExpression(property, Operator.Sub, value);
        }

        public static ArithmeticExpression Mul(this IPropertyExpression property, IValueExpression value)
        {
            return new ArithmeticExpression(property, Operator.Mul, value);
        }

        public static ArithmeticExpression Div(this IPropertyExpression property, IValueExpression value)
        {
            return new ArithmeticExpression(property, Operator.Div, value);
        }

        public static ArithmeticExpression Mod(this IPropertyExpression property, IValueExpression value)
        {
            return new ArithmeticExpression(property, Operator.Mod, value);
        }

        #endregion

        #endregion

        #region 过滤表达式 FilterExpression

        #region LogicExpresssion

        public static IFilterExpression AllEq(this IEnumerable<IPropertyExpression> props, IEnumerable<IValueExpression> values)
        {
            IFilterExpression filter = null;
            var em = values.GetEnumerator();
            foreach (var prop in props)
            {
                em.MoveNext();
                filter = filter == null ? prop.Eq(em.Current) as IFilterExpression : filter.And(prop.Eq(em.Current));
            }
            return filter;
        }

        public static IFilterExpression AllEq(this IEnumerable<IPropertyExpression> props, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is IValueExpression ? val as IValueExpression : new LiteralValueExpression(val));
            return AllEq(props, vals);
        }

        public static IFilterExpression AllEq(this IEnumerable<IPropertyExpression> props, params IValueExpression[] values)
        {
            return AllEq(props, values.AsEnumerable());
        }

        public static IFilterExpression AllEq(this IEnumerable<IPropertyExpression> props, params object[] values)
        {
            return AllEq(props, values.AsEnumerable());
        }

        public static IFilterExpression AllEqVarParam(this IEnumerable<IPropertyExpression> props)
        {
            IFilterExpression filter = null;
            foreach (var prop in props)
            {
                filter = filter == null ? prop.EqVarParam() as IFilterExpression : filter.And(prop.EqVarParam());
            }
            return filter;
        }

        public static IFilterExpression AnyEq(this IEnumerable<IPropertyExpression> props, IEnumerable<IValueExpression> values)
        {
            IFilterExpression filter = null;
            var em = values.GetEnumerator();
            foreach (var prop in props)
            {
                em.MoveNext();
                filter = filter == null ? prop.Eq(em.Current) as IFilterExpression : filter.Or(prop.Eq(em.Current));
            }
            return filter;
        }

        public static IFilterExpression AnyEq(this IEnumerable<IPropertyExpression> props, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is IValueExpression ? val as IValueExpression : new LiteralValueExpression(val));
            return AnyEq(props, vals);
        }

        public static IFilterExpression AnyEq(this IEnumerable<IPropertyExpression> props, params IValueExpression[] values)
        {
            return AnyEq(props, values.AsEnumerable());
        }

        public static IFilterExpression AnyEq(this IEnumerable<IPropertyExpression> props, params object[] values)
        {
            return AnyEq(props, values.AsEnumerable());
        }

        public static IFilterExpression AnyEqVarParam(this IEnumerable<IPropertyExpression> props)
        {
            IFilterExpression filter = null;
            foreach (var prop in props)
            {
                filter = filter == null ? prop.EqVarParam() as IFilterExpression : filter.Or(prop.EqVarParam());
            }
            return filter;
        }

        public static LogicExpression And(this IFilterExpression a, IFilterExpression b)
        {
            return new LogicExpression(a, LogicOperator.And, b);
        }

        public static LogicExpression Or(this IFilterExpression a, IFilterExpression b)
        {
            return new LogicExpression(a, LogicOperator.Or, b);
        }

        #endregion

        #region ComparisonExpression

        public static UnaryComparisonExpression IsNull(this IValueExpression prop)
        {
            return new UnaryComparisonExpression(prop, Operator.IsNull);
            //return new ComparisonExpression(prop, Operator.Is, new LiteralValueExpression(null));
        }

        public static UnaryComparisonExpression IsNotNull(this IValueExpression prop)
        {
            return new UnaryComparisonExpression(prop, Operator.IsNotNull);
            //return new ComparisonExpression(prop, Operator.IsNot, new LiteralValueExpression(null));
        }

        public static ComparisonExpression Eq(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Eq, val);
        }

        public static ComparisonExpression Neq(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Neq, val);
        }

        public static ComparisonExpression Gt(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Gt, val);
        }

        public static ComparisonExpression GtOrEq(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.GtOrEq, val);
        }

        public static ComparisonExpression Lt(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Lt, val);
        }

        public static ComparisonExpression LtOrEq(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.LtOrEq, val);
        }

        public static ComparisonExpression Like(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Like, val);
        }

        public static ComparisonExpression NotLike(this IValueExpression prop, IValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.NotLike, val);
        }

        public static ComparisonExpression Eq(this IValueExpression prop, object val)
        {
            return prop.Eq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Neq(this IValueExpression prop, object val)
        {
            return prop.Neq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Gt(this IValueExpression prop, object val)
        {
            return prop.Gt(new LiteralValueExpression(val));
        }

        public static ComparisonExpression GtOrEq(this IValueExpression prop, object val)
        {
            return prop.GtOrEq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Lt(this IValueExpression prop, object val)
        {
            return prop.Lt(new LiteralValueExpression(val));
        }

        public static ComparisonExpression LtOrEq(this IValueExpression prop, object val)
        {
            return prop.LtOrEq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Like(this IValueExpression prop, object val)
        {
            return prop.Like(new LiteralValueExpression(val));
        }

        public static ComparisonExpression NotLike(this IValueExpression prop, object val)
        {
            return prop.NotLike(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Between(this IValueExpression prop, object a, object b)
        {
            return new ComparisonExpression(prop, Operator.Between, new BetweenValueExpression(new LiteralValueExpression(a), new LiteralValueExpression(b)));
        }

        public static ComparisonExpression NotBetween(this IValueExpression prop, object a, object b)
        {
            return new ComparisonExpression(prop, Operator.NotBetween, new BetweenValueExpression(new LiteralValueExpression(a), new LiteralValueExpression(b)));
        }

        public static ComparisonExpression In(this IValueExpression prop, params object[] values)
        {
            return new ComparisonExpression(prop, Operator.In, new CollectionExpression(values.Select(val => new LiteralValueExpression(val)).ToArray()));
        }

        public static ComparisonExpression NotIn(this IValueExpression prop, params object[] values)
        {
            return new ComparisonExpression(prop, Operator.NotIn, new CollectionExpression(values.Select(val => new LiteralValueExpression(val)).ToArray()));
        }

        public static ComparisonExpression In(this IValueExpression prop, ISelectStatement select)
        {
            return new ComparisonExpression(prop, Operator.In, select);
        }

        public static ComparisonExpression NotIn(this IValueExpression prop, ISelectStatement select)
        {
            return new ComparisonExpression(prop, Operator.NotIn, select);
        }

        public static ComparisonExpression EqVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.Eq(new ParamExpression(param));
        }

        public static ComparisonExpression NeqVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.Neq(new ParamExpression(param));
        }

        public static ComparisonExpression GtVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.Gt(new ParamExpression(param));
        }

        public static ComparisonExpression GtOrEqVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.GtOrEq(new ParamExpression(param));
        }

        public static ComparisonExpression LtVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.Lt(new ParamExpression(param));
        }

        public static ComparisonExpression LtOrEqVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.LtOrEq(new ParamExpression(param));
        }

        public static ComparisonExpression LikeVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.Like(new ParamExpression(param));
        }

        public static ComparisonExpression NotLikeVarParam(this IValueExpression prop, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (prop is IPropertyExpression) param = (prop as IPropertyExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return prop.NotLike(new ParamExpression(param));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="parammin"></param>
        /// <param name="parammax"></param>
        /// <returns>Expression:{prop.Expression} BETWEEN @{prop.Name}1 AND @{prop.Name}2</returns>
        public static ComparisonExpression BetweenVarParam(this IValueExpression prop, string parammin = null, string parammax = null)
        {
            if (string.IsNullOrWhiteSpace(parammin))
            {
                if (prop is IPropertyExpression) parammin = (prop as IPropertyExpression).Name + "1";
                else throw new ArgumentNullException("param");
            }
            if (string.IsNullOrWhiteSpace(parammax))
            {
                if (prop is IPropertyExpression) parammax = (prop as IPropertyExpression).Name + "2";
                else throw new ArgumentNullException("parammax");
            }
            return new ComparisonExpression(prop, Operator.Between, new BetweenValueExpression(new ParamExpression(parammin), new ParamExpression(parammax)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="parammin"></param>
        /// <param name="parammax"></param>
        /// <returns>Expression:{prop.Expression} NOT BETWEEN @{prop.Name}1 AND @{prop.Name}2</returns>
        public static ComparisonExpression NotBetweenVarParam(this IValueExpression prop, string parammin = null, string parammax = null)
        {
            if (string.IsNullOrWhiteSpace(parammin))
            {
                if (prop is IPropertyExpression) parammin = (prop as IPropertyExpression).Name + "1";
                else throw new ArgumentNullException("param");
            }
            if (string.IsNullOrWhiteSpace(parammax))
            {
                if (prop is IPropertyExpression) parammax = (prop as IPropertyExpression).Name + "2";
                else throw new ArgumentNullException("parammax");
            }
            return new ComparisonExpression(prop, Operator.NotBetween, new BetweenValueExpression(new ParamExpression(parammin), new ParamExpression(parammax)));
        }

        public static ComparisonExpression BetweenVarCustomer(this IValueExpression prop, string customer)
        {
            return new ComparisonExpression(prop, Operator.Between, new CustomerExpression(customer));
        }

        public static ComparisonExpression NotBetweenVarCustomer(this IValueExpression prop, string customer)
        {
            return new ComparisonExpression(prop, Operator.NotBetween, new CustomerExpression(customer));
        }

        public static ComparisonExpression InVarCustomer(this IValueExpression prop, string customer)
        {
            return new ComparisonExpression(prop, Operator.In, new CustomerExpression(customer));
        }

        public static ComparisonExpression NotInVarCustomer(this IValueExpression prop, string customer)
        {
            return new ComparisonExpression(prop, Operator.NotIn, new CustomerExpression(customer));
        }

        #endregion

        #endregion
    }
}
