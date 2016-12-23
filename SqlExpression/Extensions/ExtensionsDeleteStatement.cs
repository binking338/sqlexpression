using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsDeleteExpression
    {
        #region Delete

        #region ShortCut

        #endregion

        public static DeleteStatement Delete(this ITableExpression table)
        {
            return new DeleteStatement(table, null);
        }

        public static IDeleteStatement Where(this IDeleteStatement delete, IFilterExpression filter)
        {
            delete.Where = new WhereClause(filter);
            return delete;
        }

        #region OrderBy

        public static IDeleteStatement OrderBy(this IDeleteStatement delete, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(delete, orderby);
        }
        public static IDeleteStatement OrderBy(this IDeleteStatement delete, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(delete, orderby);
        }

        public static IDeleteStatement OrderBy(this IDeleteStatement delete, IOrderByClause orderby)
        {
            delete.OrderBy = orderby;
            return delete;
        }

        #endregion

        #region Limit

        public static IDeleteStatement Limit(this IDeleteStatement delete, int count)
        {
            delete.Limit = new LimitClause(0, count);
            return delete;
        }

        public static IDeleteStatement Limit(this IDeleteStatement delete, IValueExpression count)
        {
            delete.Limit = new LimitClause(null, count);
            return delete;
        }

        #endregion
        
        #endregion
    }
}
