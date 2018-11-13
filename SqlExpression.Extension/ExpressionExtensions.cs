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
            return Set(column, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static SetExpression Set(this IColumn column, ISimpleValue value)
        {
            if (value == null) value = new LiteralValue(null);
            return new SetExpression(column, value);
        }

        public static OrderExpression Desc(this ISimpleValue column)
        {
            return new OrderExpression(column, OrderEnum.Desc);
        }

        public static OrderExpression Asc(this ISimpleValue column)
        {
            return new OrderExpression(column, OrderEnum.Asc);
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

        public static ArithmeticExpression Add(this ISimpleValue column, ISimpleValue value)
        {
            if (value == null) value = new LiteralValue(null);
            return new ArithmeticExpression(column, Operator.Add, value);
        }

        public static ArithmeticExpression Sub(this ISimpleValue column, ISimpleValue value)
        {
            if (value == null) value = new LiteralValue(null);
            return new ArithmeticExpression(column, Operator.Sub, value);
        }

        public static ArithmeticExpression Mul(this ISimpleValue column, ISimpleValue value)
        {
            if (value == null) value = new LiteralValue(null);
            return new ArithmeticExpression(column, Operator.Mul, value);
        }

        public static ArithmeticExpression Div(this ISimpleValue column, ISimpleValue value)
        {
            if (value == null) value = new LiteralValue(null);
            return new ArithmeticExpression(column, Operator.Div, value);
        }

        public static ArithmeticExpression Mod(this ISimpleValue column, ISimpleValue value)
        {
            if (value == null) value = new LiteralValue(null);
            return new ArithmeticExpression(column, Operator.Mod, value);
        }

        public static ArithmeticExpression Add(this ISimpleValue column, object value)
        {
            return new ArithmeticExpression(column, Operator.Add, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static ArithmeticExpression Sub(this ISimpleValue column, object value)
        {
            return new ArithmeticExpression(column, Operator.Sub, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static ArithmeticExpression Mul(this ISimpleValue column, object value)
        {
            return new ArithmeticExpression(column, Operator.Mul, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static ArithmeticExpression Div(this ISimpleValue column, object value)
        {
            return new ArithmeticExpression(column, Operator.Div, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static ArithmeticExpression Mod(this ISimpleValue column, object value)
        {
            return new ArithmeticExpression(column, Operator.Mod, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        #endregion

        #endregion

        #region 过滤表达式 FilterExpression

        #region LogicExpresssion

        public static IBoolValue AllEq(this IEnumerable<IColumn> columns, IEnumerable<ISimpleValue> values)
        {
            IBoolValue filter = null;
            var em = values.GetEnumerator();
            foreach (var column in columns)
            {
                em.MoveNext();
                filter = filter == null ? column.Eq(em.Current) as IBoolValue : filter.And(column.Eq(em.Current));
            }
            return filter;
        }

        public static IBoolValue AllEq(this IEnumerable<IColumn> columns, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
            return AllEq(columns, vals);
        }

        public static IBoolValue AllEq(this IEnumerable<IColumn> columns, params ISimpleValue[] values)
        {
            return AllEq(columns, values.AsEnumerable());
        }

        public static IBoolValue AllEq(this IEnumerable<IColumn> columns, params object[] values)
        {
            return AllEq(columns, values.AsEnumerable());
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

        public static IBoolValue AnyEq(this IEnumerable<IColumn> columns, IEnumerable<ISimpleValue> values)
        {
            IBoolValue boolValue = null;
            var em = values.GetEnumerator();
            foreach (var column in columns)
            {
                em.MoveNext();
                boolValue = boolValue == null ? column.Eq(em.Current) as IBoolValue : boolValue.Or(column.Eq(em.Current));
            }
            return boolValue;
        }

        public static IBoolValue AnyEq(this IEnumerable<IColumn> columns, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
            return AnyEq(columns, vals);
        }

        public static IBoolValue AnyEq(this IEnumerable<IColumn> columns, params ISimpleValue[] values)
        {
            return AnyEq(columns, values.AsEnumerable());
        }

        public static IBoolValue AnyEq(this IEnumerable<IColumn> columns, params object[] values)
        {
            return AnyEq(columns, values.AsEnumerable());
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

        public static LogicExpression And(this ISimpleValue a, ISimpleValue b)
        {
            if (b == null) b = new LiteralValue(null);
            return new LogicExpression(a, Operator.And, b);
        }

        public static LogicExpression Or(this ISimpleValue a, ISimpleValue b)
        {
            if (b == null) b = new LiteralValue(null);
            return new LogicExpression(a, Operator.Or, b);
        }

        public static LogicExpression And(this ISimpleValue a, object b)
        {
            return new LogicExpression(a, Operator.And, b is ISimpleValue ? b as ISimpleValue : new LiteralValue(b));
        }

        public static LogicExpression Or(this ISimpleValue a, object b)
        {
            return new LogicExpression(a, Operator.Or, b is ISimpleValue ? b as ISimpleValue : new LiteralValue(b));
        }

        #endregion

        #region ComparisonExpression

        public static UnaryComparisonExpression IsNull(this ISimpleValue column)
        {
            return new UnaryComparisonExpression(Operator.IsNull, column);
            //return new ComparisonExpression(column, Operator.Is, new LiteralValueExpression(null));
        }

        public static UnaryComparisonExpression IsNotNull(this ISimpleValue column)
        {
            return new UnaryComparisonExpression(Operator.IsNotNull, column);
            //return new ComparisonExpression(column, Operator.IsNot, new LiteralValueExpression(null));
        }

        public static ComparisonExpression Eq(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.Eq, val);
        }

        public static ComparisonExpression Neq(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.Neq, val);
        }

        public static ComparisonExpression Gt(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.Gt, val);
        }

        public static ComparisonExpression GtOrEq(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.GtOrEq, val);
        }

        public static ComparisonExpression Lt(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.Lt, val);
        }

        public static ComparisonExpression LtOrEq(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.LtOrEq, val);
        }

        public static ComparisonExpression Like(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.Like, val);
        }

        public static ComparisonExpression NotLike(this ISimpleValue column, ISimpleValue val)
        {
            return new ComparisonExpression(column, Operator.NotLike, val);
        }

        public static ComparisonExpression Eq(this ISimpleValue column, object val)
        {
            return column.Eq(new LiteralValue(val));
        }

        public static ComparisonExpression Neq(this ISimpleValue column, object val)
        {
            return column.Neq(new LiteralValue(val));
        }

        public static ComparisonExpression Gt(this ISimpleValue column, object val)
        {
            return column.Gt(new LiteralValue(val));
        }

        public static ComparisonExpression GtOrEq(this ISimpleValue column, object val)
        {
            return column.GtOrEq(new LiteralValue(val));
        }

        public static ComparisonExpression Lt(this ISimpleValue column, object val)
        {
            return column.Lt(new LiteralValue(val));
        }

        public static ComparisonExpression LtOrEq(this ISimpleValue column, object val)
        {
            return column.LtOrEq(new LiteralValue(val));
        }

        public static ComparisonExpression Like(this ISimpleValue column, object val)
        {
            return column.Like(new LiteralValue(val));
        }

        public static ComparisonExpression NotLike(this ISimpleValue column, object val)
        {
            return column.NotLike(new LiteralValue(val));
        }

        public static BetweenExpression Between(this ISimpleValue column, object a, object b)
        {
            return new BetweenExpression(column, new LiteralValue(a), new LiteralValue(b));
        }

        public static NotBetweenExpression NotBetween(this ISimpleValue column, object a, object b)
        {
            return new NotBetweenExpression(column, new LiteralValue(a), new LiteralValue(b));
        }

        public static InExpression In(this ISimpleValue column, IEnumerable<object> values)
        {
            return new InExpression(column, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static InExpression In(this ISimpleValue column, params object[] values)
        {
            return new InExpression(column, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static NotInExpression NotIn(this ISimpleValue column, IEnumerable<object> values)
        {
            return new NotInExpression(column, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static NotInExpression NotIn(this ISimpleValue column, params object[] values)
        {
            return new NotInExpression(column, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static InExpression In(this ISimpleValue column, ISubQueryExpression subquery)
        {
            return new InExpression(column, subquery);
        }

        public static NotInExpression NotIn(this ISimpleValue column, ISubQueryExpression subquery)
        {
            return new NotInExpression(column, subquery);
        }

        public static InExpression In(this ISimpleValue column, ISelectStatement select)
        {
            return new InExpression(column, new SubQueryExpression(select.Query));
        }

        public static NotInExpression NotIn(this ISimpleValue column, ISelectStatement select)
        {
            return new NotInExpression(column, new SubQueryExpression(select.Query));
        }

        public static InExpression In(this ISimpleValue column, IQueryStatement query)
        {
            return new InExpression(column, new SubQueryExpression(query));
        }

        public static NotInExpression NotIn(this ISimpleValue column, IQueryStatement query)
        {
            return new NotInExpression(column, new SubQueryExpression(query));
        }

        public static ComparisonExpression EqVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.Eq(new Param(param));
        }

        public static ComparisonExpression NeqVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.Neq(new Param(param));
        }

        public static ComparisonExpression GtVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.Gt(new Param(param));
        }

        public static ComparisonExpression GtOrEqVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.GtOrEq(new Param(param));
        }

        public static ComparisonExpression LtVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.Lt(new Param(param));
        }

        public static ComparisonExpression LtOrEqVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.LtOrEq(new Param(param));
        }

        public static ComparisonExpression LikeVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.Like(new Param(param));
        }

        public static ComparisonExpression NotLikeVarParam(this ISimpleValue column, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (column is IColumn)
                {
                    param = (column as IColumn).Name;
                    param = column.Option.Column2ParamContractHandler?.Invoke(param) ?? param;
                }
                else throw new ArgumentNullException(nameof(param));
            }
            return column.NotLike(new Param(param));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{column.Expression} BETWEEN @{column.Name}Lower AND @{column.Name}Upper</returns>
        public static BetweenExpression BetweenVarParam(this ISimpleValue column, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (column is IColumn)
                {
                    paramLower = paramLower ?? (column as IColumn).Name + "Lower";
                    paramLower = column.Option.Column2ParamContractHandler?.Invoke(paramLower) ?? paramLower;
                }
                else throw new ArgumentNullException(nameof(paramLower));
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (column is IColumn)
                {
                    paramUpper = paramUpper ?? (column as IColumn).Name + "Upper";
                    paramUpper = column.Option.Column2ParamContractHandler?.Invoke(paramUpper) ?? paramUpper;
                }
                else throw new ArgumentNullException(nameof(paramUpper));
            }
            return new BetweenExpression(column, new Param(paramLower), new Param(paramUpper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{column.Expression} NOT BETWEEN @{column.Name}Lower AND @{column.Name}Upper</returns>
        public static NotBetweenExpression NotBetweenVarParam(this ISimpleValue column, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (column is IColumn)
                {
                    paramLower = paramLower ?? (column as IColumn).Name + "Lower";
                    paramLower = column.Option.Column2ParamContractHandler?.Invoke(paramLower) ?? paramLower;
                }
                else throw new ArgumentNullException(nameof(paramLower));
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (column is IColumn)
                {
                    paramUpper = paramUpper ?? (column as IColumn).Name + "Upper";
                    paramUpper = column.Option.Column2ParamContractHandler?.Invoke(paramUpper) ?? paramUpper;
                }
                else throw new ArgumentNullException(nameof(paramUpper));
            }
            return new NotBetweenExpression(column, new Param(paramLower), new Param(paramUpper));
        }

        #endregion

        #endregion
    }
}