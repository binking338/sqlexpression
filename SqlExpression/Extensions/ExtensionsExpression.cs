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

        #region Alias

        public static ITableAliasExpression As(this ITable table, string alias)
        {
            return new TableAliasExpression(table, string.IsNullOrWhiteSpace(alias) ? null : new DatasetAlias(alias));
        }

        public static ISelectItemExpression As(this ISimpleValue val, string alias)
        {
            return new SelectItemExpression(val, string.IsNullOrWhiteSpace(alias) ? null : new SelectFieldAlias(alias));
        }

        #endregion

        #region Column

        #region ShortCut

        public static IParam ToP(this IColumn column, string param = null)
        {
            return ToParam(column, param);
        }

        public static ISetFieldExpression SetC(this IColumn column, string customer)
        {
            return SetVarCustomer(column, customer);
        }

        public static ISetFieldExpression SetP(this IColumn column, string param = null)
        {
            return SetVarParam(column, param);
        }

        #endregion

        public static IParam ToParam(this IColumn column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = column.Name;
            return new Param(param);
        }

        public static ISetFieldExpression SetVarCustomer(this IColumn column, string customer)
        {
            return Set(column, new CustomerExpression(customer));
        }

        public static ISetFieldExpression SetVarParam(this IColumn column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = column.Name;
            return Set(column, column.ToParam(param));
        }

        public static ISetFieldExpression Set(this IColumn column, object value = null)
        {
            return Set(column, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static ISetFieldExpression Set(this IColumn column, ISimpleValue value)
        {
            return new SetFieldExpression(column, value);
        }

        public static IOrderExpression Desc(this IColumn col)
        {
            return new OrderExpression(col, OrderEnum.Desc);
        }

        public static IOrderExpression Asc(this IColumn col)
        {
            return new OrderExpression(col, OrderEnum.Asc);
        }

        #region 聚合函数 AggregateFunctionExpression

        public static AggregateFunctionExpression Sum(this IColumn column)
        {
            return AggregateFunctionExpression.Sum(column);
        }

        public static AggregateFunctionExpression Count(this IColumn column)
        {
            return AggregateFunctionExpression.Count(column);
        }

        public static AggregateFunctionExpression Avg(this IColumn column)
        {
            return AggregateFunctionExpression.Avg(column);
        }

        public static AggregateFunctionExpression Min(this IColumn column)
        {
            return AggregateFunctionExpression.Min(column);
        }

        public static AggregateFunctionExpression Max(this IColumn column)
        {
            return AggregateFunctionExpression.Max(column);
        }

        #endregion

        #region 算术表达式 ArithmeticExpression

        public static ArithmeticExpression Add(this IColumn column, ISimpleValue value)
        {
            return new ArithmeticExpression(column, Operator.Add, value);
        }

        public static ArithmeticExpression Sub(this IColumn column, ISimpleValue value)
        {
            return new ArithmeticExpression(column, Operator.Sub, value);
        }

        public static ArithmeticExpression Mul(this IColumn column, ISimpleValue value)
        {
            return new ArithmeticExpression(column, Operator.Mul, value);
        }

        public static ArithmeticExpression Div(this IColumn column, ISimpleValue value)
        {
            return new ArithmeticExpression(column, Operator.Div, value);
        }

        public static ArithmeticExpression Mod(this IColumn column, ISimpleValue value)
        {
            return new ArithmeticExpression(column, Operator.Mod, value);
        }

        #endregion

        #endregion

        #region 过滤表达式 FilterExpression

        #region LogicExpresssion

        public static IBoolValue AllEq(this IEnumerable<IColumn> cols, IEnumerable<ISimpleValue> values)
        {
            IBoolValue filter = null;
            var em = values.GetEnumerator();
            foreach (var col in cols)
            {
                em.MoveNext();
                filter = filter == null ? col.Eq(em.Current) as IBoolValue : filter.And(col.Eq(em.Current));
            }
            return filter;
        }

        public static IBoolValue AllEq(this IEnumerable<IColumn> cols, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
            return AllEq(cols, vals);
        }

        public static IBoolValue AllEq(this IEnumerable<IColumn> cols, params ISimpleValue[] values)
        {
            return AllEq(cols, values.AsEnumerable());
        }

        public static IBoolValue AllEq(this IEnumerable<IColumn> cols, params object[] values)
        {
            return AllEq(cols, values.AsEnumerable());
        }

        public static IBoolValue AllEqVarParam(this IEnumerable<IColumn> cols)
        {
            IBoolValue boolValue = null;
            foreach (var col in cols)
            {
                boolValue = boolValue == null ? col.EqVarParam() as IBoolValue : boolValue.And(col.EqVarParam());
            }
            return boolValue;
        }

        public static IBoolValue AnyEq(this IEnumerable<IColumn> cols, IEnumerable<ISimpleValue> values)
        {
            IBoolValue boolValue = null;
            var em = values.GetEnumerator();
            foreach (var col in cols)
            {
                em.MoveNext();
                boolValue = boolValue == null ? col.Eq(em.Current) as IBoolValue : boolValue.Or(col.Eq(em.Current));
            }
            return boolValue;
        }

        public static IBoolValue AnyEq(this IEnumerable<IColumn> cols, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
            return AnyEq(cols, vals);
        }

        public static IBoolValue AnyEq(this IEnumerable<IColumn> cols, params ISimpleValue[] values)
        {
            return AnyEq(cols, values.AsEnumerable());
        }

        public static IBoolValue AnyEq(this IEnumerable<IColumn> cols, params object[] values)
        {
            return AnyEq(cols, values.AsEnumerable());
        }

        public static IBoolValue AnyEqVarParam(this IEnumerable<IColumn> cols)
        {
            IBoolValue boolValue = null;
            foreach (var col in cols)
            {
                boolValue = boolValue == null ? col.EqVarParam() as IBoolValue : boolValue.Or(col.EqVarParam());
            }
            return boolValue;
        }

        public static LogicExpression And(this ISimpleValue a, ISimpleValue b)
        {
            if (a is ILogicExpression)
            {
                (a as ILogicExpression).WithBracket = true;
            }
            if (b is ILogicExpression)
            {
                (b as ILogicExpression).WithBracket = true;
            }
            return new LogicExpression(a, LogicOperator.And, b);
        }

        public static LogicExpression Or(this ISimpleValue a, ISimpleValue b)
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

        public static UnaryComparisonExpression IsNull(this ISimpleValue col)
        {
            return new UnaryComparisonExpression(col, Operator.IsNull);
            //return new ComparisonExpression(col, Operator.Is, new LiteralValueExpression(null));
        }

        public static UnaryComparisonExpression IsNotNull(this ISimpleValue col)
        {
            return new UnaryComparisonExpression(col, Operator.IsNotNull);
            //return new ComparisonExpression(col, Operator.IsNot, new LiteralValueExpression(null));
        }

        public static ComparisonExpression Eq(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Eq, val);
        }

        public static ComparisonExpression Neq(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Neq, val);
        }

        public static ComparisonExpression Gt(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Gt, val);
        }

        public static ComparisonExpression GtOrEq(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.GtOrEq, val);
        }

        public static ComparisonExpression Lt(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Lt, val);
        }

        public static ComparisonExpression LtOrEq(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.LtOrEq, val);
        }

        public static ComparisonExpression Like(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Like, val);
        }

        public static ComparisonExpression NotLike(this ISimpleValue col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.NotLike, val);
        }

        public static ComparisonExpression Eq(this ISimpleValue col, object val)
        {
            return col.Eq(new LiteralValue(val));
        }

        public static ComparisonExpression Neq(this ISimpleValue col, object val)
        {
            return col.Neq(new LiteralValue(val));
        }

        public static ComparisonExpression Gt(this ISimpleValue col, object val)
        {
            return col.Gt(new LiteralValue(val));
        }

        public static ComparisonExpression GtOrEq(this ISimpleValue col, object val)
        {
            return col.GtOrEq(new LiteralValue(val));
        }

        public static ComparisonExpression Lt(this ISimpleValue col, object val)
        {
            return col.Lt(new LiteralValue(val));
        }

        public static ComparisonExpression LtOrEq(this ISimpleValue col, object val)
        {
            return col.LtOrEq(new LiteralValue(val));
        }

        public static ComparisonExpression Like(this ISimpleValue col, object val)
        {
            return col.Like(new LiteralValue(val));
        }

        public static ComparisonExpression NotLike(this ISimpleValue col, object val)
        {
            return col.NotLike(new LiteralValue(val));
        }

        public static BetweenExpression Between(this ISimpleValue col, object a, object b)
        {
            return new BetweenExpression(col, new LiteralValue(a), new LiteralValue(b));
        }

        public static NotBetweenExpression NotBetween(this ISimpleValue col, object a, object b)
        {
            return new NotBetweenExpression(col, new LiteralValue(a), new LiteralValue(b));
        }

        public static InExpression In(this ISimpleValue col, params object[] values)
        {
            return new InExpression(col, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static NotInExpression NotIn(this ISimpleValue col, params object[] values)
        {
            return new NotInExpression(col, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static InExpression In(this ISimpleValue col, ISubQueryExpression subquery)
        {
            return new InExpression(col, subquery);
        }

        public static NotInExpression NotIn(this ISimpleValue col, ISubQueryExpression subquery)
        {
            return new NotInExpression(col, subquery);
        }

        public static InExpression In(this ISimpleValue col, ISelectStatement select)
        {
            return new InExpression(col, new SubQueryExpression(select));
        }

        public static NotInExpression NotIn(this ISimpleValue col, ISelectStatement select)
        {
            return new NotInExpression(col, new SubQueryExpression(select));
        }

        public static ComparisonExpression EqVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Eq(new Param(param));
        }

        public static ComparisonExpression NeqVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Neq(new Param(param));
        }

        public static ComparisonExpression GtVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Gt(new Param(param));
        }

        public static ComparisonExpression GtOrEqVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.GtOrEq(new Param(param));
        }

        public static ComparisonExpression LtVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Lt(new Param(param));
        }

        public static ComparisonExpression LtOrEqVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.LtOrEq(new Param(param));
        }

        public static ComparisonExpression LikeVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.Like(new Param(param));
        }

        public static ComparisonExpression NotLikeVarParam(this ISimpleValue col, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (col is IColumn) param = (col as IColumn).Name;
                else throw new ArgumentNullException("param");
            }
            return col.NotLike(new Param(param));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{col.Expression} BETWEEN @{col.Name}Lower AND @{col.Name}Upper</returns>
        public static BetweenExpression BetweenVarParam(this ISimpleValue col, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (col is IColumn) paramLower = (col as IColumn).Name + "Lower";
                else throw new ArgumentNullException("paramLower");
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (col is IColumn) paramUpper = (col as IColumn).Name + "Upper";
                else throw new ArgumentNullException("paramUpper");
            }
            return new BetweenExpression(col, new Param(paramLower), new Param(paramUpper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{col.Expression} NOT BETWEEN @{col.Name}Lower AND @{col.Name}Upper</returns>
        public static NotBetweenExpression NotBetweenVarParam(this ISimpleValue col, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (col is IColumn) paramLower = (col as IColumn).Name + "Lower";
                else throw new ArgumentNullException("paramLower");
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (col is IColumn) paramUpper = (col as IColumn).Name + "Upper";
                else throw new ArgumentNullException("paramUpper");
            }
            return new NotBetweenExpression(col, new Param(paramLower), new Param(paramUpper));
        }

        #endregion

        #endregion
    }
}
