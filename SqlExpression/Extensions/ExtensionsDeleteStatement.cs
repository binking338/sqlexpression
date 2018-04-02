using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsDeleteExpression
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

        public static IDeleteStatement From(this IDeleteStatement delete, Table table)
        {
            delete.Table = table;
            return delete;
        }

        public static IDeleteStatement Where(this IDeleteStatement delete, ISimpleValue filter)
        {
            delete.Where = new WhereClause(filter);
            return delete;
        }
    }
}
