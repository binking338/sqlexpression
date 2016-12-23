using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression
{
    public delegate string ExpressionHandler(string name);

    public static class Config
    {
        public static ExpressionHandler TableExpressionHandler = null;
        public static ExpressionHandler PropertyExpressionHandler = null;
        public static ExpressionHandler ParamExpressionHandler = null;

    }
}
