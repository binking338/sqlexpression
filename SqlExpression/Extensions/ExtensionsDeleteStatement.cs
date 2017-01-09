using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsDeleteExpression
    {
        #region Shortcut

        public static IDeleteStatement F(this IDeleteStatement delete, ITableExpression table)
        {
            return From(delete, table);
        }
        public static IDeleteStatement F(this IDeleteStatement delete, TableExpression table)
        {
            return From(delete, table);
        }

        public static IDeleteStatement W(this IDeleteStatement delete, IFilterExpression filter)
        {
            return Where(delete, filter);
        }

        #endregion

        public static DeleteStatement Delete(this ITableExpression table)
        {
            return new DeleteStatement(table, null);
        }

        public static IDeleteStatement From(this IDeleteStatement delete, ITableExpression table)
        {
            delete.Table = table;
            return delete;
        }

        public static IDeleteStatement From(this IDeleteStatement delete, TableExpression table)
        {
            delete.Table = table;
            return delete;
        }

        public static IDeleteStatement Where(this IDeleteStatement delete, IFilterExpression filter)
        {
            delete.Where = new WhereClause(filter);
            return delete;
        }
    }
}
