using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression
{
    /// <summary>
    /// SQL语法错误
    /// </summary>
    public class SqlSyntaxException : Exception
    {

        public SqlSyntaxException(IExpression expression, string message)
            : base(message)
        {
            Expression = expression;
        }

        /// <summary>
        /// 
        /// </summary>
        public IExpression Expression { get; set; }

    }
}
