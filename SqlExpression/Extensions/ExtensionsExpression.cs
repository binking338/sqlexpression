using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsExpression
    {
        public static BatchSqlStatement Concat(this ISqlStatement sql, params ISqlStatement[] otherSqls)
        {
            List<ISqlStatement> sqls = new List<ISqlStatement>();
            sqls.Add(sql);
            sqls.AddRange(otherSqls);
            return new BatchSqlStatement(sqls.ToArray());
        }
        
        public static T B<T>(this T exp)
            where T : IBinaryExpression
        {
            return Bracket(exp);
        }

        public static T Bracket<T>(this T exp)
            where T : IBinaryExpression
        {
            exp.WithBracket = true;
            return exp;
        }

        #region Column

        #region ShortCut

        public static IParamExpression ToP(this IColumnExpression column, string param = null)
        {
            return ToParam(column, param);
        }

        public static ISetItemExpression SetC(this IColumnExpression column, string customer)
        {
            return SetVarCustomer(column, customer);
        }

        public static ISetItemExpression SetP(this IColumnExpression column, string param = null)
        {
            return SetVarParam(column, param);
        }

        #endregion

        public static IParamExpression ToParam(this IColumnExpression column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = column.Name;
            return new ParamExpression(param);
        }

        public static ISetItemExpression SetVarCustomer(this IColumnExpression column, string customer)
        {
            return Set(column, new CustomerExpression(customer));
        }

        public static ISetItemExpression SetVarParam(this IColumnExpression column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = column.Name;
            return Set(column, column.ToParam(param));
        }

        public static ISetItemExpression Set(this IColumnExpression column, object value = null)
        {
            return Set(column, value is IValueExpression ? value as IValueExpression : new LiteralValueExpression(value));
        }

        public static ISetItemExpression Set(this IColumnExpression column, IValueExpression value)
        {
            return new SetItemExpression(column, value);
        }

        public static IAsExpression As(this ISelectableValueExpression val, string asName)
        {
            return new AsExpression(val, new ColumnExpression(asName));
        }

        public static IOrderExpression Desc(this IColumnExpression col)
        {
            return new OrderExpression(col, OrderEnum.Desc);
        }

        public static IOrderExpression Asc(this IColumnExpression col)
        {
            return new OrderExpression(col, OrderEnum.Asc);
        }

        #region 聚合函数 AggregateFunctionExpression

        public static AggregateFunctionExpression Sum(this IColumnExpression column)
        {
            return FunctionExpression.Sum(column);
        }

        public static AggregateFunctionExpression Count(this IColumnExpression column)
        {
            return FunctionExpression.Count(column);
        }

        public static AggregateFunctionExpression Avg(this IColumnExpression column)
        {
            return FunctionExpression.Avg(column);
        }

        public static AggregateFunctionExpression Min(this IColumnExpression column)
        {
            return FunctionExpression.Min(column);
        }

        public static AggregateFunctionExpression Max(this IColumnExpression column)
        {
            return FunctionExpression.Max(column);
        }

        #endregion

        #region 算术表达式 ArithmeticExpression

        public static ArithmeticExpression Add(this IColumnExpression column, IValueExpression value)
        {
            return new ArithmeticExpression(column, Operator.Add, value);
        }

        public static ArithmeticExpression Sub(this IColumnExpression column, IValueExpression value)
        {
            return new ArithmeticExpression(column, Operator.Sub, value);
        }

        public static ArithmeticExpression Mul(this IColumnExpression column, IValueExpression value)
        {
            return new ArithmeticExpression(column, Operator.Mul, value);
        }

        public static ArithmeticExpression Div(this IColumnExpression column, IValueExpression value)
        {
            return new ArithmeticExpression(column, Operator.Div, value);
        }

        public static ArithmeticExpression Mod(this IColumnExpression column, IValueExpression value)
        {
            return new ArithmeticExpression(column, Operator.Mod, value);
        }

        #endregion

        #endregion

        #region 过滤表达式 FilterExpression

        #region LogicExpresssion

        public static LogicExpression AllEq(this IEnumerable<IColumnExpression> cols, IEnumerable<IValueExpression> values)
        {
            IFilterExpression filter = null;
            LogicExpression logic = null;
            var em = values.GetEnumerator();
            foreach (var col in cols)
            {
                em.MoveNext();
                if(filter == null)
                {
                    filter = col.Eq(em.Current);
                }
                else
                {
                    logic = filter.And(col.Eq(em.Current));
                    filter = logic;
                }
                //filter = filter == null ? col.Eq(em.Current) as IFilterExpression : filter.And(col.Eq(em.Current));
            }
            return logic;
        }

        public static LogicExpression AllEq(this IEnumerable<IColumnExpression> cols, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is IValueExpression ? val as IValueExpression : new LiteralValueExpression(val));
            return AllEq(cols, vals);
        }

        public static LogicExpression AllEq(this IEnumerable<IColumnExpression> cols, params IValueExpression[] values)
        {
            return AllEq(cols, values.AsEnumerable());
        }

        public static LogicExpression AllEq(this IEnumerable<IColumnExpression> cols, params object[] values)
        {
            return AllEq(cols, values.AsEnumerable());
        }

        public static LogicExpression AllEqVarParam(this IEnumerable<IColumnExpression> cols)
        {
            IFilterExpression filter = null;
            LogicExpression logic = null;
            foreach (var col in cols)
            {
                if (filter == null)
                {
                    filter = col.EqVarParam();
                }
                else
                {
                    logic = filter.And(col.EqVarParam());
                    filter = logic;
                }
                //filter = filter == null ? col.EqVarParam() as IFilterExpression : filter.And(col.EqVarParam());
            }
            return logic;
        }

        public static LogicExpression AnyEq(this IEnumerable<IColumnExpression> cols, IEnumerable<IValueExpression> values)
        {
            IFilterExpression filter = null;
            LogicExpression logic = null;
            var em = values.GetEnumerator();
            foreach (var col in cols)
            {
                em.MoveNext();
                if (filter == null)
                {
                    filter = col.Eq(em.Current);
                }
                else
                {
                    logic = filter.Or(col.Eq(em.Current));
                    filter = logic;
                }
                //filter = filter == null ? col.Eq(em.Current) as IFilterExpression : filter.Or(col.Eq(em.Current));
            }
            return logic;
        }

        public static LogicExpression AnyEq(this IEnumerable<IColumnExpression> cols, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is IValueExpression ? val as IValueExpression : new LiteralValueExpression(val));
            return AnyEq(cols, vals);
        }

        public static LogicExpression AnyEq(this IEnumerable<IColumnExpression> cols, params IValueExpression[] values)
        {
            return AnyEq(cols, values.AsEnumerable());
        }

        public static LogicExpression AnyEq(this IEnumerable<IColumnExpression> cols, params object[] values)
        {
            return AnyEq(cols, values.AsEnumerable());
        }

        public static LogicExpression AnyEqVarParam(this IEnumerable<IColumnExpression> cols)
        {
            IFilterExpression filter = null;
            LogicExpression logic = null;
            foreach (var col in cols)
            {
                if (filter == null)
                {
                    filter = col.EqVarParam();
                }
                else
                {
                    logic = filter.Or(col.EqVarParam());
                    filter = logic;
                }
                //filter = filter == null ? col.EqVarParam() as IFilterExpression : filter.Or(col.EqVarParam());
            }
            return logic;
        }

        public static LogicExpression And(this IFilterExpression a, IFilterExpression b)
        {
            if(a is ILogicExpression)
            {
                (a as ILogicExpression).WithBracket = true;
            }
            if (b is ILogicExpression)
            {
                (b as ILogicExpression).WithBracket = true;
            }
            return new LogicExpression(a, LogicOperator.And, b);
        }

        public static LogicExpression Or(this IFilterExpression a, IFilterExpression b)
        {
            if (a is ILogicExpression)
            {
                (a as ILogicExpression).WithBracket = true;
            }
            if (b is ILogicExpression)
            {
                (b as ILogicExpression).WithBracket = true;
            }
            return new LogicExpression(a, LogicOperator.Or, b);
        }

        #endregion

        #region ComparisonExpression

        public static UnaryComparisonExpression IsNull(this IValueExpression col)
        {
            return new UnaryComparisonExpression(col, Operator.IsNull);
            //return new ComparisonExpression(col, Operator.Is, new LiteralValueExpression(null));
        }

        public static UnaryComparisonExpression IsNotNull(this IValueExpression col)
        {
            return new UnaryComparisonExpression(col, Operator.IsNotNull);
            //return new ComparisonExpression(col, Operator.IsNot, new LiteralValueExpression(null));
        }

        public static ComparisonExpression Eq(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Eq, val);
        }

        public static ComparisonExpression Neq(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Neq, val);
        }

        public static ComparisonExpression Gt(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Gt, val);
        }

        public static ComparisonExpression GtOrEq(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.GtOrEq, val);
        }

        public static ComparisonExpression Lt(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Lt, val);
        }

        public static ComparisonExpression LtOrEq(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.LtOrEq, val);
        }

        public static ComparisonExpression Like(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Like, val);
        }

        public static ComparisonExpression NotLike(this IValueExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.NotLike, val);
        }

        public static ComparisonExpression Eq(this IValueExpression col, object val)
        {
            return col.Eq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Neq(this IValueExpression col, object val)
        {
            return col.Neq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Gt(this IValueExpression col, object val)
        {
            return col.Gt(new LiteralValueExpression(val));
        }

        public static ComparisonExpression GtOrEq(this IValueExpression col, object val)
        {
            return col.GtOrEq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Lt(this IValueExpression col, object val)
        {
            return col.Lt(new LiteralValueExpression(val));
        }

        public static ComparisonExpression LtOrEq(this IValueExpression col, object val)
        {
            return col.LtOrEq(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Like(this IValueExpression col, object val)
        {
            return col.Like(new LiteralValueExpression(val));
        }

        public static ComparisonExpression NotLike(this IValueExpression col, object val)
        {
            return col.NotLike(new LiteralValueExpression(val));
        }

        public static ComparisonExpression Between(this IValueExpression col, object a, object b)
        {
            return new ComparisonExpression(col, Operator.Between, new BetweenValueExpression(new LiteralValueExpression(a), new LiteralValueExpression(b)));
        }

        public static ComparisonExpression NotBetween(this IValueExpression col, object a, object b)
        {
            return new ComparisonExpression(col, Operator.NotBetween, new BetweenValueExpression(new LiteralValueExpression(a), new LiteralValueExpression(b)));
        }

        public static ComparisonExpression In(this IValueExpression col, params object[] values)
        {
            return new ComparisonExpression(col, Operator.In, new CollectionExpression(values.Select(val => new LiteralValueExpression(val)).ToArray()));
        }

        public static ComparisonExpression NotIn(this IValueExpression col, params object[] values)
        {
            return new ComparisonExpression(col, Operator.NotIn, new CollectionExpression(values.Select(val => new LiteralValueExpression(val)).ToArray()));
        }

        public static ComparisonExpression In(this IValueExpression col, ISelectStatement select)
        {
            return new ComparisonExpression(col, Operator.In, select);
        }

        public static ComparisonExpression NotIn(this IValueExpression col, ISelectStatement select)
        {
            return new ComparisonExpression(col, Operator.NotIn, select);
        }

        public static ComparisonExpression EqVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Eq(new ParamExpression(param));
        }

        public static ComparisonExpression NeqVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Neq(new ParamExpression(param));
        }

        public static ComparisonExpression GtVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Gt(new ParamExpression(param));
        }

        public static ComparisonExpression GtOrEqVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.GtOrEq(new ParamExpression(param));
        }

        public static ComparisonExpression LtVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Lt(new ParamExpression(param));
        }

        public static ComparisonExpression LtOrEqVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.LtOrEq(new ParamExpression(param));
        }

        public static ComparisonExpression LikeVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Like(new ParamExpression(param));
        }

        public static ComparisonExpression NotLikeVarParam(this IValueExpression col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumnExpression) param = (col as IColumnExpression).Name;
                else throw new ArgumentNullException("param");
            }
            return col.NotLike(new ParamExpression(param));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="parammin"></param>
        /// <param name="parammax"></param>
        /// <returns>Expression:{col.Expression} BETWEEN @{col.Name}1 AND @{col.Name}2</returns>
        public static ComparisonExpression BetweenVarParam(this IValueExpression col, string parammin = null, string parammax = null)
        {
            if (string.IsNullOrWhiteSpace(parammin))
            {
                if (col is IColumnExpression) parammin = (col as IColumnExpression).Name + "1";
                else throw new ArgumentNullException("param");
            }
            if (string.IsNullOrWhiteSpace(parammax))
            {
                if (col is IColumnExpression) parammax = (col as IColumnExpression).Name + "2";
                else throw new ArgumentNullException("parammax");
            }
            return new ComparisonExpression(col, Operator.Between, new BetweenValueExpression(new ParamExpression(parammin), new ParamExpression(parammax)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="parammin"></param>
        /// <param name="parammax"></param>
        /// <returns>Expression:{col.Expression} NOT BETWEEN @{col.Name}1 AND @{col.Name}2</returns>
        public static ComparisonExpression NotBetweenVarParam(this IValueExpression col, string parammin = null, string parammax = null)
        {
            if (string.IsNullOrWhiteSpace(parammin))
            {
                if (col is IColumnExpression) parammin = (col as IColumnExpression).Name + "1";
                else throw new ArgumentNullException("param");
            }
            if (string.IsNullOrWhiteSpace(parammax))
            {
                if (col is IColumnExpression) parammax = (col as IColumnExpression).Name + "2";
                else throw new ArgumentNullException("parammax");
            }
            return new ComparisonExpression(col, Operator.NotBetween, new BetweenValueExpression(new ParamExpression(parammin), new ParamExpression(parammax)));
        }

        public static ComparisonExpression BetweenVarCustomer(this IValueExpression col, string customer)
        {
            return new ComparisonExpression(col, Operator.Between, new CustomerExpression(customer));
        }

        public static ComparisonExpression NotBetweenVarCustomer(this IValueExpression col, string customer)
        {
            return new ComparisonExpression(col, Operator.NotBetween, new CustomerExpression(customer));
        }

        public static ComparisonExpression InVarCustomer(this IValueExpression col, string customer)
        {
            return new ComparisonExpression(col, Operator.In, new CustomerExpression(customer));
        }

        public static ComparisonExpression NotInVarCustomer(this IValueExpression col, string customer)
        {
            return new ComparisonExpression(col, Operator.NotIn, new CustomerExpression(customer));
        }

        #endregion

        #endregion
    }
}
