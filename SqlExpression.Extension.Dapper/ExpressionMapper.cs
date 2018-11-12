using System;
using System.Collections.Generic;
using System.Text;

namespace SqlExpression.Extension.Dapper
{
    public interface IExpressionMapper<SqlExpression, Entity>
        where SqlExpression : ISqlStatement
    {
         SqlExpression Expression { get; set; }
    }

    public class ExpressionMapper<SqlExpression, Entity> : IExpressionMapper<SqlExpression, Entity>
        where SqlExpression : ISqlStatement
    {
        public SqlExpression Expression { get; set; }

        public override string ToString()
        {
            return Expression?.ToString();
        }
    }

    public static class ExpressionMapperExtensions
    {
        public static ExpressionMapper<SqlExpression, Entity> Map<SqlExpression, Entity>(this SqlExpression exp, Entity entity)
            where SqlExpression : ISqlStatement
        {
            return new ExpressionMapper<SqlExpression, Entity>() { Expression = exp };
        }
    }
}
