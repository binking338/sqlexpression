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

        public static BracketExpression WithB<T>(this T exp)
            where T : ISimpleValue
        {
            return WithBracket(exp);
        }

        public static BracketExpression WithBracket<T>(this T exp)
            where T : ISimpleValue
        {
            return new BracketExpression(exp);
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

        public static IParam ToP(this IField field, string param = null)
        {
            return ToParam(field, param);
        }

        public static ISetFieldExpression SetC(this IField field, string customer)
        {
            return SetVarCustomer(field, customer);
        }

        public static ISetFieldExpression SetP(this IField field, string param = null)
        {
            return SetVarParam(field, param);
        }

        #endregion

        public static IParam ToParam(this IField field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = field.Name;
            return new Param(param);
        }

        public static ISetFieldExpression SetVarCustomer(this IField field, string customer)
        {
            return Set(field, new CustomerExpression(customer));
        }

        public static ISetFieldExpression SetVarParam(this IField field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = field.Name;
            return Set(field, field.ToParam(param));
        }

        public static ISetFieldExpression Set(this IField field, object value = null)
        {
            return Set(field, value is ISimpleValue ? value as ISimpleValue : new LiteralValue(value));
        }

        public static ISetFieldExpression Set(this IField field, ISimpleValue value)
        {
            return new SetFieldExpression(field, value);
        }

        public static IOrderExpression Desc(this IField field)
        {
            return new OrderExpression(field, OrderEnum.Desc);
        }

        public static IOrderExpression Asc(this IField field)
        {
            return new OrderExpression(field, OrderEnum.Asc);
        }

        #region 聚合函数 AggregateFunctionExpression

        public static AggregateFunctionExpression Sum(this IField field)
        {
            return AggregateFunctionExpression.Sum(field);
        }

        public static AggregateFunctionExpression Count(this IField field)
        {
            return AggregateFunctionExpression.Count(field);
        }

        public static AggregateFunctionExpression Avg(this IField field)
        {
            return AggregateFunctionExpression.Avg(field);
        }

        public static AggregateFunctionExpression Min(this IField field)
        {
            return AggregateFunctionExpression.Min(field);
        }

        public static AggregateFunctionExpression Max(this IField field)
        {
            return AggregateFunctionExpression.Max(field);
        }

        #endregion

        #region 算术表达式 ArithmeticExpression

        public static ArithmeticExpression Add(this IField field, ISimpleValue value)
        {
            return new ArithmeticExpression(field, Operator.Add, value);
        }

        public static ArithmeticExpression Sub(this IField field, ISimpleValue value)
        {
            return new ArithmeticExpression(field, Operator.Sub, value);
        }

        public static ArithmeticExpression Mul(this IField field, ISimpleValue value)
        {
            return new ArithmeticExpression(field, Operator.Mul, value);
        }

        public static ArithmeticExpression Div(this IField field, ISimpleValue value)
        {
            return new ArithmeticExpression(field, Operator.Div, value);
        }

        public static ArithmeticExpression Mod(this IField field, ISimpleValue value)
        {
            return new ArithmeticExpression(field, Operator.Mod, value);
        }

        #endregion

        #endregion

        #region 过滤表达式 FilterExpression

        #region LogicExpresssion

        public static IBoolValue AllEq(this IEnumerable<IField> fields, IEnumerable<ISimpleValue> values)
        {
            IBoolValue filter = null;
            var em = values.GetEnumerator();
            foreach (var field in fields)
            {
                em.MoveNext();
                filter = filter == null ? field.Eq(em.Current) as IBoolValue : filter.And(field.Eq(em.Current));
            }
            return filter;
        }

        public static IBoolValue AllEq(this IEnumerable<IField> fields, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
            return AllEq(fields, vals);
        }

        public static IBoolValue AllEq(this IEnumerable<IField> fields, params ISimpleValue[] values)
        {
            return AllEq(fields, values.AsEnumerable());
        }

        public static IBoolValue AllEq(this IEnumerable<IField> fields, params object[] values)
        {
            return AllEq(fields, values.AsEnumerable());
        }

        public static IBoolValue AllEqVarParam(this IEnumerable<IField> fields)
        {
            IBoolValue boolValue = null;
            foreach (var field in fields)
            {
                boolValue = boolValue == null ? field.EqVarParam() as IBoolValue : boolValue.And(field.EqVarParam());
            }
            return boolValue;
        }

        public static IBoolValue AnyEq(this IEnumerable<IField> fields, IEnumerable<ISimpleValue> values)
        {
            IBoolValue boolValue = null;
            var em = values.GetEnumerator();
            foreach (var field in fields)
            {
                em.MoveNext();
                boolValue = boolValue == null ? field.Eq(em.Current) as IBoolValue : boolValue.Or(field.Eq(em.Current));
            }
            return boolValue;
        }

        public static IBoolValue AnyEq(this IEnumerable<IField> fields, IEnumerable<object> values)
        {
            var vals = values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val));
            return AnyEq(fields, vals);
        }

        public static IBoolValue AnyEq(this IEnumerable<IField> fields, params ISimpleValue[] values)
        {
            return AnyEq(fields, values.AsEnumerable());
        }

        public static IBoolValue AnyEq(this IEnumerable<IField> fields, params object[] values)
        {
            return AnyEq(fields, values.AsEnumerable());
        }

        public static IBoolValue AnyEqVarParam(this IEnumerable<IField> fields)
        {
            IBoolValue boolValue = null;
            foreach (var field in fields)
            {
                boolValue = boolValue == null ? field.EqVarParam() as IBoolValue : boolValue.Or(field.EqVarParam());
            }
            return boolValue;
        }

        public static LogicExpression And(this ISimpleValue a, ISimpleValue b)
        {
            if (a is ILogicExpression)
            {
                a = WithBracket(a as ILogicExpression);
            }
            if (b is ILogicExpression)
            {
                b = WithBracket(b as ILogicExpression);
            }
            return new LogicExpression(a, LogicOperator.And, b);
        }

        public static LogicExpression Or(this ISimpleValue a, ISimpleValue b)
        {
            if (a is ILogicExpression)
            {
                a = WithBracket(a as ILogicExpression);
            }
            if (b is ILogicExpression)
            {
                b = WithBracket(b as ILogicExpression);
            }
            return new LogicExpression(a, LogicOperator.Or, b);
        }

        #endregion

        #region ComparisonExpression

        public static UnaryComparisonExpression IsNull(this ISimpleValue field)
        {
            return new UnaryComparisonExpression(field, Operator.IsNull);
            //return new ComparisonExpression(field, Operator.Is, new LiteralValueExpression(null));
        }

        public static UnaryComparisonExpression IsNotNull(this ISimpleValue field)
        {
            return new UnaryComparisonExpression(field, Operator.IsNotNull);
            //return new ComparisonExpression(field, Operator.IsNot, new LiteralValueExpression(null));
        }

        public static ComparisonExpression Eq(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Eq, val);
        }

        public static ComparisonExpression Neq(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Neq, val);
        }

        public static ComparisonExpression Gt(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Gt, val);
        }

        public static ComparisonExpression GtOrEq(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.GtOrEq, val);
        }

        public static ComparisonExpression Lt(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Lt, val);
        }

        public static ComparisonExpression LtOrEq(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.LtOrEq, val);
        }

        public static ComparisonExpression Like(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Like, val);
        }

        public static ComparisonExpression NotLike(this ISimpleValue field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.NotLike, val);
        }

        public static ComparisonExpression Eq(this ISimpleValue field, object val)
        {
            return field.Eq(new LiteralValue(val));
        }

        public static ComparisonExpression Neq(this ISimpleValue field, object val)
        {
            return field.Neq(new LiteralValue(val));
        }

        public static ComparisonExpression Gt(this ISimpleValue field, object val)
        {
            return field.Gt(new LiteralValue(val));
        }

        public static ComparisonExpression GtOrEq(this ISimpleValue field, object val)
        {
            return field.GtOrEq(new LiteralValue(val));
        }

        public static ComparisonExpression Lt(this ISimpleValue field, object val)
        {
            return field.Lt(new LiteralValue(val));
        }

        public static ComparisonExpression LtOrEq(this ISimpleValue field, object val)
        {
            return field.LtOrEq(new LiteralValue(val));
        }

        public static ComparisonExpression Like(this ISimpleValue field, object val)
        {
            return field.Like(new LiteralValue(val));
        }

        public static ComparisonExpression NotLike(this ISimpleValue field, object val)
        {
            return field.NotLike(new LiteralValue(val));
        }

        public static BetweenExpression Between(this ISimpleValue field, object a, object b)
        {
            return new BetweenExpression(field, new LiteralValue(a), new LiteralValue(b));
        }

        public static NotBetweenExpression NotBetween(this ISimpleValue field, object a, object b)
        {
            return new NotBetweenExpression(field, new LiteralValue(a), new LiteralValue(b));
        }

        public static InExpression In(this ISimpleValue field, params object[] values)
        {
            return new InExpression(field, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static NotInExpression NotIn(this ISimpleValue field, params object[] values)
        {
            return new NotInExpression(field, new ValueCollectionExpression(values.Select(val => new LiteralValue(val)).ToArray()));
        }

        public static InExpression In(this ISimpleValue field, ISubQueryExpression subquery)
        {
            return new InExpression(field, subquery);
        }

        public static NotInExpression NotIn(this ISimpleValue field, ISubQueryExpression subquery)
        {
            return new NotInExpression(field, subquery);
        }

        public static InExpression In(this ISimpleValue field, ISelectStatement select)
        {
            return new InExpression(field, new SubQueryExpression(select));
        }

        public static NotInExpression NotIn(this ISimpleValue field, ISelectStatement select)
        {
            return new NotInExpression(field, new SubQueryExpression(select));
        }

        public static ComparisonExpression EqVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.Eq(new Param(param));
        }

        public static ComparisonExpression NeqVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.Neq(new Param(param));
        }

        public static ComparisonExpression GtVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.Gt(new Param(param));
        }

        public static ComparisonExpression GtOrEqVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.GtOrEq(new Param(param));
        }

        public static ComparisonExpression LtVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.Lt(new Param(param));
        }

        public static ComparisonExpression LtOrEqVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.LtOrEq(new Param(param));
        }

        public static ComparisonExpression LikeVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.Like(new Param(param));
        }

        public static ComparisonExpression NotLikeVarParam(this ISimpleValue field, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                if (field is IField) param = (field as IField).Name;
                else throw new ArgumentNullException("param");
            }
            return field.NotLike(new Param(param));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{field.Expression} BETWEEN @{field.Name}Lower AND @{field.Name}Upper</returns>
        public static BetweenExpression BetweenVarParam(this ISimpleValue field, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (field is IField) paramLower = (field as IField).Name + "Lower";
                else throw new ArgumentNullException("paramLower");
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (field is IField) paramUpper = (field as IField).Name + "Upper";
                else throw new ArgumentNullException("paramUpper");
            }
            return new BetweenExpression(field, new Param(paramLower), new Param(paramUpper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="paramLower"></param>
        /// <param name="paramUpper"></param>
        /// <returns>Expression:{field.Expression} NOT BETWEEN @{field.Name}Lower AND @{field.Name}Upper</returns>
        public static NotBetweenExpression NotBetweenVarParam(this ISimpleValue field, string paramLower = null, string paramUpper = null)
        {
            if (string.IsNullOrWhiteSpace(paramLower))
            {
                if (field is IField) paramLower = (field as IField).Name + "Lower";
                else throw new ArgumentNullException("paramLower");
            }
            if (string.IsNullOrWhiteSpace(paramUpper))
            {
                if (field is IField) paramUpper = (field as IField).Name + "Upper";
                else throw new ArgumentNullException("paramUpper");
            }
            return new NotBetweenExpression(field, new Param(paramLower), new Param(paramUpper));
        }

        #endregion

        #endregion
    }
}
