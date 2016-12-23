using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlExpression
{
    public enum ExpressionType
    {
        MySql = 1
    }

    /// <summary>
    /// 表达式抽象类
    /// </summary>
    public abstract class ExpressionBase : IExpression
    {
        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression
        {
            get;
            protected set;
        }

        /// <summary>
        /// Sql语句类型
        /// </summary>
        public ExpressionType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 构建表达式
        /// </summary>
        protected abstract void GenExpression();

        public override string ToString()
        {
            return Expression;
        }
    }

    public class TableExpression : ExpressionBase, ITableExpression
    {
        public TableExpression(string name)
        {
            Name = name;
        }

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            switch (Type)
            {
                case ExpressionType.MySql:
                    Expression = string.IsNullOrWhiteSpace(Name) ? string.Empty : string.Format("`{0}`", Name);
                    break;
                default:
                    Expression = string.IsNullOrWhiteSpace(Name) ? string.Empty : string.Format("{0}", Name);
                    break;
            }
        }

        public static implicit operator TableExpression(string name)
        {
            return new TableExpression(name);
        }
    }

    public class PropertyExpression : ExpressionBase, IPropertyExpression
    {
        public PropertyExpression(string name, ITableExpression table = null)
        {
            _name = name;
            _table = table;
            GenExpression();
        }

        private string _name = null;
        private ITableExpression _table = null;

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                GenExpression();
            }
        }

        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Expression = string.Empty;
            }
            else
            {
                switch (Type)
                {
                    case ExpressionType.MySql:
                        Expression = (new string[] {
                            Table?.Expression,
                            string.IsNullOrWhiteSpace(Name) ? string.Empty : string.Format("`{0}`", Name)
                        }).Where(s => !string.IsNullOrWhiteSpace(s)).Join(".");
                        break;
                    default:
                        Expression = string.IsNullOrWhiteSpace(Name) ? string.Empty : string.Format("{0}", Name);
                        break;
                }
            }
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ComparisonExpression(prop, Operator.LtOrEq, val);
        }

        #endregion

        #region 算术运算符
        public static ArithmeticExpression operator +(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(PropertyExpression prop, ISelectableValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Mod, val);
        }
        public static ArithmeticExpression operator +(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(PropertyExpression prop, LiteralValueExpression val)
        {
            return new ArithmeticExpression(prop, Operator.Mod, val);
        }

        #endregion

        #region 隐式转换

        public static implicit operator PropertyExpression(string prop)
        {
            return new PropertyExpression(prop);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is PropertyExpression)
            {
                return (obj as PropertyExpression).Name == this.Name;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.Name?.GetHashCode() ?? 0;
        }
    }

    public class LiteralValueExpression : ExpressionBase, ILiteralValueExpression
    {
        public LiteralValueExpression(object value)
        {
            if (value is LiteralValueExpression)
            {
                Value = (value as LiteralValueExpression).Value;
            }
            else if (value is IExpression)
            {
                throw new ArgumentException("value");
            }
            else
            {
                Value = value;
            }
        }

        private object _value = null;

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            var valueType = Value?.GetType();
            if (Value == null || Value is DBNull)
            {
                Expression = " NULL ";
            }
            else if (valueType == typeof(string))
            {
                Expression = string.Format("'{0}'", Value);
            }
            else if (valueType == typeof(DateTime))
            {
                Expression = string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", Value);
            }
            else if (valueType.IsEnum)
            {
                Expression = Convert.ToInt32(Value).ToString();
            }
            else
            {
                Expression = Value?.ToString();
            }
        }

        #region 隐式转换

        public static implicit operator LiteralValueExpression(string value)
        {
            return new LiteralValueExpression(value);
        }

        public static implicit operator LiteralValueExpression(DateTime value)
        {
            return new LiteralValueExpression(value);
        }

        public static implicit operator LiteralValueExpression(int value)
        {
            return new LiteralValueExpression(value);
        }

        public static implicit operator LiteralValueExpression(long value)
        {
            return new LiteralValueExpression(value);
        }

        public static implicit operator LiteralValueExpression(decimal value)
        {
            return new LiteralValueExpression(value);
        }

        public static implicit operator LiteralValueExpression(double value)
        {
            return new LiteralValueExpression(value);
        }

        public static implicit operator LiteralValueExpression(float value)
        {
            return new LiteralValueExpression(value);
        }

        #endregion
    }

    public class ParamExpression : ExpressionBase, IParamExpression
    {
        public ParamExpression(string param)
        {
            Name = param;
        }

        private string _paramName = null;
        public string Name
        {
            get
            {
                return _paramName;
            }
            set
            {
                _paramName = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            switch (Type)
            {
                case ExpressionType.MySql:
                    Expression = string.IsNullOrWhiteSpace(Name) ? string.Empty : string.Format("@{0}", Name);
                    break;
                default:
                    Expression = string.IsNullOrWhiteSpace(Name) ? string.Empty : string.Format("@{0}", Name);
                    break;
            }
        }

        #region 隐式转换

        public static implicit operator ParamExpression(string param)
        {
            return new ParamExpression(param);
        }

        public static implicit operator ParamExpression(PropertyExpression prop)
        {
            return new ParamExpression(prop.Name);
        }

        #endregion
    }

    public class CollectionExpression : ExpressionBase, IValueExpression
    {
        public CollectionExpression(params ILiteralValueExpression[] values)
        {
            Values = values;
        }

        private ILiteralValueExpression[] _values = null;

        public ILiteralValueExpression[] Values
        {
            get
            {
                return _values;
            }

            set
            {
                _values = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Values == null || Values.Length == 0)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("({0})", _values.Join(",", exp => exp.Expression));
            }
        }

        #region 隐式转换

        public static implicit operator CollectionExpression(object[] values)
        {
            return new CollectionExpression(values.Select(v => new LiteralValueExpression(v)).ToArray());
        }

        #endregion
    }

    public class BetweenValueExpression : ExpressionBase, IValueExpression
    {
        public BetweenValueExpression(IValueExpression min, IValueExpression max)
        {
            _min = min;
            _max = max;
            GenExpression();
        }

        private IValueExpression _min;
        private IValueExpression _max;

        public IValueExpression Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
                GenExpression();
            }
        }
        public IValueExpression Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Min == null || Max == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0} AND {1}", Min.Expression, Max.Expression);
            }
        }
    }

    public class UnaryExpression : ExpressionBase, IUnaryExpression
    {
        public UnaryExpression(IExpression a, IUnaryOperator op)
        {
            _a = a;
            _op = op;
            GenExpression();
        }

        private IExpression _a = null;
        private IUnaryOperator _op = null;

        /// <summary>
        /// 属性名称
        /// </summary>
        public IExpression A { get { return _a; } set { _a = value; GenExpression(); } }

        /// <summary>
        /// 操作符
        /// </summary>
        public IUnaryOperator Op { get { return _op; } set { _op = value; GenExpression(); } }

        protected override void GenExpression()
        {
            if (A == null || Op == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0}{1}", A?.Expression, Op);
            }
        }
    }

    public class BinaryExpression : ExpressionBase, IBinaryExpression
    {

        public BinaryExpression(IExpression a, IBinaryOperator op, IExpression b)
        {
            _a = a;
            _op = op;
            _b = b;
            GenExpression();
        }

        private IExpression _a = null;
        /// <summary>
        /// 属性名称
        /// </summary>
        public IExpression A { get { return _a; } set { _a = value; GenExpression(); } }

        private IBinaryOperator _op = null;
        /// <summary>
        /// 操作符
        /// </summary>
        public IBinaryOperator Op { get { return _op; } set { _op = value; GenExpression(); } }

        private IExpression _b = null;
        /// <summary>
        /// 值
        /// </summary>
        public IExpression B { get { return _b; } set { _b = value; GenExpression(); } }

        protected override void GenExpression()
        {
            if (A == null || B == null || Op == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0}{1}{2}", A?.Expression, Op, B?.Expression);
            }
        }
    }

    public class ArithmeticExpression : BinaryExpression, IArithmeticExpression
    {
        public ArithmeticExpression(IValueExpression a, IArithmeticOperator op, IValueExpression b)
            : base(a, op, b)
        {
        }
    }

    public class FunctionExpression : ExpressionBase, IFunctionExpression
    {
        public static AggregateFunctionExpression Count(IValueExpression prop)
        {
            return new AggregateFunctionExpression("COUNT", prop);
        }

        public static AggregateFunctionExpression Sum(IValueExpression prop)
        {
            return new AggregateFunctionExpression("SUM", prop);
        }

        public static AggregateFunctionExpression Avg(IValueExpression prop)
        {
            return new AggregateFunctionExpression("AVG", prop);
        }

        public static AggregateFunctionExpression Max(IValueExpression prop)
        {
            return new AggregateFunctionExpression("MAX", prop);
        }

        public static AggregateFunctionExpression Min(IValueExpression prop)
        {
            return new AggregateFunctionExpression("MIN", prop);
        }

        public static FunctionExpression Exists(ISelectStatement sql)
        {
            return new FunctionExpression("EXISTS", sql);
        }

        public static FunctionExpression NotExists(ISelectStatement sql)
        {
            return new FunctionExpression("NOT EXISTS", sql);
        }

        public FunctionExpression(string name, params IValueExpression[] values)
        {
            _name = name;
            _values = values;
            GenExpression();
        }

        private string _name;
        private IValueExpression[] _values;


        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                GenExpression();
            }
        }

        public IValueExpression[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
                GenExpression();
            }
        }

        public object Value
        {
            get
            {
                return Expression;
            }
            set
            {
                ;
            }
        }

        protected override void GenExpression()
        {
            if (Name == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0}({1})", Name, Values.Join(",", v => v?.Expression));
            }
        }
    }

    public class AggregateFunctionExpression : FunctionExpression, IAggregateFunctionExpression
    {
        public AggregateFunctionExpression(string name, IValueExpression value)
            : base(name, value)
        {

        }
    }

    public class BatchSqlStatement : ExpressionBase, IBatchSqlStatement
    {
        public BatchSqlStatement(params ISqlStatement[] sqls)
        {
            var list = new List<ISqlStatement>();
            foreach (var sql in sqls)
            {
                if (sql is IBatchSqlStatement)
                {
                    list.AddRange((sql as IBatchSqlStatement).Sqls);
                }
                else
                {
                    list.Add(sql);
                }
            }
            _sqls = list.ToArray();
            GenExpression();
        }

        private ISqlStatement[] _sqls = null;
        public ISqlStatement[] Sqls
        {
            get
            {
                return _sqls;
            }
            set
            {
                _sqls = value;
                GenExpression();
            }
        }

        public IEnumerable<string> Params
        {
            get
            {
                IEnumerable<string> list = new List<string>();
                foreach (var sql in Sqls)
                {
                    list = list.Concat(sql.Params);
                }
                return list.Distinct();
            }
        }

        protected override void GenExpression()
        {
            if (Sqls == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = Sqls.Where(sql => !string.IsNullOrWhiteSpace(sql?.Expression)).Join(";", sql => sql.Expression);
            }
        }
    }

    public class UnionStatement : ExpressionBase, IUnionStatement
    {
        public UnionStatement(ISelectStatement select, IUnionItemExpression[] unionItems, IOrderByClause orderBy)
        {
            _select = select;
            _unionItems = unionItems;
            _orderBy = orderBy;
            GenExpression();
        }

        private ISelectStatement _select;
        private IUnionItemExpression[] _unionItems;
        private IOrderByClause _orderBy;

        public ISelectStatement Select
        {
            get
            {
                return _select;
            }
            set
            {
                _select = value;
                GenExpression();
            }
        }

        public IUnionItemExpression[] UnionItems
        {
            get
            {
                return _unionItems;
            }
            set
            {
                _unionItems = value;
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

        public IEnumerable<string> Params
        {
            get
            {
                var list = Select.Params;
                foreach (var item in UnionItems)
                {
                    list = list.Concat(item.Select.Params);
                }
                return list.Distinct();
            }
        }

        protected override void GenExpression()
        {
            if (Select == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = (new string[] {
                    Select.Expression,
                    UnionItems?.Join(" ", union => union.Expression),
                    OrderBy?.Expression
                }).Where(s => !string.IsNullOrWhiteSpace(s)).Join(" ");
            }
        }
    }

    public class UnionItemExpression : ExpressionBase, IUnionItemExpression
    {
        public UnionItemExpression(IUnionOperator unionOp, ISelectStatement select)
        {
            _unionOp = unionOp;
            _select = select;
            GenExpression();
        }

        private IUnionOperator _unionOp;
        private ISelectStatement _select;

        public IUnionOperator UnionOp
        {
            get
            {
                return _unionOp;
            }
            set
            {
                _unionOp = value;
                GenExpression();
            }
        }

        public ISelectStatement Select
        {
            get
            {
                return _select;
            }
            set
            {
                _select = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Select == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0}{1}", UnionOp, Select?.Expression);
            }
        }
    }

    public class InsertStatement : ExpressionBase, IInsertStatement
    {
        public InsertStatement() { }

        public InsertStatement(ITableExpression table, IPropertyExpression[] properties, IValueExpression[] values)
        {
            _table = table;
            _properties = properties;
            _values = values;
            GenExpression();
        }

        private ITableExpression _table = null;
        private IPropertyExpression[] _properties = null;
        private IValueExpression[] _values = null;


        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }

        public IPropertyExpression[] Properties
        {
            get
            {
                return _properties;
            }

            set
            {
                _properties = value;
                GenExpression();
            }
        }

        public IValueExpression[] Values
        {
            get
            {
                return _values;
            }

            set
            {
                _values = value;
                GenExpression();
            }
        }

        public IEnumerable<string> Params
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var val in Values)
                {
                    if (val is ICustomerExpression) list.AddRange((val as ICustomerExpression).Params);
                    else if (val is IParamExpression) list.Add((val as IParamExpression).Name);
                }
                return list.Distinct();
            }
        }

        protected override void GenExpression()
        {
            if (Table == null || Properties == null || Values == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("INSERT INTO {0}({1}) VALUES({2})", Table?.Expression, Properties?.Join(",", p => p.Expression), Values?.Join(",", v => v?.Expression));
            }
        }
    }

    public class DeleteStatement : ExpressionBase, IDeleteStatement
    {
        public DeleteStatement()
        { }

        public DeleteStatement(ITableExpression table, IWhereClause where)
        {
            _table = table;
            _where = where;
            GenExpression();
        }

        private ITableExpression _table = null;
        private IWhereClause _where = null;

        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }

        public IWhereClause Where
        {
            get
            {
                return _where;
            }

            set
            {
                _where = value;
                GenExpression();
            }
        }

        public IEnumerable<string> Params
        {
            get
            {
                return Where?.Filter.Params();
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
                Expression = string.Format("DELETE FROM {0} {1}", Table?.Expression, Where?.Expression).TrimEnd();
            }
        }
    }

    public class UpdateStatement : ExpressionBase, IUpdateStatement
    {
        public UpdateStatement()
        { }

        public UpdateStatement(ITableExpression table, ISetClause set, IWhereClause where)
        {
            _table = table;
            _set = set;
            _where = where;
            GenExpression();
        }

        private ITableExpression _table = null;
        private ISetClause _set = null;
        private IWhereClause _where = null;

        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }
        public ISetClause Set
        {
            get
            {
                return _set;
            }

            set
            {
                _set = value;
                GenExpression();
            }
        }

        public IWhereClause Where
        {
            get
            {
                return _where;
            }

            set
            {
                _where = value;
                GenExpression();
            }
        }

        public IEnumerable<string> Params
        {
            get
            {
                var list = new List<string>();
                foreach (var item in Set.Sets)
                {
                    if (item.Value is ICustomerExpression)
                    {
                        list.AddRange((item.Value as ICustomerExpression).Params);
                    }
                    else if (item.Value is IParamExpression)
                    {
                        list.Add((item.Value as IParamExpression).Name);
                    }
                }
                list.AddRange(Where?.Filter.Params());
                return list.Distinct();
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
                Expression = string.Format("UPDATE {0} {1} {2}",
                    Table?.Expression,
                    Set?.Expression,
                    Where?.Expression).TrimEnd();
            }
        }
    }

    public class SetItemExpression : ExpressionBase, ISetItemExpression
    {
        public SetItemExpression(IPropertyExpression property, IValueExpression value)
        {
            _property = property;
            _value = value;
            GenExpression();
        }

        public SetItemExpression(IPropertyExpression property)
            : this(property, new ParamExpression(property.Name))
        {
        }

        private IPropertyExpression _property = null;
        private IValueExpression _value = null;

        public IPropertyExpression Property
        {
            get
            {
                return _property;
            }

            set
            {
                _property = value;
                GenExpression();
            }
        }

        public IValueExpression Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Property == null || Value == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0}={1}", Property?.Expression, Value?.Expression);
            }
        }
    }

    public class SetClause : ExpressionBase, ISetClause
    {
        public SetClause(ISetItemExpression[] sets)
        {
            _sets = sets;
            GenExpression();
        }

        private ISetItemExpression[] _sets = null;
        public ISetItemExpression[] Sets
        {
            get
            {
                return _sets;
            }

            set
            {
                _sets = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Sets == null || Sets.Length == 0)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("SET {0}", Sets?.Join(",", set => set.Expression));
            }
        }
    }

    public class SelectStatement : ExpressionBase, ISelectStatement
    {
        public SelectStatement()
        { }

        public SelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IJoinExpression[] joins, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
        {
            _tables = tables;
            _items = items;
            _joins = joins;
            _where = where;
            _groupby = groupby;
            _having = having;
            _orderby = orderby;
            GenExpression();
        }

        public SelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
            : this(tables, items, null, where, groupby, having, orderby)
        {
        }

        public SelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IWhereClause where, IOrderByClause orderby = null)
            : this(tables, items, where, null, null, orderby)
        {

        }

        private ITableExpression[] _tables = null;
        private ISelectItemExpression[] _items = null;
        private IJoinExpression[] _joins = null;
        private IWhereClause _where = null;
        private IGroupByClause _groupby = null;
        private IHavingClause _having = null;
        private IOrderByClause _orderby = null;

        public ITableExpression[] Tables
        {
            get
            {
                return _tables;
            }

            set
            {
                _tables = value;
                GenExpression();
            }
        }

        public ISelectItemExpression[] Items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = value;
                GenExpression();
            }
        }

        public IJoinExpression[] Joins
        {
            get { return _joins; }
            set
            {
                _joins = value;
                GenExpression();
            }
        }

        public IWhereClause Where
        {
            get
            {
                return _where;
            }

            set
            {
                _where = value;
                GenExpression();
            }
        }

        public IGroupByClause GroupBy
        {
            get
            {
                return _groupby;
            }
            set
            {
                _groupby = value;
                GenExpression();
            }
        }

        public IHavingClause Having
        {
            get
            {
                return _having;
            }
            set
            {
                _having = value;
                GenExpression();
            }
        }

        public IOrderByClause OrderBy
        {
            get
            {
                return _orderby;
            }

            set
            {
                _orderby = value;
                GenExpression();
            }
        }

        public IEnumerable<string> Params
        {
            get
            {
                var list = new List<string>();
                list.AddRange(Having?.Filter.Params());
                list.AddRange(Where?.Filter.Params());
                foreach (var join in Joins)
                {
                    list.AddRange(join?.On.Params());
                }
                return list.Distinct();
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
                Expression = string.Format("SELECT {1} FROM {6} {0} {2} {3} {4} {5}", Tables?.Join(",", t => t.Expression),
                    Items?.Join(",", s => s.Expression),
                    Where?.Expression,
                    GroupBy?.Expression,
                    string.IsNullOrWhiteSpace(GroupBy?.Expression) ? string.Empty : Having?.Expression,
                    OrderBy?.Expression,
                    Joins?.Join(" ", j => j?.Expression)).TrimEnd();
            }
        }
    }

    public class AsExpression : ExpressionBase, IAsExpression
    {
        public AsExpression(ISelectItemExpression selectItem, IPropertyExpression asProperty)
        {
            if (selectItem is IAsExpression)
            {
                throw new ArgumentException("selectItem");
            }
            _selectItem = selectItem;
            _asProperty = asProperty;
            GenExpression();
        }

        private ISelectItemExpression _selectItem = null;
        private IPropertyExpression _asProperty = null;

        public ISelectItemExpression SelectItem
        {
            get
            {
                return _selectItem;
            }

            set
            {
                _selectItem = value;
                GenExpression();
            }
        }

        public IPropertyExpression AsProperty
        {
            get
            {
                return _asProperty;
            }

            set
            {
                _asProperty = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (SelectItem == null || AsProperty == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format(" {0} as {1} ", SelectItem?.Expression, AsProperty?.Expression);
            }
        }
    }

    public class AllPropertiesExpression : ExpressionBase, ISelectItemExpression
    {
        public AllPropertiesExpression(ITableExpression table)
        {
            _table = table;
            GenExpression();
        }

        private ITableExpression _table = null;

        public ITableExpression Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;

            }
        }

        protected override void GenExpression()
        {
            Expression = string.Format(" {0}* ", Table == null ? string.Empty : Table.Expression + ".");
        }
    }

    public class JoinExpression : ExpressionBase, IJoinExpression
    {
        public JoinExpression(IJoinOperator joinOp, ITableExpression table, IFilterExpression on)
        {
            _joinOp = joinOp;
            _table = table;
            _on = on;
            GenExpression();
        }

        private IJoinOperator _joinOp;
        private ITableExpression _table;
        private IFilterExpression _on;

        public IJoinOperator JoinOp
        {
            get
            {
                return _joinOp;
            }
            set
            {
                _joinOp = value;
                GenExpression();
            }
        }

        public ITableExpression Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                GenExpression();
            }
        }

        public IFilterExpression On
        {
            get
            {
                return _on;
            }
            set
            {
                _on = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (JoinOp == null || Table == null || On == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0} {1} ON {2}", JoinOp, Table?.Expression, On?.Expression);
            }
        }
    }

    public class GroupByClause : ExpressionBase, IGroupByClause
    {
        public GroupByClause(IPropertyExpression property)
        {
            _property = property;
            GenExpression();
        }

        private IPropertyExpression _property;

        public IPropertyExpression Property
        {
            get
            {
                return _property;
            }

            set
            {
                _property = value;
            }
        }

        protected override void GenExpression()
        {
            if (Property == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("GROUP BY {0}", Property?.Expression);
            }
        }
    }

    public class HavingClause : ExpressionBase, IHavingClause
    {
        public HavingClause(IFilterExpression filter)
        {
            _filter = filter;
            GenExpression();
        }

        private IFilterExpression _filter = null;

        public IFilterExpression Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                _filter = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Filter == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("HAVING {0}", Filter?.Expression);
            }
        }
    }

    public class OrderExpression : ExpressionBase, IOrderExpression
    {
        public OrderExpression(string property, OrderEnum order = OrderEnum.Asc)
        {
            this.property = new PropertyExpression(property);
            this.order = order;
            GenExpression();
        }
        public OrderExpression(IPropertyExpression property, OrderEnum order = OrderEnum.Asc)
        {
            this.property = property;
            this.order = order;
            GenExpression();
        }

        public OrderExpression(IAggregateFunctionExpression fun, OrderEnum order = OrderEnum.Asc)
        {
            this.property = fun;
            this.order = order;
            GenExpression();
        }

        private OrderEnum order = OrderEnum.Asc;
        private ISelectableValueExpression property = null;
        public OrderEnum Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                GenExpression();
            }
        }

        public ISelectableValueExpression Property
        {
            get
            {
                return property;
            }
            set
            {
                property = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Property == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("{0} {1}", Property?.Expression, Order == OrderEnum.Asc ? "ASC" : "DESC");
            }
        }
    }

    public class OrderByClause : ExpressionBase, IOrderByClause
    {
        public OrderByClause(params IOrderExpression[] orders)
        {
            _orders = orders;
            GenExpression();
        }

        private IOrderExpression[] _orders;
        public IOrderExpression[] Orders
        {
            get
            {
                return _orders;
            }

            set
            {
                _orders = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Orders == null || Orders.Length == 0)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("ORDER BY {0}", Orders.Join(",", order => order.Expression));
            }
        }
    }

    public class WhereClause : ExpressionBase, IWhereClause
    {
        public WhereClause(IFilterExpression filter)
        {
            _filter = filter;
            GenExpression();
        }

        private IFilterExpression _filter = null;

        public IFilterExpression Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                _filter = value;
                GenExpression();
            }
        }

        protected override void GenExpression()
        {
            if (Filter == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("WHERE {0}", Filter.Expression);
            }
        }
    }

    public class ComparisonExpression : BinaryExpression, IComparisonExpression
    {
        public ComparisonExpression(IValueExpression a, IComparisonOperator op, IValueExpression b)
            : base(a, op, b)
        {
        }

        public static LogicExpression operator &(ComparisonExpression exp1, IFilterExpression exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(ComparisonExpression exp1, IFilterExpression exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }
    }

    public class UnaryComparisonExpression : UnaryExpression, IUnaryComparisonExpression
    {
        public UnaryComparisonExpression(IValueExpression a, IUnaryComparisonOperator op)
            : base(a, op)
        {
        }

        public static LogicExpression operator &(UnaryComparisonExpression exp1, IFilterExpression exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(UnaryComparisonExpression exp1, IFilterExpression exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }
    }

    public class LogicExpression : BinaryExpression, ILogicExpression
    {
        public LogicExpression(IFilterExpression a, ILogicOperator op, IFilterExpression b)
            : base(a, op, b)
        {
        }

        protected override void GenExpression()
        {
            if (A == null || B == null || Op == null)
            {
                Expression = string.Empty;
            }
            else
            {
                Expression = string.Format("({0}){1}({2})", A?.Expression, Op, B?.Expression);
            }
        }

        public static LogicExpression operator &(LogicExpression exp1, IFilterExpression exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(LogicExpression exp1, IFilterExpression exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }
    }

    public class CustomerExpression : ExpressionBase, ICustomerExpression
    {
        public CustomerExpression(string expression)
        {
            Expression = expression;
        }

        public IEnumerable<string> Params
        {
            get
            {
                var list = new List<string>();
                var matchs = Regex.Matches(Expression, "(?<=@)[_a-zA-Z]+[_a-zA-Z0-9]*(?=[^a-zA-Z0-9]|$)");
                foreach (Match match in matchs)
                {
                    list.Add(match.Value);
                }
                return list;
            }
        }

        protected override void GenExpression()
        {
        }

        public static implicit operator CustomerExpression(string expression)
        {
            return new CustomerExpression(expression);
        }
    }

    static class _Extension
    {
        public static IEnumerable<string> Params(this IFilterExpression filter)
        {
            if (filter is ICustomerExpression)
            {
                return (filter as ICustomerExpression).Params;
            }
            else if (filter is IBinaryExpression)
            {
                return GetParam(filter as IBinaryExpression);
            }
            else if (filter is IUnaryExpression)
            {
                return GetParam(filter as IUnaryExpression);
            }
            else
            {
                return new List<string>();
            }
        }

        private static List<string> GetParam(IBinaryExpression binary)
        {
            List<string> list = new List<string>();
            if (binary.A is ICustomerExpression)
            {
                list.AddRange((binary.A as ICustomerExpression).Params);
            }
            else if (binary.A is IParamExpression)
            {
                list.Add((binary.A as IParamExpression).Name);
            }
            else if (binary.A is IUnaryExpression)
            {
                list.AddRange(GetParam(binary.A as IUnaryExpression));
            }
            else if (binary.A is IBinaryExpression)
            {
                list.AddRange(GetParam(binary.A as IBinaryExpression));
            }

            if (binary.B is ICustomerExpression)
            {
                list.AddRange((binary.B as ICustomerExpression).Params);
            }
            else if (binary.B is IParamExpression)
            {
                list.Add((binary.B as IParamExpression).Name);
            }
            else if (binary.B is IUnaryExpression)
            {
                list.AddRange(GetParam(binary.B as IUnaryExpression));
            }
            else if (binary.B is IBinaryExpression)
            {
                list.AddRange(GetParam(binary.B as IBinaryExpression));
            }

            return list.Distinct().ToList();
        }

        private static List<string> GetParam(IUnaryExpression unaray)
        {
            List<string> list = new List<string>();
            if (unaray.A is ICustomerExpression)
            {
                list.AddRange((unaray as ICustomerExpression).Params());
            }
            else if (unaray.A is IParamExpression)
            {
                list.Add((unaray.A as IParamExpression).Name);
            }
            else if (unaray.A is IUnaryExpression)
            {
                list.AddRange(GetParam(unaray.A as IUnaryExpression));
            }
            else if (unaray.A is IBinaryExpression)
            {
                list.AddRange(GetParam(unaray.A as IBinaryExpression));
            }
            return list.Distinct().ToList();
        }
    }
}