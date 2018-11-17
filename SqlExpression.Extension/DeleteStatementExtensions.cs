
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression.Extension
{
    public static class DeleteStatementExtensions
    {
        public static DeleteStatement Delete(this ITableFilterExpression tableFilter)
        {
            return new DeleteStatement(tableFilter.Table, tableFilter.Where);
        }

        public static DeleteStatement Delete(this ITable table)
        {
            return new DeleteStatement(table, null);
        }

        public static IDeleteStatement From(this IDeleteStatement delete, ITable table)
        {
            delete.Table = table;
            return delete;
        }

        #region ShortCut

        public static IDeleteStatement WhereC(this IDeleteStatement delete, string customFilter)
        {
            return WhereVarCustom(delete, customFilter);
        }

        public static IDeleteStatement OrWhereC(this IDeleteStatement delete, string customFilter)
        {
            return OrWhereVarCustom(delete, customFilter);
        }

        #endregion

        public static IDeleteStatement Where(this IDeleteStatement delete, ISimpleValue filter)
        {
            if (delete.Where == null)
            {
                delete.Where = new WhereClause(filter);
            }
            else
            {
                delete.Where.Filter = new LogicExpression(delete.Where.Filter, Operator.And, filter);
            }
            return delete;
        }

        public static IDeleteStatement WhereVarCustom(this IDeleteStatement delete, string customFilter)
        {
            return Where(delete, new CustomExpression(customFilter));
        }

        public static IDeleteStatement OrWhere(this IDeleteStatement delete, ISimpleValue filter)
        {
            if (delete.Where == null)
            {
                delete.Where = new WhereClause(filter);
            }
            else
            {
                delete.Where.Filter = new LogicExpression(delete.Where.Filter, Operator.Or, filter);
            }
            return delete;
        }

        public static IDeleteStatement OrWhereVarCustom(this IDeleteStatement delete, string customFilter)
        {
            return OrWhere(delete, new CustomExpression(customFilter));
        }
    }
}
