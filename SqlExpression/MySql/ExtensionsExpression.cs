using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.MySql
{
    public static class ExtensionsExpression
    {
        #region Update OrderBy

        public static IUpdateStatement OrderBy(this IMySqlUpdateStatement update, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(update, orderby);
        }
        public static IUpdateStatement OrderBy(this IMySqlUpdateStatement update, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(update, orderby);
        }

        public static IUpdateStatement OrderBy(this IMySqlUpdateStatement update, IOrderByClause orderby)
        {
            update.OrderBy = orderby;
            return update;
        }

        #endregion

        #region Delete OrderBy

        public static IDeleteStatement OrderBy(this IMySqlDeleteStatement delete, IEnumerable<IOrderExpression> orders)
        {
            var orderby = new OrderByClause(orders.ToArray());
            return OrderBy(delete, orderby);
        }
        public static IDeleteStatement OrderBy(this IMySqlDeleteStatement delete, params IOrderExpression[] orders)
        {
            var orderby = new OrderByClause(orders);
            return OrderBy(delete, orderby);
        }

        public static IDeleteStatement OrderBy(this IMySqlDeleteStatement delete, IOrderByClause orderby)
        {
            delete.OrderBy = orderby;
            return delete;
        }

        #endregion

        #region Update Limit

        public static IUpdateStatement Limit(this IMySqlUpdateStatement update, int count)
        {
            update.Limit = new LimitClause(0, count);
            return update;
        }

        public static IUpdateStatement Limit(this IMySqlUpdateStatement update, IValueExpression count)
        {
            update.Limit = new LimitClause(null, count);
            return update;
        }

        #endregion

        #region Delete Limit

        public static IDeleteStatement Limit(this IMySqlDeleteStatement delete, int count)
        {
            delete.Limit = new LimitClause(0, count);
            return delete;
        }

        public static IDeleteStatement Limit(this IMySqlDeleteStatement delete, IValueExpression count)
        {
            delete.Limit = new LimitClause(null, count);
            return delete;
        }

        #endregion

        #region Select Limit

        public static ISelectStatement Limit(this IMySqlSelectStatement select, int count)
        {
            select.Limit = new LimitClause(0, count);
            return select;
        }

        public static ISelectStatement Limit(this IMySqlSelectStatement select, int offset, int count)
        {
            select.Limit = new LimitClause(offset, count);
            return select;
        }

        public static ISelectStatement Limit(this IMySqlSelectStatement select, IValueExpression count)
        {
            select.Limit = new LimitClause(null, count);
            return select;
        }

        public static ISelectStatement Limit(this IMySqlSelectStatement select, IValueExpression offset, IValueExpression count)
        {
            select.Limit = new LimitClause(offset, count);
            return select;
        }

        public static ISelectStatement Limit(this IMySqlSelectStatement select, ILimitClause limit)
        {
            select.Limit = limit;
            return select;
        }

        public static ISelectStatement Page(this IMySqlSelectStatement select, int pageindex, int pagesize)
        {
            select.Limit = new LimitClause(pagesize * (pageindex - 1), pagesize);
            return select;
        }

        #endregion
    }
}
