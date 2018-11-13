using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression.Extension
{
    public static class ExpressionExtensions
    {
        public static BatchSqlStatement Concat(this ISqlStatement sql, params ISqlStatement[] otherSqls)
        {
            List<ISqlStatement> sqls = new List<ISqlStatement>();
            sqls.Add(sql);
            sqls.AddRange(otherSqls);
            return new BatchSqlStatement(sqls.ToArray());
        }

        public static BracketExpression Bracket<T>(this T exp)
            where T : ISimpleValue
        {
            return new BracketExpression(exp);
        }

        #region Alias

        public static AliasTableExpression As(this ITable table, string alias)
        {
            return new AliasTableExpression(table, alias);
        }

        public static SelectItemExpression As(this ISimpleValue val, string alias)
        {
            return new SelectItemExpression(val, alias);
        }

        public static AliasSubQueryExpression As(this ISubQueryExpression subquery, string alias)
        {
            return new AliasSubQueryExpression(subquery, alias);
        }

        public static AliasSubQueryExpression As(this ISelectStatement select, string alias)
        {
            return As(new SubQueryExpression(select.Query), alias);
        }

        public static AliasSubQueryExpression As(this IQueryStatement query, string alias)
        {
            return As(new SubQueryExpression(query), alias);
        }

        #endregion

        #region Column

        #region ShortCut

        public static Param ToP(this IColumn column, string param = null)
        {
            return ToParam(column, param);
        }

        public static SetExpression SetC(this IColumn column, string custom)
        {
            return SetVarCustom(column, custom);
        }

        public static SetExpression SetP(this IColumn column, string param = null)
        {
            return SetVarParam(column, param);
        }

        #endregion

        public static AllColumnsExpression Asterisk(this IAliasDataset dataset)
        {
            var alias = string.IsNullOrEmpty(dataset.Alias) && dataset is IAliasTableExpression
                              ? (dataset as IAliasTableExpression).Table.Name
                              : dataset.Alias;
            return new AllColumnsExpression(alias);
        }

        public static AllColumnsExpression Asterisk(this ITable table)
        {
            return new AllColumnsExpression(table.Name);
        }

        public static Param ToParam(this IColumn column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                param = column.Name;
                if (column.Option.Column2ParamContractHandler != null)
                {
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
            }
            return new Param(param);
        }

        public static SetExpression SetVarCustom(this IColumn column, string custom)
        {
            return Set(column, new CustomExpression(custom));
        }

        public static SetExpression SetVarParam(this IColumn column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = column.Name;
            return Set(column, column.ToParam(param));
        }

        public static SetExpression Set(this IColumn column, object value = null)
        {
            return new SetExpression(column, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static OrderExpression Desc(this ISimpleValue value)
        {
            return new OrderExpression(value, OrderEnum.Desc);
        }

        public static OrderExpression Asc(this ISimpleValue value)
        {
            return new OrderExpression(value, OrderEnum.Asc);
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

        public static ArithmeticExpression Add(this ISimpleValue value, object val)
        {
            return new ArithmeticExpression(value, Operator.Add, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ArithmeticExpression Sub(this ISimpleValue value, object val)
        {
            return new ArithmeticExpression(value, Operator.Sub, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ArithmeticExpression Mul(this ISimpleValue value, object val)
        {
            return new ArithmeticExpression(value, Operator.Mul, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ArithmeticExpression Div(this ISimpleValue value, object val)
        {
            return new ArithmeticExpression(value, Operator.Div, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ArithmeticExpression Mod(this ISimpleValue value, object val)
        {
            return new ArithmeticExpression(value, Operator.Mod, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        #endregion

        #endregion

        #region 过滤表达式 FilterExpression

        #region LogicExpresssion

        public static LogicExpression And(this ISimpleValue value, object assert)
        {
            return new LogicExpression(value, Operator.And, assert is ISimpleValue ? assert as ISimpleValue : new LiteralValue(assert));
        }

        public static LogicExpression Or(this ISimpleValue value, object assert)
        {
            return new LogicExpression(value, Operator.Or, assert is ISimpleValue ? assert as ISimpleValue : new LiteralValue(assert));
        }

        public static IBoolValue AllSatisfied(this IEnumerable<ISimpleValue> exps)
        {
            IBoolValue filter = null;
            filter = exps.First().And(exps.Skip(1).First());
            foreach (var exp in exps.Skip(2))
            {
                filter = filter.And(exp);
            }
            return filter;
        }

        public static IBoolValue AnySatisfied(this IEnumerable<ISimpleValue> exps)
        {
            IBoolValue filter = null;
            filter = exps.First().Or(exps.Skip(1).First());
            foreach (var exp in exps.Skip(2))
            {
                filter = filter.Or(exp);
            }
            return filter;
        }

        public static IBoolValue AllEqVarParam(this IEnumerable<IColumn> columns)
        {
            IBoolValue boolValue = null;
            foreach (var column in columns)
            {
                boolValue = boolValue == null ? column.EqVarParam() as IBoolValue : boolValue.And(column.EqVarParam());
            }
            return boolValue;
        }

        public static IBoolValue AnyEqVarParam(this IEnumerable<IColumn> columns)
        {
            IBoolValue boolValue = null;
            foreach (var column in columns)
            {
                boolValue = boolValue == null ? column.EqVarParam() as IBoolValue : boolValue.Or(column.EqVarParam());
            }
            return boolValue;
        }

        #endregion

        #region ComparisonExpression

        public static UnaryComparisonExpression IsNull(this ISimpleValue value)
        {
            return new UnaryComparisonExpression(Operator.IsNull, value);
            //return new ComparisonExpression(value, Operator.Is, new LiteralValueExpression(null));
        }

        public static UnaryComparisonExpression IsNotNull(this ISimpleValue value)
        {
            return new UnaryComparisonExpression(Operator.IsNotNull, value);
            //return new ComparisonExpression(value, Operator.IsNot, new LiteralValueExpression(null));
        }

        public static ComparisonExpression Eq(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.Eq, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ComparisonExpression Neq(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.Neq, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ComparisonExpression Gt(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.Gt, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ComparisonExpression GtOrEq(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.GtOrEq, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ComparisonExpression Lt(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.Lt, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ComparisonExpression LtOrEq(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.LtOrEq, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ComparisonExpression Like(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.Like, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static ComparisonExpression NotLike(this ISimpleValue value, object val)
        {
            return new ComparisonExpression(value, Operator.NotLike, val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
        }

        public static BetweenExpression Between(this ISimpleValue value, object lower, object upper)
        {
            return new BetweenExpression(value, new LiteralValue(lower), new LiteralValue(upper));
        }

        public static NotBetweenExpression NotBetween(this ISimpleValue value, object lower, object upper)
        {
            return new NotBetweenExpression(value, new LiteralValue(lower), new LiteralValue(upper));
        }

        public static InExpression In(this ISimpleValue value, IEnumerable<object> values)
        {
            return new InExpression(value, new ValueCollectionExpression(values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val)).ToArray()));
        }

        public static InExpression In(this ISimpleValue value, params object[] values)
        {
            return new InExpression(value, new ValueCollectionExpression(values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val)).ToArray()));
        }

        public static InExpression In(this ISimpleValue value, ISubQueryExpression subquery)
        {
            return new InExpression(value, subquery);
        }

        public static InExpression In(this ISimpleValue value, ISelectStatement select)
        {
            return new InExpression(value, new SubQueryExpression(select.Query));
        }

        public static InExpression In(this ISimpleValue value, IQueryStatement query)
        {
            return new InExpression(value, new SubQueryExpression(query));
        }

        public static NotInExpression NotIn(this ISimpleValue value, IEnumerable<object> collections)
        {
            return new NotInExpression(value, new ValueCollectionExpression(collections.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val)).ToArray()));
        }

        public static NotInExpression NotIn(this ISimpleValue value, params object[] collections)
        {
            return new NotInExpression(value, new ValueCollectionExpression(collections.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val)).ToArray()));
        }

        public static NotInExpression NotIn(this ISimpleValue value, ISubQueryExpression subquery)
        {
            return new NotInExpression(value, subquery);
        }

        public static NotInExpression NotIn(this ISimpleValue value, ISelectStatement select)
        {
            return new NotInExpression(value, new SubQueryExpression(select.Query));
        }

        public static NotInExpression NotIn(this ISimpleValue value, IQueryStatement query)
        {
            return new NotInExpression(value, new SubQueryExpression(query));
        }

        public static ComparisonExpression EqVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.Eq(new Param(param));
        }

        public static ComparisonExpression NeqVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.Neq(new Param(param));
        }

        public static ComparisonExpression GtVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.Gt(new Param(param));
        }

        public static ComparisonExpression GtOrEqVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.GtOrEq(new Param(param));
        }

        public static ComparisonExpression LtVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.Lt(new Param(param));
        }

        public static ComparisonExpression LtOrEqVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.LtOrEq(new Param(param));
        }

        public static ComparisonExpression LikeVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.Like(new Param(param));
        }

        public static ComparisonExpression NotLikeVarParam(this ISimpleValue value, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (value is IColumn)
                {
                    param = (value as IColumn).Name;
                    param = value.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return value.NotLike(new Param(param));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{column.Expression} BETWEEN @{column.Name}Lower AND @{column.Name}Upper</returns>
        public static BetweenExpression BetweenVarParam(this ISimpleValue value, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (value is IColumn)
                {
                    paramLower = paramLower ?? (value as IColumn).Name + "Lower";
                    paramLower = value.Option.Column2ParamContractHandler?.Invoke(paramLower) ?? paramLower;
                }
                else throw new ArgumentNullException(nameof(paramLower));
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (value is IColumn)
                {
                    paramUpper = paramUpper ?? (value as IColumn).Name + "Upper";
                    paramUpper = value.Option.Column2ParamContractHandler?.Invoke(paramUpper) ?? paramUpper;
                }
                else throw new ArgumentNullException(nameof(paramUpper));
            }
            return new BetweenExpression(value, new Param(paramLower), new Param(paramUpper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{column.Expression} NOT BETWEEN @{column.Name}Lower AND @{column.Name}Upper</returns>
        public static NotBetweenExpression NotBetweenVarParam(this ISimpleValue value, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (value is IColumn)
                {
                    paramLower = paramLower ?? (value as IColumn).Name + "Lower";
                    paramLower = value.Option.Column2ParamContractHandler?.Invoke(paramLower) ?? paramLower;
                }
                else throw new ArgumentNullException(nameof(paramLower));
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (value is IColumn)
                {
                    paramUpper = paramUpper ?? (value as IColumn).Name + "Upper";
                    paramUpper = value.Option.Column2ParamContractHandler?.Invoke(paramUpper) ?? paramUpper;
                }
                else throw new ArgumentNullException(nameof(paramUpper));
            }
            return new NotBetweenExpression(value, new Param(paramLower), new Param(paramUpper));
        }

        #endregion

        #endregion
    }
}