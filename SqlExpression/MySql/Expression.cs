using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.MySql
{
    public class LimitClause : ExpressionBase, ILimitClause
    {
        public LimitClause(int count)
            : this(0, count)
        { }

        public LimitClause(int offset, int count)
        {
            if (offset < 0) offset = 0;
            if (count < 1) count = 1;
            _offset = offset == 0 ? null : new LiteralValueExpression(offset);
            _count = new LiteralValueExpression(count);
        }

        public LimitClause(IValueExpression count)
            : this(null, count)
        { }

        public LimitClause(IValueExpression offset, IValueExpression count)
        {
            _offset = offset;
            _count = count;
            GenExpression();
        }

        private IValueExpression _offset;
        private IValueExpression _count;

        public IValueExpression Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
                GenExpression();
            }
        }

        public IValueExpression Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Count == null)
            {
                Expression = string.Empty;
            }
            if (Offset == null)
            {
                Expression = string.Format("LIMIT {0}", Count?.Expression);
            }
            else
            {
                Expression = string.Format("LIMIT {0},{1}", Offset?.Expression, Count?.Expression);
            }
        }
    }

    public class MySqlSelectStatement : SelectStatement, IMySqlSelectStatement
    {
        public MySqlSelectStatement()
            : base()
        { }

        public MySqlSelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IJoinExpression[] joins, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
            : base(tables, items, joins, where, groupby, having, orderby)
        {
        }

        public MySqlSelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
            : base(tables, items, null, where, groupby, having, orderby)
        {
        }

        public MySqlSelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IWhereClause where, IOrderByClause orderby = null)
            : base(tables, items, where, null, null, orderby)
        {

        }

        private ILimitClause _limit = null;

        public ILimitClause Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Tables == null || Tables.Length == 0 || Items == null || Items.Length == 0)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("SELECT {1} FROM {7} {0} {2} {3} {4} {5} {6}", Tables?.Join(",", t => t.Expression),
                    Items?.Join(",", s => s.Expression),
                    Where?.Expression,
                    GroupBy?.Expression,
                    string.IsNullOrWhiteSpace(GroupBy?.Expression) ? string.Empty : Having?.Expression,
                    OrderBy?.Expression,
                    Limit?.Expression,
                    Joins?.Join(" ", j => j?.Expression)).TrimEnd();
            }
        }
    }

    public class MySqlUpdateStatement : UpdateStatement, IMySqlUpdateStatement
    {
        public MySqlUpdateStatement()
            : base()
        {

        }

        public MySqlUpdateStatement(ITableExpression table, ISetClause set, IWhereClause where) 
            : base(table, set, where)
        {
        }

        private ILimitClause _limit = null;
        private IOrderByClause _orderBy = null;

        public ILimitClause Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value;
                GenExpression();
            }
        }

        public IOrderByClause OrderBy
        {
            get
            {
                return _orderBy;
            }
            set
            {
                _orderBy = value;
                GenExpression();
            }
        }
        
        protected override void GenExpression()
        {
            if (Table == null || Set == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("UPDATE {0} {1} {2} {3} {4}", 
                    Table?.Expression, 
                    Set?.Expression, 
                    Where?.Expression,
                    OrderBy?.Expression,
                    Limit?.Expression).TrimEnd();
            }
        }
    }

    public class MySqlDeleteStatement : DeleteStatement, IMySqlDeleteStatement
    {
        public MySqlDeleteStatement()
            : base()
        {

        }

        public MySqlDeleteStatement(ITableExpression table, IWhereClause where) 
            : base(table, where)
        {
        }

        private ILimitClause _limit = null;
        private IOrderByClause _orderBy = null;

        public ILimitClause Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value;
                GenExpression();
            }
        }

        public IOrderByClause OrderBy
        {
            get
            {
                return _orderBy;
            }
            set
            {
                _orderBy = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Table == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("DELETE FROM {0} {1} {2} {3}", 
                    Table?.Expression, 
                    Where?.Expression, 
                    OrderBy?.Expression, 
                    Limit?.Expression).TrimEnd();
            }
        }
    }
}
