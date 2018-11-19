using System;
using System.Collections.Generic;
using System.Text;

namespace SqlExpression.Extension.Dapper
{
    public interface IExpressionMapper<SqlStatement, Entity>
        where SqlStatement : ISqlStatement
    {
         SqlStatement Expression { get; set; }
    }

    public class ExpressionMapper<SqlStatement, Entity> : IExpressionMapper<SqlStatement, Entity>
        where SqlStatement : ISqlStatement
    {
        public SqlStatement Expression { get; set; }

        public override string ToString()
        {
            return Expression?.ToString();
        }
    }

    public static class ExpressionMapperExtensions
    {
        public static ExpressionMapper<ISelectStatement, Entity> Map<Entity>(this ISelectStatement exp, Entity entity)
        {
            return new ExpressionMapper<ISelectStatement, Entity>() { Expression = exp };
        }
        public static ExpressionMapper<ISelectStatement, Entity> Map<Entity>(this ISelectStatement exp)
        {
            return new ExpressionMapper<ISelectStatement, Entity>() { Expression = exp };
        }
        public static ExpressionMapper<IQueryStatement, Entity> Map<Entity>(this IQueryStatement exp, Entity entity)
        {
            return new ExpressionMapper<IQueryStatement, Entity>() { Expression = exp };
        }
        public static ExpressionMapper<IQueryStatement, Entity> Map<Entity>(this IQueryStatement exp)
        {
            return new ExpressionMapper<IQueryStatement, Entity>() { Expression = exp };
        }
    }
}
