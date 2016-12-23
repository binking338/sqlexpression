using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.MySql
{
    /// <summary>
    /// 限制行数子句（MySql）
    /// </summary>
    public interface ILimitClause : IExpression
    {
        /// <summary>
        /// 偏移
        /// </summary>
        IValueExpression Offset { get; set; }

        /// <summary>
        /// 限制数量
        /// </summary>
        IValueExpression Count { get; set; }
    }

    /// <summary>
    /// select语句（MySql）
    /// </summary>
    public interface IMySqlSelectStatement : ISelectStatement
    {
        /// <summary>
        /// 限制行数(MySql)
        /// </summary>
        ILimitClause Limit { get; set; }
    }

    /// <summary>
    /// update语句（MySql）
    /// </summary>
    public interface IMySqlUpdateStatement : IUpdateStatement
    {
        /// <summary>
        /// 排序方式(MySql)
        /// </summary>
        IOrderByClause OrderBy { get; set; }

        /// <summary>
        /// 限制行数(MySql)
        /// </summary>
        ILimitClause Limit { get; set; }
    }

    /// <summary>
    /// delete语句（MySql）
    /// </summary>
    public interface IMySqlDeleteStatement:IDeleteStatement
    {
        /// <summary>
        /// 排序方式(MySql)
        /// </summary>
        IOrderByClause OrderBy { get; set; }

        /// <summary>
        /// 限制行数(MySql)
        /// </summary>
        ILimitClause Limit { get; set; }
    }
}
