
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression.Extension
{
    public static class DeleteExpressionExtensions
    {
        public static DeleteStatement Delete(this ITable table)
        {
            return new DeleteStatement(table, null);
        }

        public static IDeleteStatement From(this IDeleteStatement delete, ITable table)
        {
            delete.Table = table;
            return delete;
        }

        public static IDeleteStatement Where(this IDeleteStatement delete, ISimpleValue filter)
        {
            delete.Where = new WhereClause(filter);
            return delete;
        }

        public static IDeleteStatement WhereC(this IDeleteStatement delete, string customFilter)
        {
            return WhereVarCustom(delete, customFilter);
        }

        public static IDeleteStatement WhereVarCustom(this IDeleteStatement delete, string customFilter)
        {
            return Where(delete, new CustomExpression(customFilter));
        }
    }
}
