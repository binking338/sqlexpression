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

        public ExpressionBase()
        {
            Type = SqlExpression.Expression.DefaultType;
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

    public static class Expression
    {
        public static DBType DefaultType { get; set; } = DBType.Common;
    }

    /// <summary>
    /// 表
    /// </summary>
    public class Table : ExpressionBase<Table>, ITable
    {
        public Table(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        protected override string GenExpression()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new SqlSyntaxException(this, Error.TableNameMissing);
            }
            return Name;
        }

        public static implicit operator Table(string name)
        {
            return new Table(name);
        }
    }

    /// <summary>
    /// 表别名
    /// </summary>
    public class DatasetAlias : ExpressionBase<DatasetAlias>, IDatasetAlias
    {
        public DatasetAlias(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string Name { get; set; }

        protected override string GenExpression()
        {
            if (Name == null)
            {
                throw new SqlSyntaxException(this, Error.AliasNameMissing);
            }
            return Name;
        }
    }

    /// <summary>
    /// 表别名表达式
    /// </summary>
    public class TableAliasExpression : ExpressionBase<TableAliasExpression>, ITableAliasExpression
    {
        public TableAliasExpression(ITable table, IDatasetAlias alias = null)
        {
            Table = table;
            Alias = alias;
        }

        public ITable Table { get; set; }

        public IDatasetAlias Alias { get; set; }

        protected override string GenExpression()
        {
            if (Table == null)
            {
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            if (string.IsNullOrWhiteSpace(Alias?.Name))
            {
                return Table.Expression;
            }
            else
            {
                return string.Format("{0} AS {1}", Table.Expression, Alias.Expression);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubQueryAliasExpression : ExpressionBase<SubQueryAliasExpression>, ISubQueryAliasExpression
    {
        public SubQueryAliasExpression(ISubQueryExpression subquery, IDatasetAlias alias = null)
        {
            SubQuery = subquery;
            Alias = alias;
        }

        public SubQueryAliasExpression(ISelectStatement subquery)
            : this(new SubQueryExpression(subquery))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public IDatasetAlias Alias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ISubQueryExpression SubQuery { get; set; }

        protected override string GenExpression()
        {
            if (SubQuery == null)
            {
                throw new SqlSyntaxException(this, Error.SubQueryMissing);
            }
            return string.Format("({0})", SubQuery);
        }
    }

    /// <summary>
    /// 字段
    /// </summary>
    public class Column : ExpressionBase<Column>, IColumn
    {
        public Column(string name, IDatasetAlias dataset = null)
        {
            Name = name;
            Dataset = dataset;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据集别名
        /// </summary>
        public IDatasetAlias Dataset { get; set; }

        protected override string GenExpression()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new SqlSyntaxException(this, Error.ColumnNameMissing);
            }
            if (Dataset == null)
            {
                return Name;
            }
            else
            {
                return string.Format("{1}.{0}", Name, Dataset.Expression);
            }
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(Column col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(Column col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(Column col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(Column col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(Column col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(Column col, ISimpleValue val)
        {
            return new ComparisonExpression(col, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(Column col, LiteralValue val)
        {
            return new ComparisonExpression(col, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(Column col, LiteralValue val)
        {
            return new ComparisonExpression(col, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(Column col, LiteralValue val)
        {
            return new ComparisonExpression(col, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(Column col, LiteralValue val)
        {
            return new ComparisonExpression(col, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(Column col, LiteralValue val)
        {
            return new ComparisonExpression(col, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(Column col, LiteralValue val)
        {
            return new ComparisonExpression(col, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValue val, Column col)
        {
            return new ComparisonExpression(val, Operator.Eq, col);
        }
        public static ComparisonExpression operator !=(LiteralValue val, Column col)
        {
            return new ComparisonExpression(val, Operator.Neq, col);
        }
        public static ComparisonExpression operator >(LiteralValue val, Column col)
        {
            return new ComparisonExpression(val, Operator.Gt, col);
        }
        public static ComparisonExpression operator <(LiteralValue val, Column col)
        {
            return new ComparisonExpression(val, Operator.Lt, col);
        }
        public static ComparisonExpression operator >=(LiteralValue val, Column col)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, col);
        }
        public static ComparisonExpression operator <=(LiteralValue val, Column col)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, col);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(Column col, ISimpleValue val)
        {
            return new ArithmeticExpression(col, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(Column col, ISimpleValue val)
        {
            return new ArithmeticExpression(col, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(Column col, ISimpleValue val)
        {
            return new ArithmeticExpression(col, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(Column col, ISimpleValue val)
        {
            return new ArithmeticExpression(col, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(Column col, ISimpleValue val)
        {
            return new ArithmeticExpression(col, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(Column col, LiteralValue val)
        {
            return new ArithmeticExpression(col, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(Column col, LiteralValue val)
        {
            return new ArithmeticExpression(col, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(Column col, LiteralValue val)
        {
            return new ArithmeticExpression(col, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(Column col, LiteralValue val)
        {
            return new ArithmeticExpression(col, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(Column col, LiteralValue val)
        {
            return new ArithmeticExpression(col, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValue val, Column col)
        {
            return new ArithmeticExpression(val, Operator.Add, col);
        }
        public static ArithmeticExpression operator -(LiteralValue val, Column col)
        {
            return new ArithmeticExpression(val, Operator.Sub, col);
        }
        public static ArithmeticExpression operator *(LiteralValue val, Column col)
        {
            return new ArithmeticExpression(val, Operator.Mul, col);
        }
        public static ArithmeticExpression operator /(LiteralValue val, Column col)
        {
            return new ArithmeticExpression(val, Operator.Div, col);
        }
        public static ArithmeticExpression operator %(LiteralValue val, Column col)
        {
            return new ArithmeticExpression(val, Operator.Mod, col);
        }

        #endregion

        #region 隐式转换

        public static implicit operator Column(string col)
        {
            return new Column(col);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is Column)
            {
                return (obj as Column).Name == this.Name;
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
    /// 字面值
    /// </summary>
    public class LiteralValue : ExpressionBase<LiteralValue>, ILiteralValue
    {
        public LiteralValue(object value)
        {
            if (value is ILiteralValue)
            {
                Value = (value as ILiteralValue).Value;
            }
            else if (value is IExpression)
            {
                throw new ArgumentException(nameof(value));
            }
            else
            {
                Value = value;
            }
        }

        public object Value { get; set; }

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

        public static implicit operator LiteralValue(string value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(DateTime value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(ushort value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(short value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(uint value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(int value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(ulong value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(long value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(decimal value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(double value)
        {
            return new LiteralValue(value);
        }

        public static implicit operator LiteralValue(float value)
        {
            return new LiteralValue(value);
        }

        #endregion
    }

    /// <summary>
    /// 参数
    /// </summary>
    public class Param : ExpressionBase<Param>, IParam
    {
        public Param(string param)
        {
            Name = param;
        }

        public string Name { get; set; }

        protected override string GenExpression()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new SqlSyntaxException(this, Error.ParamNameMissing);
            }
            return string.Format("@{0}", Name);
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(Param param, ISimpleValue val)
        {
            return new ComparisonExpression(param, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(Param param, ISimpleValue val)
        {
            return new ComparisonExpression(param, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(Param param, ISimpleValue val)
        {
            return new ComparisonExpression(param, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(Param param, ISimpleValue val)
        {
            return new ComparisonExpression(param, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(Param param, ISimpleValue val)
        {
            return new ComparisonExpression(param, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(Param param, ISimpleValue val)
        {
            return new ComparisonExpression(param, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(Param param, LiteralValue val)
        {
            return new ComparisonExpression(param, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(Param param, LiteralValue val)
        {
            return new ComparisonExpression(param, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(Param param, LiteralValue val)
        {
            return new ComparisonExpression(param, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(Param param, LiteralValue val)
        {
            return new ComparisonExpression(param, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(Param param, LiteralValue val)
        {
            return new ComparisonExpression(param, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(Param param, LiteralValue val)
        {
            return new ComparisonExpression(param, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValue val, Param param)
        {
            return new ComparisonExpression(val, Operator.Eq, param);
        }
        public static ComparisonExpression operator !=(LiteralValue val, Param param)
        {
            return new ComparisonExpression(val, Operator.Neq, param);
        }
        public static ComparisonExpression operator >(LiteralValue val, Param param)
        {
            return new ComparisonExpression(val, Operator.Gt, param);
        }
        public static ComparisonExpression operator <(LiteralValue val, Param param)
        {
            return new ComparisonExpression(val, Operator.Lt, param);
        }
        public static ComparisonExpression operator >=(LiteralValue val, Param param)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, param);
        }
        public static ComparisonExpression operator <=(LiteralValue val, Param param)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, param);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(Param param, ISimpleValue val)
        {
            return new ArithmeticExpression(param, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(Param param, ISimpleValue val)
        {
            return new ArithmeticExpression(param, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(Param param, ISimpleValue val)
        {
            return new ArithmeticExpression(param, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(Param param, ISimpleValue val)
        {
            return new ArithmeticExpression(param, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(Param param, ISimpleValue val)
        {
            return new ArithmeticExpression(param, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(Param param, LiteralValue val)
        {
            return new ArithmeticExpression(param, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(Param param, LiteralValue val)
        {
            return new ArithmeticExpression(param, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(Param param, LiteralValue val)
        {
            return new ArithmeticExpression(param, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(Param param, LiteralValue val)
        {
            return new ArithmeticExpression(param, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(Param param, LiteralValue val)
        {
            return new ArithmeticExpression(param, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValue val, Param param)
        {
            return new ArithmeticExpression(val, Operator.Add, param);
        }
        public static ArithmeticExpression operator -(LiteralValue val, Param param)
        {
            return new ArithmeticExpression(val, Operator.Sub, param);
        }
        public static ArithmeticExpression operator *(LiteralValue val, Param param)
        {
            return new ArithmeticExpression(val, Operator.Mul, param);
        }
        public static ArithmeticExpression operator /(LiteralValue val, Param param)
        {
            return new ArithmeticExpression(val, Operator.Div, param);
        }
        public static ArithmeticExpression operator %(LiteralValue val, Param param)
        {
            return new ArithmeticExpression(val, Operator.Mod, param);
        }

        #endregion

        #region 隐式转换

        public static implicit operator Param(string param)
        {
            return new Param(param);
        }

        public static implicit operator Param(Column col)
        {
            return new Param(col.Name);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is Param)
            {
                return (obj as Param).Expression == this.Expression;
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
    public class ValueCollectionExpression : ExpressionBase<ValueCollectionExpression>, IValueCollectionExpression
    {
        public ValueCollectionExpression(params ISimpleValue[] values)
        {
            Values = values;
        }

        public ISimpleValue[] Values { get; set; }

        protected override string GenExpression()
        {
            if (Values == null || Values.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.CollectionValuesMissing);
            }
            return string.Format("({0})", Values.Join(",", exp => exp.Expression));
        }

        #region 隐式转换

        public static implicit operator ValueCollectionExpression(object[] values)
        {
            return new ValueCollectionExpression(values.Select(v => new LiteralValue(v)).ToArray());
        }

        #endregion
    }

    public class SubQueryExpression : ExpressionBase<SubQueryExpression>, ISubQueryExpression
    {
        public SubQueryExpression(ISelectStatement query)
        {
            Query = query;
        }

        public ISelectStatement Query { get; set; }

        protected override string GenExpression()
        {
            if (Query == null)
            {
                throw new SqlSyntaxException(this, Error.QueryMissing);
            }
            return string.Format("({0})", Query.Expression);
        }
    }

    /// <summary>
    /// 一元表达式
    /// </summary>
    public abstract class UnaryExpression : ExpressionBase<UnaryExpression>, IUnaryExpression
    {
        public UnaryExpression(ISimpleValue a, IUnaryOperator op)
        {
            A = a;
            Op = op;
        }

        /// <summary>
        /// 操作数
        /// </summary>
        public ISimpleValue A { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public IUnaryOperator Op { get; set; }

        /// <summary>
        /// 是否括号括起来
        /// </summary>
        public bool WithBracket { get; set; }

        protected override string GenExpression()
        {
            if (Op == null)
            {
                throw new SqlSyntaxException(this, Error.OperatorMissing);
            }
            if (A == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            if ((A is IBinaryExpression && !(A as IBinaryExpression).WithBracket) || (A is IUnaryExpression && !(A as IUnaryExpression).WithBracket))
            {
                return string.Format(WithBracket ? "(({0}){1})" : "({0}){1}", A.Expression, Op);
            }
            else
            {
                return string.Format(WithBracket ? "({0}{1})" : "{0}{1}", A.Expression, Op);
            }
        }
    }
    /// <summary>
    /// 二元表达式
    /// </summary>
    public class BinaryExpression : ExpressionBase<BinaryExpression>, IBinaryExpression
    {

        public BinaryExpression(ISimpleValue a, IBinaryOperator op, ISimpleValue b)
        {
            A = a;
            Op = op;
            B = b;
        }

        /// <summary>
        /// 操作数
        /// </summary>
        public ISimpleValue A { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public IBinaryOperator Op { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        public ISimpleValue B { get; set; }

        /// <summary>
        /// 是否括号括起来
        /// </summary>
        public bool WithBracket { get; set; }

        protected override string GenExpression()
        {
            if (Op == null)
            {
                throw new SqlSyntaxException(this, Error.OperatorMissing);
            }
            if (A == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            if (B == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            return string.Format(WithBracket ? "({0}{1}{2})" : "{0}{1}{2}", A.Expression, Op, B.Expression);
        }
    }

    /// <summary>
    /// 一元比较表达式
    /// </summary>
    public class UnaryComparisonExpression : UnaryExpression, IUnaryComparisonExpression
    {
        public UnaryComparisonExpression(ISimpleValue a, IUnaryComparisonOperator op)
            : base(a, op)
        {
        }

        public static LogicExpression operator &(UnaryComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(UnaryComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }
    }

    /// <summary>
    /// 二元比较表达式
    /// </summary>
    public class ComparisonExpression : BinaryExpression, IComparisonExpression
    {
        public ComparisonExpression(ISimpleValue a, IComparisonOperator op, ISimpleValue b)
            : base(a, op, b)
        {
        }

        public static LogicExpression operator &(ComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(ComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }
    }

    public class ExistsExpression : ExpressionBase<ExistsExpression>, IExistsExpression
    {
        public ExistsExpression(ISubQueryExpression subquery)
        {
            SubQuery = subquery;
        }

        public ISubQueryExpression SubQuery { get; set; }

        protected override string GenExpression()
        {
            if (SubQuery == null)
            {
                throw new SqlSyntaxException(this, Error.SubQueryMissing);
            }
            return string.Format("EXISTS {0}", SubQuery.Expression);
        }
    }

    public class NotExistsExpression : ExpressionBase<NotExistsExpression>, INotExistsExpression
    {
        public NotExistsExpression(ISubQueryExpression subquery)
        {
            SubQuery = subquery;
        }

        public ISubQueryExpression SubQuery { get; set; }

        protected override string GenExpression()
        {
            if (SubQuery == null)
            {
                throw new SqlSyntaxException(this, Error.SubQueryMissing);
            }
            return string.Format("NOT EXISTS {0}", SubQuery.Expression);
        }
    }

    public class BetweenExpression : ExpressionBase<BetweenExpression>, IBetweenExpression
    {
        public BetweenExpression(ISimpleValue value, ISimpleValue lower, ISimpleValue upper)
        {
            Value = value;
            Lower = lower;
            Upper = upper;
        }

        public ISimpleValue Value { get; set; }

        public ISimpleValue Lower { get; set; }

        public ISimpleValue Upper { get; set; }

        protected override string GenExpression()
        {
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            if (Lower == null)
            {
                throw new SqlSyntaxException(this, Error.BetweenLowerMissing);
            }
            if (Upper == null)
            {
                throw new SqlSyntaxException(this, Error.BetweenUpperMissing);
            }
            return string.Format("{0} BETWEEN {1} AND {2}", Value.Expression, Lower.Expression, Upper.Expression);
        }
    }

    public class NotBetweenExpression : ExpressionBase<NotBetweenExpression>, INotBetweenExpression
    {
        public NotBetweenExpression(ISimpleValue value, ISimpleValue lower, ISimpleValue upper)
        {
            Value = value;
            Lower = lower;
            Upper = upper;
        }

        public ISimpleValue Value { get; set; }

        public ISimpleValue Lower { get; set; }

        public ISimpleValue Upper { get; set; }

        protected override string GenExpression()
        {
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            if (Lower == null)
            {
                throw new SqlSyntaxException(this, Error.BetweenLowerMissing);
            }
            if (Upper == null)
            {
                throw new SqlSyntaxException(this, Error.BetweenUpperMissing);
            }
            return string.Format("{0} NOT BETWEEN {1} AND {2}", Value.Expression, Lower.Expression, Upper.Expression);
        }
    }

    public class InExpression : ExpressionBase<InExpression>, IInExpression
    {
        public InExpression(ISimpleValue value, ICollection collection)
        {
            Value = value;
            Collection = collection;
        }

        public ISimpleValue Value { get; set; }

        public ICollection Collection { get; set; }

        protected override string GenExpression()
        {
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            if (Collection == null)
            {
                throw new SqlSyntaxException(this, Error.CollectionMissing);
            }
            return string.Format("{0} IN {1}", Value.Expression, Collection.Expression);
        }
    }

    public class NotInExpression : ExpressionBase<NotInExpression>, INotInExpression
    {
        public NotInExpression(ISimpleValue value, ICollection collection)
        {
            Value = value;
            Collection = collection;
        }

        public ISimpleValue Value { get; set; }

        public ICollection Collection { get; set; }

        protected override string GenExpression()
        {
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            if (Collection == null)
            {
                throw new SqlSyntaxException(this, Error.CollectionMissing);
            }
            return string.Format("{0} NOT IN {1}", Value.Expression, Collection.Expression);
        }
    }


    /// <summary>
    /// 逻辑表达式
    /// </summary>
    public class LogicExpression : BinaryExpression, ILogicExpression
    {
        public LogicExpression(ISimpleValue a, ILogicOperator op, ISimpleValue b)
            : base(a, op, b)
        {
        }

        public static LogicExpression operator &(LogicExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(LogicExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }
    }

    /// <summary>
    /// 算术表达式
    /// </summary>
    public class ArithmeticExpression : BinaryExpression, IArithmeticExpression
    {
        public ArithmeticExpression(ISimpleValue a, IArithmeticOperator op, ISimpleValue b)
            : base(a, op, b)
        {
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(ArithmeticExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(ArithmeticExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(ArithmeticExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(ArithmeticExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(ArithmeticExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(ArithmeticExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValue val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Eq, exp);
        }
        public static ComparisonExpression operator !=(LiteralValue val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Neq, exp);
        }
        public static ComparisonExpression operator >(LiteralValue val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Gt, exp);
        }
        public static ComparisonExpression operator <(LiteralValue val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.Lt, exp);
        }
        public static ComparisonExpression operator >=(LiteralValue val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, exp);
        }
        public static ComparisonExpression operator <=(LiteralValue val, ArithmeticExpression exp)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, exp);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ArithmeticExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(ArithmeticExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(ArithmeticExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(ArithmeticExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(ArithmeticExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(ArithmeticExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValue val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Add, exp);
        }
        public static ArithmeticExpression operator -(LiteralValue val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Sub, exp);
        }
        public static ArithmeticExpression operator *(LiteralValue val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Mul, exp);
        }
        public static ArithmeticExpression operator /(LiteralValue val, ArithmeticExpression exp)
        {
            return new ArithmeticExpression(val, Operator.Div, exp);
        }
        public static ArithmeticExpression operator %(LiteralValue val, ArithmeticExpression exp)
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
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 函数表达式
    /// </summary>
    public class FunctionExpression : ExpressionBase<FunctionExpression>, IFunctionExpression
    {
        public FunctionExpression(string name, params ISimpleValue[] values)
        {
            Name = name;
            Values = values;
        }


        public string Name { get; set; }

        public ISimpleValue[] Values { get; set; }

        protected override string GenExpression()
        {
            if (Name == null)
            {
                throw new SqlSyntaxException(this, Error.FunctionNameMissing);
            }
            return string.Format("{0}({1})", Name, Values == null ? string.Empty : Values.Join(",", v => v.Expression));
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(FunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(FunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(FunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(FunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(FunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(FunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(FunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(FunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(FunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(FunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(FunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(FunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValue val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Eq, fun);
        }
        public static ComparisonExpression operator !=(LiteralValue val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Neq, fun);
        }
        public static ComparisonExpression operator >(LiteralValue val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Gt, fun);
        }
        public static ComparisonExpression operator <(LiteralValue val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Lt, fun);
        }
        public static ComparisonExpression operator >=(LiteralValue val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, fun);
        }
        public static ComparisonExpression operator <=(LiteralValue val, FunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, fun);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(FunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(FunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(FunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(FunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(FunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(FunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(FunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(FunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(FunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(FunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValue val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Add, fun);
        }
        public static ArithmeticExpression operator -(LiteralValue val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Sub, fun);
        }
        public static ArithmeticExpression operator *(LiteralValue val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Mul, fun);
        }
        public static ArithmeticExpression operator /(LiteralValue val, FunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Div, fun);
        }
        public static ArithmeticExpression operator %(LiteralValue val, FunctionExpression fun)
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
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 筛选条件子句 
    /// </summary>
    public class WhereClause : ExpressionBase<WhereClause>, IWhereClause
    {
        public WhereClause(ISimpleValue filter)
        {
            Filter = filter;
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public ISimpleValue Filter { get; set; }

        protected override string GenExpression()
        {
            if (Filter == null)
            {
                throw new SqlSyntaxException(this, Error.FilterMissing);
            }
            return string.Format("WHERE {0}", Filter.Expression);
        }
    }

    /// <summary>
    /// 插入语句
    /// </summary>
    public class InsertStatement : ExpressionBase<InsertStatement>, IInsertStatement
    {
        public InsertStatement() : this(null) { }

        public InsertStatement(ITable table)
            : this(table, new IColumn[0], new ISimpleValue[0])
        { }

        public InsertStatement(ITable table, IColumn[] columns, ISimpleValue[] values)
            : this(table, columns, new ValueCollectionExpression(values))
        {

        }

        public InsertStatement(ITable table, IColumn[] columns, ICollection values)
        {
            Table = table;
            Columns = columns;
            Values = values;
        }

        public ITable Table { get; set; }

        public IColumn[] Columns { get; set; }

        public ICollection Values { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                List<string> list = new List<string>();
                if (Values is ICustomerExpression) list.AddRange((Values as ICustomerExpression).Params);
                else if (Values is IValueCollectionExpression)
                {
                    foreach (var val in (Values as IValueCollectionExpression).Values)
                    {
                        if ((val is IParam)) list.Add((val as IParam).Name);
                    }
                }
                return list.Distinct();
            }
        }

        protected override string GenExpression()
        {
            if (Table == null)
            {
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            if (Columns == null || Columns.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.ColumnsMissing);
            }
            if (Values == null)
            {
                throw new SqlSyntaxException(this, Error.ValuesMissing);
            }
            return string.Format("INSERT INTO {0}({1}) VALUES{2}", Table.Expression, Columns.Join(",", p => p.Expression), Values.Expression);
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

        public DeleteStatement(ITable table)
            : this(table, null)
        { }

        public DeleteStatement(ITable table, IWhereClause where)
        {
            Table = table;
            Where = where;
        }

        public ITable Table { get; set; }

        public IWhereClause Where { get; set; }

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
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            return string.Format("DELETE FROM {0} {1}", Table.Expression, Where?.Expression).TrimEnd();
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

        public UpdateStatement(ITableAliasExpression table)
            : this(table, new SetClause(null), null)
        {
        }

        public UpdateStatement(ITableAliasExpression table, ISetClause set, IWhereClause where)
        {
            Table = table;
            Set = set;
            Where = where;
        }

        public ITableAliasExpression Table { get; set; }

        public ISetClause Set { get; set; }

        public IWhereClause Where { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                var list = new List<string>();
                foreach (var item in Set.SetFields)
                {
                    if (item.Value is ICustomerExpression)
                    {
                        list.AddRange((item.Value as ICustomerExpression).Params);
                    }
                    else if (item.Value is IParam)
                    {
                        list.Add((item.Value as IParam).Name);
                    }
                }
                if (Where != null) list.AddRange(Where.Filter.Params());
                return list.Distinct();
            }
        }

        protected override string GenExpression()
        {
            if (Table == null)
            {
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            if (Set == null)
            {
                throw new SqlSyntaxException(this, Error.SetClauseMissing);
            }
            return string.Format("UPDATE {0} {1} {2}",
                Table.Expression,
                Set.Expression,
                Where?.Expression).TrimEnd();
        }
    }

    /// <summary>
    /// 更新赋值项
    /// </summary>
    public class SetFieldExpression : ExpressionBase<SetFieldExpression>, ISetFieldExpression
    {
        public SetFieldExpression(IColumn column, ISimpleValue value)
        {
            Column = column;
            Value = value;
        }

        public SetFieldExpression(IColumn column)
            : this(column, new Param(column.Name))
        {
        }

        public IColumn Column { get; set; }

        public ISimpleValue Value { get; set; }

        protected override string GenExpression()
        {
            if (Column == null)
            {
                throw new SqlSyntaxException(this, Error.ColumnMissing);
            }
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            return string.Format("{0}={1}", Column.Expression, Value.Expression);
        }
    }

    /// <summary>
    /// 更新赋值子句
    /// </summary>
    public class SetClause : ExpressionBase<SetClause>, ISetClause
    {
        public SetClause(ISetFieldExpression[] sets)
        {
            SetFields = sets;
        }
        public ISetFieldExpression[] SetFields { get; set; }

        protected override string GenExpression()
        {
            if (SetFields == null || SetFields.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.SetClauseFieldsMissing);
            }
            return string.Format("SET {0}", SetFields.Join(",", set => set.Expression));
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

        public SelectStatement(params IDatasetWithAlias[] tables)
            : this(tables, new ISelectItemExpression[0], null)
        { }

        public SelectStatement(IDatasetWithAlias[] tables, ISelectItemExpression[] fields, IWhereClause where, IOrderByClause orderby = null)
            : this(tables, fields, where, null, null, orderby)
        { }

        public SelectStatement(IDatasetWithAlias[] tables, ISelectItemsExpression fields, IWhereClause where, IOrderByClause orderby = null)
            : this(tables, fields, where, null, null, orderby)
        { }

        public SelectStatement(IDatasetWithAlias[] tables, ISelectItemExpression[] fields, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
            : this(tables, fields, null, where, groupby, having, orderby)
        { }

        public SelectStatement(IDatasetWithAlias[] tables, ISelectItemsExpression fields, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
            : this(tables, fields, null, where, groupby, having, orderby)
        { }

        public SelectStatement(IDatasetWithAlias[] tables, ISelectItemExpression[] fields, IJoinExpression[] joins, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
            : this(tables, new SelectItemsExpression(fields), joins, where, groupby, having, orderby)
        { }

        public SelectStatement(IDatasetWithAlias[] tables, ISelectItemsExpression fields, IJoinExpression[] joins, IWhereClause where, IGroupByClause groupby, IHavingClause having, IOrderByClause orderby = null)
        {
            Tables = tables;
            Items = fields;
            Joins = joins;
            Where = where;
            GroupBy = groupby;
            Having = having;
            OrderBy = orderby;
        }

        public IDatasetWithAlias[] Tables { get; set; }

        public ISelectItemsExpression Items { get; set; }

        public IJoinExpression[] Joins { get; set; }

        public IWhereClause Where { get; set; }

        public IGroupByClause GroupBy { get; set; }

        public IHavingClause Having { get; set; }

        public IOrderByClause OrderBy { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                var list = new List<string>();
                foreach(var field in Items.Fields)
                {
                    list.AddRange(field.Field.Params());
                }
                if (Having != null) list.AddRange(Having.Filter.Params());
                if (Where != null) list.AddRange(Where.Filter.Params());
                if (Joins != null)
                {
                    foreach (var join in Joins)
                    {
                        if (join?.On != null) list.AddRange(join.On.Condition.Params());
                    }
                }
                return list.Distinct();
            }
        }

        protected override string GenExpression()
        {
            if (Tables == null || Tables.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            if (Items == null || Items.Fields.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.SelectFieldsMissing);
            }
            return string.Format("SELECT {0} FROM {1}", Items.Fields.Join(",", s => s.Expression),
                string.Join(" ", new string[] {
                        Tables.Join(",", t => t.Expression),
                        Joins?.Join(" ", j => j?.Expression),
                        Where?.Expression,
                        GroupBy?.Expression,
                        string.IsNullOrWhiteSpace(GroupBy?.Expression) ? string.Empty : Having?.Expression,
                        OrderBy?.Expression}.Where(s => !string.IsNullOrWhiteSpace(s))))
               .TrimEnd();
        }
    }

    /// <summary>
    /// 合并查询语句
    /// </summary>
    public class UnionStatement : ExpressionBase<UnionStatement>, IUnionStatement
    {
        public UnionStatement(ISelectStatement select, IUnionExpression[] unions, IOrderByClause orderBy)
        {
            Select = select;
            Unions = unions;
            OrderBy = orderBy;
        }

        public ISelectStatement Select { get; set; }

        public IUnionExpression[] Unions { get; set; }

        public IOrderByClause OrderBy { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                var list = Select.Params;
                foreach (var item in Unions)
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
                throw new SqlSyntaxException(this, Error.UnionSelectMissing);
            }
            return (new string[] {
                Select.Expression,
                Unions?.Join(" ", union => union.Expression),
                OrderBy?.Expression
            }).Where(s => !string.IsNullOrWhiteSpace(s)).Join(" ");
        }
    }

    /// <summary>
    /// 合并查询子项
    /// </summary>
    public class UnionExpression : ExpressionBase<UnionExpression>, IUnionExpression
    {
        public UnionExpression(IUnionOperator unionOp, ISelectStatement select)
        {
            UnionOp = unionOp;
            Select = select;
        }


        public IUnionOperator UnionOp { get; set; }

        public ISelectStatement Select { get; set; }

        protected override string GenExpression()
        {
            if (UnionOp == null)
            {
                throw new SqlSyntaxException(this, Error.UnionOperatorMissing);
            }
            if (Select == null)
            {
                throw new SqlSyntaxException(this, Error.UnionSelectMissing);
            }
            return string.Format("{0} {1}", UnionOp, Select.Expression);
        }
    }

    /// <summary>
    /// 查询项列表表达式
    /// </summary>
    public class SelectItemsExpression : ExpressionBase<SelectItemsExpression>, ISelectItemsExpression
    {
        public SelectItemsExpression(ISelectItemExpression[] fields)
        {
            Fields = fields;
        }

        public ISelectItemExpression[] Fields { get; set; }

        protected override string GenExpression()
        {
            if (Fields == null || Fields.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.SelectFieldsMissing);
            }
            return Fields.Join(",", s => s.Expression);
        }
    }

    /// <summary>
    /// 查询项列表去重表达式
    /// </summary>
    public class DistinctSelectItemsExpression : ExpressionBase<DistinctSelectItemsExpression>, IDistinctSelectItemsExpression
    {
        public DistinctSelectItemsExpression(ISelectItemExpression[] fields)
        {
            Fields = fields;
        }

        public ISelectItemExpression[] Fields { get; set; }

        protected override string GenExpression()
        {
            if (Fields == null || Fields.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.SelectFieldsMissing);
            }
            return string.Format("DISTINCT {0}", Fields.Join(",", s => s.Expression));
        }
    }

    /// <summary>
    /// 查询项表达式
    /// </summary>
    public class SelectItemExpression : ExpressionBase<SelectItemExpression>, ISelectItemExpression
    {
        public SelectItemExpression(ISimpleValue field, ISelectFieldAlias alias = null)
        {
            if (field is ISelectItemExpression)
            {
                throw new ArgumentException(nameof(field));
            }
            Field = field;
            Alias = alias;
        }

        public ISimpleValue Field { get; set; }

        public ISelectFieldAlias Alias { get; set; }

        protected override string GenExpression()
        {
            if (Field == null)
            {
                throw new SqlSyntaxException(this, Error.FieldMissing);
            }
            if (string.IsNullOrWhiteSpace(Alias?.Name))
            {
                return Field.Expression;
            }
            else
            {
                return string.Format("{0} AS {1}", Field.Expression, Alias.Expression);
            }
        }
    }

    /// <summary>
    /// 查询项别名
    /// </summary>
    public class SelectFieldAlias : ExpressionBase<SelectFieldAlias>, ISelectFieldAlias
    {
        public SelectFieldAlias(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        protected override string GenExpression()
        {
            if (Name == null)
            {
                throw new SqlSyntaxException(this, Error.SelectFieldNameMissing);
            }
            return Name;
        }
    }

    /// <summary>
    /// 所有项 *
    /// </summary>
    public class AllFieldssExpression : Column
    {
        public AllFieldssExpression(IDatasetAlias dataset = null)
            : base("*", dataset)
        {
        }
    }

    /// <summary>
    /// 查询联接
    /// </summary>
    public class JoinExpression : ExpressionBase<JoinExpression>, IJoinExpression
    {
        public JoinExpression(IJoinOperator joinOp, IDatasetWithAlias table, ISimpleValue on)
            : this(joinOp, table, new OnClause(on))
        {
        }
        public JoinExpression(IJoinOperator joinOp, IDatasetWithAlias table, IOnClause on)
        {
            JoinOp = joinOp;
            Table = table;
            On = on;
        }

        public IJoinOperator JoinOp { get; set; }

        public IDatasetWithAlias Table { get; set; }

        public IOnClause On { get; set; }

        protected override string GenExpression()
        {
            if (JoinOp == null)
            {
                throw new SqlSyntaxException(this, Error.OperatorMissing);
            }
            if (Table == null)
            {
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            return string.Format("{0} {1}{2}", JoinOp, Table.Expression, On?.Expression);
        }
    }
    
    /// <summary>
    /// On子句
    /// </summary>
    public class OnClause : ExpressionBase<OnClause>, IOnClause
    {
        public OnClause(ISimpleValue condition)
        {
            this.Condition = condition;
        }

        /// <summary>
        /// 连接条件
        /// </summary>
        public ISimpleValue Condition { get; set; }

        protected override string GenExpression()
        {
            if (Condition == null)
            {
                return string.Empty;
            }

            return string.Format(" ON {0}", Condition.Expression);
        }

    }

    /// <summary>
    /// 分组子句
    /// </summary>
    public class GroupByClause : ExpressionBase<GroupByClause>, IGroupByClause
    {
        public GroupByClause(params ISimpleValue[] fields)
        {
            Fields = fields;
        }

        public ISimpleValue[] Fields { get; set; }

        protected override string GenExpression()
        {
            if (Fields == null || Fields.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.GroupByFieldsMissing);
            }
            return string.Format("GROUP BY {0}", Fields.Join(",", val => val.Expression));
        }
    }

    /// <summary>
    /// 分组条件
    /// </summary>
    public class HavingClause : ExpressionBase<HavingClause>, IHavingClause
    {
        public HavingClause(ISimpleValue filter)
        {
            Filter = filter;
        }

        public ISimpleValue Filter { get; set; }

        protected override string GenExpression()
        {
            if (Filter == null)
            {
                throw new SqlSyntaxException(this, Error.FilterMissing);
            }
            return string.Format("HAVING {0}", Filter.Expression);
        }
    }

    /// <summary>
    /// 聚合函数表达式
    /// </summary>
    public class AggregateFunctionExpression : FunctionExpression, IAggregateFunctionExpression
    {
        public static AggregateFunctionExpression Count(ISimpleValue col)
        {
            return new AggregateFunctionExpression("COUNT", col);
        }

        public static AggregateFunctionExpression Sum(ISimpleValue col)
        {
            return new AggregateFunctionExpression("SUM", col);
        }

        public static AggregateFunctionExpression Avg(ISimpleValue col)
        {
            return new AggregateFunctionExpression("AVG", col);
        }

        public static AggregateFunctionExpression Max(ISimpleValue col)
        {
            return new AggregateFunctionExpression("MAX", col);
        }

        public static AggregateFunctionExpression Min(ISimpleValue col)
        {
            return new AggregateFunctionExpression("MIN", col);
        }

        public AggregateFunctionExpression(string name, ISimpleValue value)
            : base(name, value)
        {

        }
    }

    /// <summary>
    /// 排序子句
    /// </summary>
    public class OrderByClause : ExpressionBase<OrderByClause>, IOrderByClause
    {
        public OrderByClause(params IOrderExpression[] orders)
        {
            Orders = orders;
        }

        public IOrderExpression[] Orders { get; set; }

        protected override string GenExpression()
        {
            if (Orders == null || Orders.Length == 0)
            {
                throw new SqlSyntaxException(this, Error.OrderByFieldsMissing);
            }
            return string.Format("ORDER BY {0}", Orders.Join(",", order => order.Expression));
        }
    }

    /// <summary>
    /// 排序项
    /// </summary>
    public class OrderExpression : ExpressionBase<OrderExpression>, IOrderExpression
    {
        public OrderExpression(ISimpleValue value, OrderEnum order = OrderEnum.Asc)
        {
            Field = value;
            Order = order;
        }

        public ISimpleValue Field { get; set; }

        public OrderEnum Order { get; set; } = OrderEnum.Asc;

        protected override string GenExpression()
        {
            if (Field == null)
            {
                throw new SqlSyntaxException(this, Error.FieldMissing);
            }
            return string.Format("{0} {1}", Field.Expression, Order == OrderEnum.Asc ? "ASC" : "DESC");
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
                return this.Params();
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
        public static IEnumerable<string> Params(this ISimpleValue simpleValue)
        {
            if (simpleValue is ICustomerExpression)
            {
                return (simpleValue as ICustomerExpression).Params();
            }
            else if (simpleValue is IBinaryExpression)
            {
                return GetParam(simpleValue as IBinaryExpression);
            }
            else if (simpleValue is IUnaryExpression)
            {
                return GetParam(simpleValue as IUnaryExpression);
            }
            else  if(simpleValue is IFunctionExpression)
            {
                return GetParam(simpleValue as IFunctionExpression);
            }
            else if (simpleValue is IParam)
            {
                return new List<string>() { (simpleValue as IParam).Name };
            }
            else
            {
                return new List<string>();
            }
        }

        public static List<string> Params(this ICustomerExpression customer)
        {
            var list = new List<string>();
            var matchs = Regex.Matches(customer.Expression, "(?<=@)[_a-zA-Z]+[_a-zA-Z0-9]*(?=[^a-zA-Z0-9]|$)");
            foreach (Match match in matchs)
            {
                list.Add(match.Value);
            }
            return list.Distinct().ToList();
        }

        private static List<string> GetParam(IBinaryExpression binary)
        {
            List<string> list = new List<string>();
            if (binary.A is ICustomerExpression)
            {
                list.AddRange((binary.A as ICustomerExpression).Params());
            }
            else if (binary.A is IParam)
            {
                list.Add((binary.A as IParam).Name);
            }
            else if (binary.A is IUnaryExpression)
            {
                list.AddRange(GetParam(binary.A as IUnaryExpression));
            }
            else if (binary.A is IBinaryExpression)
            {
                list.AddRange(GetParam(binary.A as IBinaryExpression));
            }
            else if (binary.A is IFunctionExpression)
            {
                list.AddRange(GetParam(binary.A as IFunctionExpression));
            }

            if (binary.B is ICustomerExpression)
            {
                list.AddRange((binary.B as ICustomerExpression).Params());
            }
            else if (binary.B is IParam)
            {
                list.Add((binary.B as IParam).Name);
            }
            else if (binary.B is IUnaryExpression)
            {
                list.AddRange(GetParam(binary.B as IUnaryExpression));
            }
            else if (binary.B is IBinaryExpression)
            {
                list.AddRange(GetParam(binary.B as IBinaryExpression));
            }
            else if (binary.B is IFunctionExpression)
            {
                list.AddRange(GetParam(binary.B as IFunctionExpression));
            }

            return list.Distinct().ToList();
        }

        private static List<string> GetParam(IUnaryExpression unaray)
        {
            List<string> list = new List<string>();
            if (unaray.A is ICustomerExpression)
            {
                list.AddRange((unaray.A as ICustomerExpression).Params());
            }
            else if (unaray.A is IParam)
            {
                list.Add((unaray.A as IParam).Name);
            }
            else if (unaray.A is IUnaryExpression)
            {
                list.AddRange(GetParam(unaray.A as IUnaryExpression));
            }
            else if (unaray.A is IBinaryExpression)
            {
                list.AddRange(GetParam(unaray.A as IBinaryExpression));
            }
            else if (unaray.A is IFunctionExpression)
            {
                list.AddRange(GetParam(unaray.A as IFunctionExpression));
            }
            return list.Distinct().ToList();
        }

        private static List<string> GetParam(IFunctionExpression function)
        {
            List<string> list = new List<string>();
            foreach(var value  in function.Values)
            {
                if (value is ICustomerExpression)
                {
                    list.AddRange((value as ICustomerExpression).Params());
                }
                else if (value is IParam)
                {
                    list.Add((value as IParam).Name);
                }
                else if (value is IUnaryExpression)
                {
                    list.AddRange(GetParam(value as IUnaryExpression));
                }
                else if (value is IBinaryExpression)
                {
                    list.AddRange(GetParam(value as IBinaryExpression));
                }
                else if(value is IFunctionExpression)
                {
                    list.AddRange(GetParam(value as IFunctionExpression));
                }
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