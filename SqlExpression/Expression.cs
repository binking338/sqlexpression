using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlExpression
{
    public delegate string ExpressionHandler<Exp>(Exp exp) where Exp : class, IExpression;

    /// <summary>
    /// 表达式抽象类
    /// </summary>
    public abstract class ExpressionBase<Exp> : IExpression
        where Exp : class, IExpression
    {
        private static Dictionary<DBType, ExpressionHandler<Exp>> handlers = new Dictionary<DBType, ExpressionHandler<Exp>>();
        public static Dictionary<DBType, ExpressionHandler<Exp>> Handlers
        {
            get
            {
                return handlers;
            }
        }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression
        {
            get
            {
                Type type = this.GetType();
                if (Handlers.ContainsKey(Type) && Handlers[Type] != null)
                {
                    return Handlers[Type](this as Exp);
                }
                return GenExpression();
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DBType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 构建表达式
        /// </summary>
        protected abstract string GenExpression();

        public override string ToString()
        {
            return Expression;
        }

        public override bool Equals(object obj)
        {
            if (obj is IExpression)
            {
                return (obj as IExpression).Expression == this.Expression;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.Expression?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// 表
    /// </summary>
    public class TableExpression : ExpressionBase<TableExpression>, ITableExpression
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
            }
        }

        protected override string GenExpression()
        {
            return string.IsNullOrWhiteSpace(Name) ? string.Empty : Name;
        }

        public static implicit operator TableExpression(string name)
        {
            return new TableExpression(name);
        }
    }

    /// <summary>
    /// 字面值
    /// </summary>
    public class LiteralValueExpression : ExpressionBase<LiteralValueExpression>, ILiteralValueExpression
    {
        public LiteralValueExpression(object value)
        {
            if (value is ILiteralValueExpression)
            {
                Value = (value as ILiteralValueExpression).Value;
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
            }
        }

        protected override string GenExpression()
        {
            var valueType = Value?.GetType();
            if (Value == null || Value is DBNull)
            {
                return "NULL";
            }
            else if (valueType == typeof(string) || valueType == typeof(char))
            {
                return string.Format("'{0}'", Value);
            }
            else if (valueType == typeof(DateTime))
            {
                return string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", Value);
            }
            else if (valueType.IsEnum)
            {
                return Convert.ToInt32(Value).ToString();
            }
            else
            {
                return Value?.ToString();
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

    /// <summary>
    /// 字段
    /// </summary>
    public class ColumnExpression : ExpressionBase<ColumnExpression>, IColumnExpression
    {
        public ColumnExpression(string name, ITableExpression table = null)
        {
            _name = name;
            _table = table;
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
            }
        }

        protected override string GenExpression()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return string.Empty;
            }
            else
            {

                return string.Format("{1}{0}", Name, string.IsNullOrEmpty(Table?.Expression) ? string.Empty : Table.Expression + ".");
            }
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(ColumnExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ColumnExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ColumnExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ColumnExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ColumnExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ColumnExpression col, IValueExpression val)
        {
            return new ComparisonExpression(col, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(ColumnExpression col, LiteralValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ColumnExpression col, LiteralValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ColumnExpression col, LiteralValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ColumnExpression col, LiteralValueExpression val)
        {
            return new ComparisonExpression(col, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ColumnExpression col, LiteralValueExpression val)
        {
            return new ComparisonExpression(col, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ColumnExpression col, LiteralValueExpression val)
        {
            return new ComparisonExpression(col, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValueExpression val, ColumnExpression col)
        {
            return new ComparisonExpression(val, Operator.Eq, col);
        }
        public static ComparisonExpression operator !=(LiteralValueExpression val, ColumnExpression col)
        {
            return new ComparisonExpression(val, Operator.Neq, col);
        }
        public static ComparisonExpression operator >(LiteralValueExpression val, ColumnExpression col)
        {
            return new ComparisonExpression(val, Operator.Gt, col);
        }
        public static ComparisonExpression operator <(LiteralValueExpression val, ColumnExpression col)
        {
            return new ComparisonExpression(val, Operator.Lt, col);
        }
        public static ComparisonExpression operator >=(LiteralValueExpression val, ColumnExpression col)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, col);
        }
        public static ComparisonExpression operator <=(LiteralValueExpression val, ColumnExpression col)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, col);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(ColumnExpression col, IValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ColumnExpression col, IValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ColumnExpression col, IValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ColumnExpression col, IValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ColumnExpression col, IValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(ColumnExpression col, LiteralValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ColumnExpression col, LiteralValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ColumnExpression col, LiteralValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ColumnExpression col, LiteralValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ColumnExpression col, LiteralValueExpression val)
        {
            return new ArithmeticExpression(col, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValueExpression val, ColumnExpression col)
        {
            return new ArithmeticExpression(val, Operator.Add, col);
        }
        public static ArithmeticExpression operator -(LiteralValueExpression val, ColumnExpression col)
        {
            return new ArithmeticExpression(val, Operator.Sub, col);
        }
        public static ArithmeticExpression operator *(LiteralValueExpression val, ColumnExpression col)
        {
            return new ArithmeticExpression(val, Operator.Mul, col);
        }
        public static ArithmeticExpression operator /(LiteralValueExpression val, ColumnExpression col)
        {
            return new ArithmeticExpression(val, Operator.Div, col);
        }
        public static ArithmeticExpression operator %(LiteralValueExpression val, ColumnExpression col)
        {
            return new ArithmeticExpression(val, Operator.Mod, col);
        }

        #endregion

        #region 隐式转换

        public static implicit operator ColumnExpression(string col)
        {
            return new ColumnExpression(col);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is ColumnExpression)
            {
                return (obj as ColumnExpression).Name == this.Name;
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

    /// <summary>
    /// 参数
    /// </summary>
    public class ParamExpression : ExpressionBase<ParamExpression>, IParamExpression
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
            }
        }

        protected override string GenExpression()
        {
            return string.IsNullOrWhiteSpace(Name) ? string.Empty : string.Format("@{0}", Name);
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(ParamExpression param, IValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ParamExpression param, IValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ParamExpression param, IValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ParamExpression param, IValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ParamExpression param, IValueExpression val)
        {
            return new ComparisonExpression(param, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ParamExpression param, IValueExpression val)
        {
            return new ComparisonExpression(param, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(ParamExpression param, LiteralValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ParamExpression param, LiteralValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ParamExpression param, LiteralValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ParamExpression param, LiteralValueExpression val)
        {
            return new ComparisonExpression(param, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ParamExpression param, LiteralValueExpression val)
        {
            return new ComparisonExpression(param, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ParamExpression param, LiteralValueExpression val)
        {
            return new ComparisonExpression(param, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValueExpression val, ParamExpression param)
        {
            return new ComparisonExpression(val, Operator.Eq, param);
        }
        public static ComparisonExpression operator !=(LiteralValueExpression val, ParamExpression param)
        {
            return new ComparisonExpression(val, Operator.Neq, param);
        }
        public static ComparisonExpression operator >(LiteralValueExpression val, ParamExpression param)
        {
            return new ComparisonExpression(val, Operator.Gt, param);
        }
        public static ComparisonExpression operator <(LiteralValueExpression val, ParamExpression param)
        {
            return new ComparisonExpression(val, Operator.Lt, param);
        }
        public static ComparisonExpression operator >=(LiteralValueExpression val, ParamExpression param)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, param);
        }
        public static ComparisonExpression operator <=(LiteralValueExpression val, ParamExpression param)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, param);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(ParamExpression param, IValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ParamExpression param, IValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ParamExpression param, IValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ParamExpression param, IValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ParamExpression param, IValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(ParamExpression param, LiteralValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ParamExpression param, LiteralValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ParamExpression param, LiteralValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ParamExpression param, LiteralValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ParamExpression param, LiteralValueExpression val)
        {
            return new ArithmeticExpression(param, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValueExpression val, ParamExpression param)
        {
            return new ArithmeticExpression(val, Operator.Add, param);
        }
        public static ArithmeticExpression operator -(LiteralValueExpression val, ParamExpression param)
        {
            return new ArithmeticExpression(val, Operator.Sub, param);
        }
        public static ArithmeticExpression operator *(LiteralValueExpression val, ParamExpression param)
        {
            return new ArithmeticExpression(val, Operator.Mul, param);
        }
        public static ArithmeticExpression operator /(LiteralValueExpression val, ParamExpression param)
        {
            return new ArithmeticExpression(val, Operator.Div, param);
        }
        public static ArithmeticExpression operator %(LiteralValueExpression val, ParamExpression param)
        {
            return new ArithmeticExpression(val, Operator.Mod, param);
        }

        #endregion

        #region 隐式转换

        public static implicit operator ParamExpression(string param)
        {
            return new ParamExpression(param);
        }

        public static implicit operator ParamExpression(ColumnExpression col)
        {
            return new ParamExpression(col.Name);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is ParamExpression)
            {
                return (obj as ParamExpression).Name == this.Name;
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

    /// <summary>
    /// 集合（In）
    /// </summary>
    public class CollectionExpression : ExpressionBase<CollectionExpression>, IValueExpression
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
            }
        }

        protected override string GenExpression()
        {
            if (Values == null || Values.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("({0})", _values.Join(",", exp => exp.Expression));
            }
        }

        #region 隐式转换

        public static implicit operator CollectionExpression(object[] values)
        {
            return new CollectionExpression(values.Select(v => new LiteralValueExpression(v)).ToArray());
        }

        #endregion
    }

    /// <summary>
    /// 值范围（Between）
    /// </summary>
    public class BetweenValueExpression : ExpressionBase<BetweenValueExpression>, IValueExpression
    {
        public BetweenValueExpression(IValueExpression min, IValueExpression max)
        {
            _min = min;
            _max = max;
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
            }
        }

        protected override string GenExpression()
        {
            if (Min == null || Max == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0} AND {1}", Min.Expression, Max.Expression);
            }
        }
    }

    /// <summary>
    /// 一元表达式
    /// </summary>
    public class UnaryExpression : ExpressionBase<UnaryExpression>, IUnaryExpression
    {
        public UnaryExpression(IExpression a, IUnaryOperator op)
        {
            _a = a;
            _op = op;
        }

        private IExpression _a = null;
        private IUnaryOperator _op = null;

        /// <summary>
        /// 属性名称
        /// </summary>
        public IExpression A { get { return _a; } set { _a = value; } }

        /// <summary>
        /// 操作符
        /// </summary>
        public IUnaryOperator Op { get { return _op; } set { _op = value; } }

        protected override string GenExpression()
        {
            if (A == null || Op == null)
            {
                return string.Empty;
            }
            else
            {
                if (A is IBinaryExpression || A is IUnaryExpression)
                {
                    return string.Format("({0}){1}", A?.Expression, Op);
                }
                else
                {
                    return string.Format("{0}{1}", A?.Expression, Op);
                }
            }
        }
    }

    /// <summary>
    /// 二元表达式
    /// </summary>
    public class BinaryExpression : ExpressionBase<BinaryExpression>, IBinaryExpression
    {

        public BinaryExpression(IExpression a, IBinaryOperator op, IExpression b)
        {
            _a = a;
            _op = op;
            _b = b;
        }

        private IExpression _a = null;
        /// <summary>
        /// 属性名称
        /// </summary>
        public IExpression A { get { return _a; } set { _a = value; } }

        private IBinaryOperator _op = null;
        /// <summary>
        /// 操作符
        /// </summary>
        public IBinaryOperator Op { get { return _op; } set { _op = value; } }

        private IExpression _b = null;
        /// <summary>
        /// 值
        /// </summary>
        public IExpression B { get { return _b; } set { _b = value; } }

        private bool _withBracket = false;
        /// <summary>
        /// 是否括号括起来
        /// </summary>
        public bool WithBracket { get { return _withBracket; } set { _withBracket = value; } }

        protected override string GenExpression()
        {
            if (A == null || B == null || Op == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format(WithBracket ? "({0}{1}{2})" : "{0}{1}{2}", A?.Expression, Op, B?.Expression);
            }
        }
    }

    /// <summary>
    /// 算术表达式
    /// </summary>
    public class ArithmeticExpression : BinaryExpression, IArithmeticExpression
    {
        public ArithmeticExpression(IValueExpression a, IArithmeticOperator op, IValueExpression b)
            : base(a, op, b)
        {
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(ArithmeticExpression exp, IValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ArithmeticExpression exp, IValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ArithmeticExpression exp, IValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ArithmeticExpression exp, IValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ArithmeticExpression exp, IValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ArithmeticExpression exp, IValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ComparisonExpression(exp, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Eq, exp);
        }
        public static ComparisonExpression operator !=(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Neq, exp);
        }
        public static ComparisonExpression operator >(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Gt, exp);
        }
        public static ComparisonExpression operator <(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Lt, exp);
        }
        public static ComparisonExpression operator >=(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, exp);
        }
        public static ComparisonExpression operator <=(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, exp);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(ArithmeticExpression exp, IValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ArithmeticExpression exp, IValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ArithmeticExpression exp, IValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ArithmeticExpression exp, IValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ArithmeticExpression exp, IValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ArithmeticExpression exp, LiteralValueExpression val)
        {
            return new ArithmeticExpression(exp, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Add, exp);
        }
        public static ArithmeticExpression operator -(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Sub, exp);
        }
        public static ArithmeticExpression operator *(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Mul, exp);
        }
        public static ArithmeticExpression operator /(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Div, exp);
        }
        public static ArithmeticExpression operator %(LiteralValueExpression val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Mod, exp);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is ArithmeticExpression)
            {
                return (obj as ArithmeticExpression).Expression == this.Expression;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.Expression?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// 函数表达式
    /// </summary>
    public class FunctionExpression : ExpressionBase<FunctionExpression>, IFunctionExpression
    {
        public static AggregateFunctionExpression Count(IValueExpression col)
        {
            return new AggregateFunctionExpression("COUNT", col);
        }

        public static AggregateFunctionExpression Sum(IValueExpression col)
        {
            return new AggregateFunctionExpression("SUM", col);
        }

        public static AggregateFunctionExpression Avg(IValueExpression col)
        {
            return new AggregateFunctionExpression("AVG", col);
        }

        public static AggregateFunctionExpression Max(IValueExpression col)
        {
            return new AggregateFunctionExpression("MAX", col);
        }

        public static AggregateFunctionExpression Min(IValueExpression col)
        {
            return new AggregateFunctionExpression("MIN", col);
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

        protected override string GenExpression()
        {
            if (Name == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0}({1})", Name, Values.Join(",", v => v?.Expression));
            }
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(FunctionExpression fun, IValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(FunctionExpression fun, IValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(FunctionExpression fun, IValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(FunctionExpression fun, IValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(FunctionExpression fun, IValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(FunctionExpression fun, IValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ComparisonExpression(fun, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Eq, fun);
        }
        public static ComparisonExpression operator !=(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Neq, fun);
        }
        public static ComparisonExpression operator >(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Gt, fun);
        }
        public static ComparisonExpression operator <(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Lt, fun);
        }
        public static ComparisonExpression operator >=(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, fun);
        }
        public static ComparisonExpression operator <=(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, fun);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(FunctionExpression fun, IValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(FunctionExpression fun, IValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(FunctionExpression fun, IValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(FunctionExpression fun, IValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(FunctionExpression fun, IValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(FunctionExpression fun, LiteralValueExpression val)
        {
            return new ArithmeticExpression(fun, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Add, fun);
        }
        public static ArithmeticExpression operator -(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Sub, fun);
        }
        public static ArithmeticExpression operator *(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Mul, fun);
        }
        public static ArithmeticExpression operator /(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Div, fun);
        }
        public static ArithmeticExpression operator %(LiteralValueExpression val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Mod, fun);
        }

        #endregion


        public override bool Equals(object obj)
        {
            if (obj is FunctionExpression)
            {
                return (obj as FunctionExpression).Expression == this.Expression;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.Expression?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// 聚合函数表达式
    /// </summary>
    public class AggregateFunctionExpression : FunctionExpression, IAggregateFunctionExpression
    {
        public AggregateFunctionExpression(string name, IValueExpression value)
            : base(name, value)
        {

        }
    }

    /// <summary>
    /// 批量Sql语句
    /// </summary>
    public class BatchSqlStatement : ExpressionBase<BatchSqlStatement>, IBatchSqlStatement
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

        protected override string GenExpression()
        {
            if (Sqls == null)
            {
                return string.Empty;
            }
            else
            {
                return Sqls.Where(sql => !string.IsNullOrWhiteSpace(sql?.Expression)).Join(";", sql => sql.Expression);
            }
        }
    }

    /// <summary>
    /// 合并查询语句
    /// </summary>
    public class UnionStatement : ExpressionBase<UnionStatement>, IUnionStatement
    {
        public UnionStatement(ISelectStatement select, IUnionItemExpression[] unionItems, IOrderByClause orderBy)
        {
            _select = select;
            _unionItems = unionItems;
            _orderBy = orderBy;
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

        protected override string GenExpression()
        {
            if (Select == null)
            {
                return string.Empty;
            }
            else
            {
                return (new string[] {
                    Select.Expression,
                    UnionItems?.Join(" ", union => union.Expression),
                    OrderBy?.Expression
                }).Where(s => !string.IsNullOrWhiteSpace(s)).Join(" ");
            }
        }
    }

    /// <summary>
    /// 合并查询子项
    /// </summary>
    public class UnionItemExpression : ExpressionBase<UnionItemExpression>, IUnionItemExpression
    {
        public UnionItemExpression(IUnionOperator unionOp, ISelectStatement select)
        {
            _unionOp = unionOp;
            _select = select;
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
            }
        }

        protected override string GenExpression()
        {
            if (Select == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0} {1}", UnionOp, Select.Expression);
            }
        }
    }

    /// <summary>
    /// 插入语句
    /// </summary>
    public class InsertStatement : ExpressionBase<InsertStatement>, IInsertStatement
    {
        public InsertStatement() : this(null) { }

        public InsertStatement(ITableExpression table)
            : this(table, new IColumnExpression[0], new IColumnExpression[0])
        { }

        public InsertStatement(ITableExpression table, IColumnExpression[] columns, IValueExpression[] values)
        {
            _table = table;
            _properties = columns;
            _values = values;
        }

        private ITableExpression _table = null;
        private IColumnExpression[] _properties = null;
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
            }
        }

        public IColumnExpression[] Columns
        {
            get
            {
                return _properties;
            }

            set
            {
                _properties = value;
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

        protected override string GenExpression()
        {
            if (Table == null || Columns == null || Values == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("INSERT INTO {0}({1}) VALUES({2})", Table?.Expression, Columns?.Join(",", p => p.Expression), Values?.Join(",", v => v?.Expression));
            }
        }
    }

    /// <summary>
    /// 删除语句
    /// </summary>
    public class DeleteStatement : ExpressionBase<DeleteStatement>, IDeleteStatement
    {
        public DeleteStatement()
            : this(null)
        { }

        public DeleteStatement(ITableExpression table)
            : this(table, null)
        { }

        public DeleteStatement(ITableExpression table, IWhereClause where)
        {
            _table = table;
            _where = where;
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
            }
        }

        public IEnumerable<string> Params
        {
            get
            {
                List<string> list = new List<string>();
                if (Where != null) list.AddRange(Where.Filter.Params());
                return list;
            }
        }

        protected override string GenExpression()
        {
            if (Table == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("DELETE FROM {0} {1}", Table?.Expression, Where?.Expression).TrimEnd();
            }
        }
    }

    /// <summary>
    /// 更新语句
    /// </summary>
    public class UpdateStatement : ExpressionBase<UpdateStatement>, IUpdateStatement
    {
        public UpdateStatement()
            : this(null)
        {
        }

        public UpdateStatement(ITableExpression table)
            : this(table, new SetClause(null), null)
        {
        }

        public UpdateStatement(ITableExpression table, ISetClause set, IWhereClause where)
        {
            _table = table;
            _set = set;
            _where = where;
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
                if (Where != null) list.AddRange(Where.Filter.Params());
                return list.Distinct();
            }
        }

        protected override string GenExpression()
        {
            if (Table == null || Set == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("UPDATE {0} {1} {2}",
                    Table?.Expression,
                    Set?.Expression,
                    Where?.Expression).TrimEnd();
            }
        }
    }

    /// <summary>
    /// 更新赋值项
    /// </summary>
    public class SetItemExpression : ExpressionBase<SetItemExpression>, ISetItemExpression
    {
        public SetItemExpression(IColumnExpression column, IValueExpression value)
        {
            _property = column;
            _value = value;
        }

        public SetItemExpression(IColumnExpression column)
            : this(column, new ParamExpression(column.Name))
        {
        }

        private IColumnExpression _property = null;
        private IValueExpression _value = null;

        public IColumnExpression Column
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

        public IValueExpression Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        protected override string GenExpression()
        {
            if (Column == null || Value == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0}={1}", Column?.Expression, Value?.Expression);
            }
        }
    }

    /// <summary>
    /// 更新赋值子句
    /// </summary>
    public class SetClause : ExpressionBase<SetClause>, ISetClause
    {
        public SetClause(ISetItemExpression[] sets)
        {
            _sets = sets;
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
            }
        }

        protected override string GenExpression()
        {
            if (Sets == null || Sets.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("SET {0}", Sets?.Join(",", set => set.Expression));
            }
        }
    }

    /// <summary>
    /// 查询语句
    /// </summary>
    public class SelectStatement : ExpressionBase<SelectStatement>, ISelectStatement
    {
        public SelectStatement()
            : this(null)
        { }

        public SelectStatement(params ITableExpression[] tables)
            : this(tables, new ISelectItemExpression[0], null)
        { }

        public SelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IWhereClause where, IOrderByClause orderby = null)
            : this(tables, items, where, null, null, orderby)
        { }

        public SelectStatement(ITableExpression[] tables, ISelectItemExpression[] items, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
            : this(tables, items, null, where, groupby, having, orderby)
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
            }
        }

        public IJoinExpression[] Joins
        {
            get { return _joins; }
            set
            {
                _joins = value;
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
            }
        }

        public IEnumerable<string> Params
        {
            get
            {
                var list = new List<string>();
                if (Having != null) list.AddRange(Having.Filter.Params());
                if (Where != null) list.AddRange(Where.Filter.Params());
                if (Joins != null)
                {
                    foreach (var join in Joins)
                    {
                        if (join?.On != null) list.AddRange(join.On.Params());
                    }
                }
                return list.Distinct();
            }
        }

        protected override string GenExpression()
        {
            if (Tables == null || Tables.Length == 0 || Items == null || Items.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("SELECT {0} FROM {1}", Items?.Join(",", s => s.Expression),
                    string.Join(" ", new string[] {
                        Tables?.Join(",", t => t.Expression),
                        Joins?.Join(" ", j => j?.Expression),
                        Where?.Expression,
                        GroupBy?.Expression,
                        string.IsNullOrWhiteSpace(GroupBy?.Expression) ? string.Empty : Having?.Expression,
                        OrderBy?.Expression}.Where(s => !string.IsNullOrWhiteSpace(s))))
                   .TrimEnd();
            }
        }
    }

    /// <summary>
    /// 查询列别名
    /// </summary>
    public class AsExpression : ExpressionBase<AsExpression>, IAsExpression
    {
        public AsExpression(ISelectItemExpression selectItem, IColumnExpression asProperty)
        {
            if (selectItem is IAsExpression)
            {
                throw new ArgumentException("selectItem");
            }
            _selectItem = selectItem;
            _asProperty = asProperty;
        }

        private ISelectItemExpression _selectItem = null;
        private IColumnExpression _asProperty = null;

        public ISelectItemExpression SelectItem
        {
            get
            {
                return _selectItem;
            }

            set
            {
                _selectItem = value;
            }
        }

        public IColumnExpression AsName
        {
            get
            {
                return _asProperty;
            }

            set
            {
                _asProperty = value;
            }
        }

        protected override string GenExpression()
        {
            if (SelectItem == null || AsName == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0} AS {1}", SelectItem?.Expression, AsName?.Expression);
            }
        }
    }

    /// <summary>
    /// 所有项 *
    /// </summary>
    public class AllPropertiesExpression : ExpressionBase<AllPropertiesExpression>, ISelectItemExpression
    {
        public AllPropertiesExpression(ITableExpression table)
        {
            _table = table;
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

        protected override string GenExpression()
        {
            return string.Format(" {0}* ", Table == null ? string.Empty : Table.Expression + ".");
        }
    }

    /// <summary>
    /// 查询联接
    /// </summary>
    public class JoinExpression : ExpressionBase<JoinExpression>, IJoinExpression
    {
        public JoinExpression(IJoinOperator joinOp, ITableExpression table, IFilterExpression on)
        {
            _joinOp = joinOp;
            _table = table;
            _on = on;
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
            }
        }

        protected override string GenExpression()
        {
            if (JoinOp == null || Table == null || On == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0} {1} ON {2}", JoinOp, Table?.Expression, On?.Expression);
            }
        }
    }

    /// <summary>
    /// 分组子句
    /// </summary>
    public class GroupByClause : ExpressionBase<GroupByClause>, IGroupByClause
    {
        public GroupByClause(IColumnExpression column)
        {
            _property = column;
        }

        private IColumnExpression _property;

        public IColumnExpression Column
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

        protected override string GenExpression()
        {
            if (Column == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("GROUP BY {0}", Column?.Expression);
            }
        }
    }

    /// <summary>
    /// 分组条件
    /// </summary>
    public class HavingClause : ExpressionBase<HavingClause>, IHavingClause
    {
        public HavingClause(IFilterExpression filter)
        {
            _filter = filter;
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
            }
        }

        protected override string GenExpression()
        {
            if (Filter == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("HAVING {0}", Filter?.Expression);
            }
        }
    }

    /// <summary>
    /// 排序项
    /// </summary>
    public class OrderExpression : ExpressionBase<OrderExpression>, IOrderExpression
    {
        public OrderExpression(string column, OrderEnum order = OrderEnum.Asc)
        {
            this.column = new ColumnExpression(column);
            this.order = order;
        }
        public OrderExpression(IColumnExpression column, OrderEnum order = OrderEnum.Asc)
        {
            this.column = column;
            this.order = order;
        }

        public OrderExpression(IAggregateFunctionExpression fun, OrderEnum order = OrderEnum.Asc)
        {
            this.column = fun;
            this.order = order;
        }

        private OrderEnum order = OrderEnum.Asc;
        public OrderEnum Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        private ISelectItemExpression column = null;
        public ISelectItemExpression Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }

        protected override string GenExpression()
        {
            if (Column == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0} {1}", Column?.Expression, Order == OrderEnum.Asc ? "ASC" : "DESC");
            }
        }
    }

    /// <summary>
    /// 排序子句
    /// </summary>
    public class OrderByClause : ExpressionBase<OrderByClause>, IOrderByClause
    {
        public OrderByClause(params IOrderExpression[] orders)
        {
            _orders = orders;
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
            }
        }

        protected override string GenExpression()
        {
            if (Orders == null || Orders.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("ORDER BY {0}", Orders.Join(",", order => order.Expression));
            }
        }
    }

    /// <summary>
    /// 筛选条件子句 
    /// </summary>
    public class WhereClause : ExpressionBase<WhereClause>, IWhereClause
    {
        public WhereClause(IFilterExpression filter)
        {
            _filter = filter;
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
            }
        }

        protected override string GenExpression()
        {
            if (Filter == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("WHERE {0}", Filter.Expression);
            }
        }
    }

    /// <summary>
    /// 二元比较表达式
    /// </summary>
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

    /// <summary>
    /// 一元比较表达式
    /// </summary>
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

    /// <summary>
    /// 逻辑表达式
    /// </summary>
    public class LogicExpression : BinaryExpression, ILogicExpression
    {
        public LogicExpression(IFilterExpression a, ILogicOperator op, IFilterExpression b)
            : base(a, op, b)
        {
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

    /// <summary>
    /// 自定义表达式
    /// </summary>
    public class CustomerExpression : ExpressionBase<CustomerExpression>, ICustomerExpression
    {
        string _expression;
        public CustomerExpression(string expression)
        {
            _expression = expression;
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
                return list.Distinct();
            }
        }

        protected override string GenExpression()
        {
            return _expression;
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

    public static class Extensions
    {
        /// <summary>
        /// 拼接字符串数组，拼接之前调用transformer变换字符串
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator"></param>
        /// <param name="transformer"></param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> array, string separator, Func<T, string> transformer = null)
        {
            if (array == null) return string.Empty;
            if (transformer == null) transformer = o => o.ToString();
            var wrapArray = from i in array select transformer(i);
            var result = string.Join(separator, wrapArray);
            return result;
        }
    }
}