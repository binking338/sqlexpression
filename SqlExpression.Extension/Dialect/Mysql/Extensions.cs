using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.Extension.Dialect.Mysql
{
    public static class Config
    {
        public static void EnableDialect()
        {
            Expression.DefaultOption.OpenQuotationMark = "`";
            Expression.DefaultOption.CloseQuotationMark = "`";
        }

        public static void DisableDialect()
        {
            Expression.DefaultOption.OpenQuotationMark = string.Empty;
            Expression.DefaultOption.CloseQuotationMark = string.Empty;
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="select"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static LimitSelectStatement Limit(this ISelectStatement select, int limit)
        {
            return new LimitSelectStatement(select.Query, select.OrderBy, new LimitExpression(0, limit));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="select"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static LimitSelectStatement Limit(this ISelectStatement select, int offset, int limit)
        {
            return new LimitSelectStatement(select.Query, select.OrderBy, new LimitExpression(offset, limit));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="select"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static LimitSelectStatement Page(this ISelectStatement select, int pageindex, int pagesize)
        {
            return new LimitSelectStatement(select.Query, select.OrderBy, new LimitExpression((pageindex - 1) * pagesize, pagesize));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static LimitSimpleQueryStatement Limit(this ISimpleQueryStatement query, int limit)
        {
            return new LimitSimpleQueryStatement(query.Select, query.From, query.Where, query.GroupBy, new LimitExpression(0, limit));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static LimitSimpleQueryStatement Limit(this ISimpleQueryStatement query, int offset, int limit)
        {
            return new LimitSimpleQueryStatement(query.Select, query.From, query.Where, query.GroupBy, new LimitExpression(offset, limit));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static LimitSimpleQueryStatement Page(this ISimpleQueryStatement query, int pagesize, int pageindex)
        {
            return new LimitSimpleQueryStatement(query.Select, query.From, query.Where, query.GroupBy, new LimitExpression((pageindex - 1) * pagesize, pagesize));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static LimitUnionQueryStatement Limit(this IUnionQueryStatement query, int limit)
        {
            return new LimitUnionQueryStatement(query.Query1, query.UnionOp, query.Query2, new LimitExpression(0, limit));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static LimitUnionQueryStatement Limit(this IUnionQueryStatement query, int offset, int limit)
        {
            return new LimitUnionQueryStatement(query.Query1, query.UnionOp, query.Query2, new LimitExpression(offset, limit));
        }

        /// <summary>
        /// 返回分页sql语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static LimitUnionQueryStatement Page(this IUnionQueryStatement query, int pageindex, int pagesize)
        {
            return new LimitUnionQueryStatement(query.Query1, query.UnionOp, query.Query2, new LimitExpression((pageindex - 1) * pagesize, pagesize));
        }

        /// <summary>
        /// 返回插入的自增Key
        /// </summary>
        /// <returns>The identifier.</returns>
        /// <param name="insert">Insert.</param>
        public static BatchSqlStatement ReturnId(this IInsertStatement insert)
        {
            return new BatchSqlStatement(new List<ISqlStatement>() { insert, new CustomExpression("SELECT LAST_INSERT_ID()") });
        }

        /// <summary>
        /// 转换gbk编码
        /// </summary>
        private static UnaryOperator ConvertGbkOperator = new UnaryOperator("CONVERT_GBK", "CONVERT({0} USING gbk)");

        /// <summary>
        /// gbk中文拼音升序
        /// </summary>
        /// <returns></returns>
        /// <param name="column">Column.</param>
        public static OrderExpression AscGBK(this IColumn column)
        {
            return new OrderExpression(new UnaryExpression(ConvertGbkOperator, column), OrderEnum.Asc);
        }

        /// <summary>
        /// gbk中文拼音降序
        /// </summary>
        /// <returns>The gbk.</returns>
        /// <param name="column">Column.</param>
        public static OrderExpression DescGBK(this IColumn column)
        {
            return new OrderExpression(new UnaryExpression(ConvertGbkOperator, column), OrderEnum.Desc);
        }

        /// <summary>
        /// Limit expression.
        /// </summary>
        public class LimitExpression : Expression
        {
            public LimitExpression(int limit) : this(0, limit) { }
            public LimitExpression(int offset, int limit)
            {
                Offset = offset;
                Limit = limit;
            }

            /// <summary>
            /// Gets or sets the limit.
            /// </summary>
            /// <value>The limit.</value>
            public int Limit { get; set; }
            /// <summary>
            /// Gets or sets the offset.
            /// </summary>
            /// <value>The offset.</value>
            public int Offset { get; set; }

            protected override string Build()
            {
                if (Offset == 0)
                {
                    return string.Format("LIMIT {0}", Limit);
                }
                else
                {
                    return string.Format("LIMIT {0},{1}", Offset, Limit);
                }
            }
        }

        /// <summary>
        /// Limit select statement.
        /// </summary>
        public class LimitSelectStatement : SelectStatement
        {
            public LimitSelectStatement(IQueryStatement query) : this(query, null) { }
            public LimitSelectStatement(IQueryStatement query, IOrderByClause orderBy) : this(query, orderBy, null) { }
            public LimitSelectStatement(IQueryStatement query, IOrderByClause orderBy, LimitExpression limit) : base(query, orderBy)
            {
                Limit = limit;
            }

            /// <summary>
            /// Gets or sets the limit.
            /// </summary>
            /// <value>The limit.</value>
            public LimitExpression Limit { get; set; }

            protected override string Build()
            {
                if (Limit == null)
                {
                    return base.Build();
                }
                else
                {
                    return string.Format("{0} {1}", base.Build(), Limit.ToString());
                }
            }
        }

        /// <summary>
        /// Limit simple query statement.
        /// </summary>
        public class LimitSimpleQueryStatement : SimpleQueryStatement
        {
            public LimitSimpleQueryStatement(ISelectClause select) : this(select, null, null, null) { }
            public LimitSimpleQueryStatement(ISelectClause select, IFromClause from) : this(select, from, null, null) { }
            public LimitSimpleQueryStatement(ISelectClause select, IFromClause from, IWhereClause where) : this(select, from, where, null) { }
            public LimitSimpleQueryStatement(ISelectClause select, IFromClause from, IGroupByClause groupBy) : this(select, from, null, groupBy) { }
            public LimitSimpleQueryStatement(ISelectClause select, IFromClause from, IWhereClause where, IGroupByClause groupBy) : this(select, from, where, groupBy, null) { }
            public LimitSimpleQueryStatement(ISelectClause select, IFromClause from, IWhereClause where, IGroupByClause groupBy, LimitExpression limit) : base(select, from, where, groupBy)
            {
                Limit = limit;
            }

            /// <summary>
            /// Gets or sets the limit.
            /// </summary>
            /// <value>The limit.</value>
            public LimitExpression Limit { get; set; }

            protected override string Build()
            {
                if (Limit == null)
                {
                    return base.Build();
                }
                else
                {
                    return string.Format("{0} {1}", base.Build(), Limit.ToString());
                }
            }
        }

        /// <summary>
        /// Limit union query statement.
        /// </summary>
        public class LimitUnionQueryStatement : UnionQueryStatement
        {
            public LimitUnionQueryStatement(IQueryStatement query1, IUnionOperator unionOp, ISimpleQueryStatement query2) : this(query1, unionOp, query2, null) { }
            public LimitUnionQueryStatement(IQueryStatement query1, IUnionOperator unionOp, ISimpleQueryStatement query2, LimitExpression limit) : base(query1, unionOp, query2)
            {
                Limit = limit;
            }

            /// <summary>
            /// Gets or sets the limit.
            /// </summary>
            /// <value>The limit.</value>
            public LimitExpression Limit { get; set; }

            protected override string Build()
            {
                if (Limit == null)
                {
                    return base.Build();
                }
                else
                {
                    return string.Format("{0} {1}", base.Build(), Limit.ToString());
                }
            }
        }

    }
}
