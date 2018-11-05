using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class Expression
    {
        public static string OpenQuotationMark { get; set; } = string.Empty;
        public static string CloseQuotationMark { get; set; } = string.Empty;
        public static string ParamMark { get; set; } = "@";
    }

    /// <summary>
    /// 表达式抽象类
    /// </summary>
    public abstract class ExpressionBase<Exp> : IExpression
        where Exp : class, IExpression
    {

        public ExpressionBase()
        {

        }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression
        {
            get
            {
                return Build();
            }
        }

        /// <summary>
        /// 构建表达式
        /// </summary>
        protected abstract string Build();

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
    public class Table : ExpressionBase<Table>, ITable
    {
        public Table(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string Build()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new SqlSyntaxException(this, Error.TableNameMissing);
            }
            return string.Format("{1}{0}{2}", Name, SqlExpression.Expression.OpenQuotationMark, SqlExpression.Expression.CloseQuotationMark);
        }

        public static implicit operator Table(string name)
        {
            return new Table(name);
        }
    }

    /// <summary>
    /// 字段
    /// </summary>
    public class Field : ExpressionBase<Field>, IField
    {
        public Field(string name, string dataset = null)
        {
            Name = name;
            DatasetAlias = dataset;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所在数据集别名
        /// </summary>
        public string DatasetAlias { get; set; }

        protected override string Build()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new SqlSyntaxException(this, Error.FieldNameMissing);
            }
            if (DatasetAlias == null)
            {
                return Name;
            }
            else
            {
                return string.Format("{2}{1}{3}.{2}{0}{3}", Name, DatasetAlias, SqlExpression.Expression.OpenQuotationMark, SqlExpression.Expression.CloseQuotationMark);
            }
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(Field field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(Field field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(Field field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(Field field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(Field field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(Field field, ISimpleValue val)
        {
            return new ComparisonExpression(field, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(Field field, LiteralValue val)
        {
            return new ComparisonExpression(field, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(Field field, LiteralValue val)
        {
            return new ComparisonExpression(field, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(Field field, LiteralValue val)
        {
            return new ComparisonExpression(field, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(Field field, LiteralValue val)
        {
            return new ComparisonExpression(field, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(Field field, LiteralValue val)
        {
            return new ComparisonExpression(field, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(Field field, LiteralValue val)
        {
            return new ComparisonExpression(field, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(LiteralValue val, Field field)
        {
            return new ComparisonExpression(val, Operator.Eq, field);
        }
        public static ComparisonExpression operator !=(LiteralValue val, Field field)
        {
            return new ComparisonExpression(val, Operator.Neq, field);
        }
        public static ComparisonExpression operator >(LiteralValue val, Field field)
        {
            return new ComparisonExpression(val, Operator.Gt, field);
        }
        public static ComparisonExpression operator <(LiteralValue val, Field field)
        {
            return new ComparisonExpression(val, Operator.Lt, field);
        }
        public static ComparisonExpression operator >=(LiteralValue val, Field field)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, field);
        }
        public static ComparisonExpression operator <=(LiteralValue val, Field field)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, field);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(Field field, ISimpleValue val)
        {
            return new ArithmeticExpression(field, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(Field field, ISimpleValue val)
        {
            return new ArithmeticExpression(field, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(Field field, ISimpleValue val)
        {
            return new ArithmeticExpression(field, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(Field field, ISimpleValue val)
        {
            return new ArithmeticExpression(field, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(Field field, ISimpleValue val)
        {
            return new ArithmeticExpression(field, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(Field field, LiteralValue val)
        {
            return new ArithmeticExpression(field, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(Field field, LiteralValue val)
        {
            return new ArithmeticExpression(field, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(Field field, LiteralValue val)
        {
            return new ArithmeticExpression(field, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(Field field, LiteralValue val)
        {
            return new ArithmeticExpression(field, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(Field field, LiteralValue val)
        {
            return new ArithmeticExpression(field, Operator.Mod, val);
        }


        public static ArithmeticExpression operator +(LiteralValue val, Field field)
        {
            return new ArithmeticExpression(val, Operator.Add, field);
        }
        public static ArithmeticExpression operator -(LiteralValue val, Field field)
        {
            return new ArithmeticExpression(val, Operator.Sub, field);
        }
        public static ArithmeticExpression operator *(LiteralValue val, Field field)
        {
            return new ArithmeticExpression(val, Operator.Mul, field);
        }
        public static ArithmeticExpression operator /(LiteralValue val, Field field)
        {
            return new ArithmeticExpression(val, Operator.Div, field);
        }
        public static ArithmeticExpression operator %(LiteralValue val, Field field)
        {
            return new ArithmeticExpression(val, Operator.Mod, field);
        }

        #endregion

        #region 逻辑运算符

        public static LogicExpression operator &(Field exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(Field exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion

        #region 隐式转换

        public static implicit operator Field(string field)
        {
            return new Field(field);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is Field)
            {
                return (obj as Field).Name == this.Name;
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

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        protected override string Build()
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

        public static implicit operator LiteralValue(bool value)
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
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

        /// <summary>
        /// 参数名 不包含@
        /// </summary>
        public string Name { get; set; }

        protected override string Build()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new SqlSyntaxException(this, Error.ParamNameMissing);
            }
            return string.Format("{0}{1}", Name, SqlExpression.Expression.ParamMark);
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

        #region 逻辑运算符

        public static LogicExpression operator &(Param exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(Param exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion

        #region 隐式转换

        public static implicit operator Param(string param)
        {
            return new Param(param);
        }

        public static implicit operator Param(Field field)
        {
            return new Param(field.Name);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is Param)
            {
                return (obj as Param).Name == this.Name;
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
    /// 集合（In|Insert）
    /// </summary>
    public class ValueCollectionExpression : ExpressionBase<ValueCollectionExpression>, IValueCollectionExpression
    {
        public ValueCollectionExpression(IList<ISimpleValue> values)
        {
            Values = values;
        }

        /// <summary>
        /// 值列表
        /// </summary>
        public IList<ISimpleValue> Values { get; set; }

        protected override string Build()
        {
            if (Values == null || Values.Count == 0)
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

    /// <summary>
    /// 子查询表达式
    /// </summary>
    public class SubQueryExpression : ExpressionBase<SubQueryExpression>, ISubQueryExpression
    {
        public SubQueryExpression(ISelectStatement query)
        {
            Query = query;
        }

        /// <summary>
        /// 查询语句
        /// </summary>
        public ISelectStatement Query { get; set; }

        protected override string Build()
        {
            if (Query == null)
            {
                throw new SqlSyntaxException(this, Error.QueryMissing);
            }
            return string.Format("({0})", Query.Expression);
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ComparisonExpression(subquery, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ComparisonExpression(subquery, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ComparisonExpression(subquery, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ComparisonExpression(subquery, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ComparisonExpression(subquery, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ComparisonExpression(subquery, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(SubQueryExpression subquery, LiteralValue val)
        {
            return new ComparisonExpression(subquery, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(SubQueryExpression subquery, LiteralValue val)
        {
            return new ComparisonExpression(subquery, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(SubQueryExpression subquery, LiteralValue val)
        {
            return new ComparisonExpression(subquery, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(SubQueryExpression subquery, LiteralValue val)
        {
            return new ComparisonExpression(subquery, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(SubQueryExpression subquery, LiteralValue val)
        {
            return new ComparisonExpression(subquery, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(SubQueryExpression subquery, LiteralValue val)
        {
            return new ComparisonExpression(subquery, Operator.LtOrEq, val);
        }


        public static ComparisonExpression operator ==(LiteralValue val, SubQueryExpression subquery)
        {
            return new ComparisonExpression(val, Operator.Eq, subquery);
        }
        public static ComparisonExpression operator !=(LiteralValue val, SubQueryExpression subquery)
        {
            return new ComparisonExpression(val, Operator.Neq, subquery);
        }
        public static ComparisonExpression operator >(LiteralValue val, SubQueryExpression subquery)
        {
            return new ComparisonExpression(val, Operator.Gt, subquery);
        }
        public static ComparisonExpression operator <(LiteralValue val, SubQueryExpression subquery)
        {
            return new ComparisonExpression(val, Operator.Lt, subquery);
        }
        public static ComparisonExpression operator >=(LiteralValue val, SubQueryExpression subquery)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, subquery);
        }
        public static ComparisonExpression operator <=(LiteralValue val, SubQueryExpression subquery)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, subquery);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(SubQueryExpression subquery, ISimpleValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(SubQueryExpression subquery, LiteralValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(SubQueryExpression subquery, LiteralValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(SubQueryExpression subquery, LiteralValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(SubQueryExpression subquery, LiteralValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(SubQueryExpression subquery, LiteralValue val)
        {
            return new ArithmeticExpression(subquery, Operator.Mod, val);
        }

        public static ArithmeticExpression operator +(LiteralValue val, SubQueryExpression subquery)
        {
            return new ArithmeticExpression(val, Operator.Add, subquery);
        }
        public static ArithmeticExpression operator -(LiteralValue val, SubQueryExpression subquery)
        {
            return new ArithmeticExpression(val, Operator.Sub, subquery);
        }
        public static ArithmeticExpression operator *(LiteralValue val, SubQueryExpression subquery)
        {
            return new ArithmeticExpression(val, Operator.Mul, subquery);
        }
        public static ArithmeticExpression operator /(LiteralValue val, SubQueryExpression subquery)
        {
            return new ArithmeticExpression(val, Operator.Div, subquery);
        }
        public static ArithmeticExpression operator %(LiteralValue val, SubQueryExpression subquery)
        {
            return new ArithmeticExpression(val, Operator.Mod, subquery);
        }

        #endregion

        #region 逻辑运算符

        public static LogicExpression operator &(SubQueryExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(SubQueryExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion

        #region 隐式转换

        public static implicit operator SubQueryExpression(SelectStatement query)
        {
            return new SubQueryExpression(query);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 一元表达式
    /// </summary>
    public abstract class UnaryExpression : ExpressionBase<UnaryExpression>, IUnaryExpression
    {
        public UnaryExpression(ISimpleValue value, IUnaryOperator op)
        {
            Value = value;
            Op = op;
        }

        /// <summary>
        /// 操作数
        /// </summary>
        public ISimpleValue Value { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public IUnaryOperator Op { get; set; }

        protected override string Build()
        {
            if (Op == null)
            {
                throw new SqlSyntaxException(this, Error.OperatorMissing);
            }
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            return string.Format(Op.Format, Value.Expression);
        }
    }

    /// <summary>
    /// 二元表达式
    /// </summary>
    public abstract class BinaryExpression : ExpressionBase<BinaryExpression>, IBinaryExpression
    {
        public BinaryExpression(ISimpleValue a, IBinaryOperator op, ISimpleValue b)
        {
            Value1 = a;
            Op = op;
            Value2 = b;
        }

        /// <summary>
        /// 操作数
        /// </summary>
        public ISimpleValue Value1 { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public IBinaryOperator Op { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        public ISimpleValue Value2 { get; set; }

        protected override string Build()
        {
            if (Op == null)
            {
                throw new SqlSyntaxException(this, Error.OperatorMissing);
            }
            if (Value1 == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            if (Value2 == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            return string.Format(Op.Format, Value1.Expression, Value2.Expression);
        }
    }

    /// <summary>
    /// 括号表达式
    /// </summary>
    public class BracketExpression : ExpressionBase<BracketExpression>, IUnaryExpression
    {
        public BracketExpression(ISimpleValue val)
        {
            Value = val;
            Op = Operator.Bracket;
        }

        /// <summary>
        /// 操作数A
        /// </summary>
        public ISimpleValue Value { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public IUnaryOperator Op { get; set; }

        protected override string Build()
        {
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            return string.Format(Op.Format, Value.Expression);
        }

        #region 比较运算符

        public static ComparisonExpression operator ==(BracketExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(BracketExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(BracketExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(BracketExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(BracketExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(BracketExpression exp, ISimpleValue val)
        {
            return new ComparisonExpression(exp, Operator.LtOrEq, val);
        }

        public static ComparisonExpression operator ==(BracketExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(BracketExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(BracketExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(BracketExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(BracketExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(BracketExpression exp, LiteralValue val)
        {
            return new ComparisonExpression(exp, Operator.LtOrEq, val);
        }


        public static ComparisonExpression operator ==(LiteralValue val, BracketExpression value)
        {
            return new ComparisonExpression(val, Operator.Eq, value);
        }
        public static ComparisonExpression operator !=(LiteralValue val, BracketExpression value)
        {
            return new ComparisonExpression(val, Operator.Neq, value);
        }
        public static ComparisonExpression operator >(LiteralValue val, BracketExpression value)
        {
            return new ComparisonExpression(val, Operator.Gt, value);
        }
        public static ComparisonExpression operator <(LiteralValue val, BracketExpression value)
        {
            return new ComparisonExpression(val, Operator.Lt, value);
        }
        public static ComparisonExpression operator >=(LiteralValue val, BracketExpression value)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, value);
        }
        public static ComparisonExpression operator <=(LiteralValue val, BracketExpression value)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, value);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(BracketExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(BracketExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(BracketExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(BracketExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(BracketExpression exp, ISimpleValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mod, val);
        }


        public static ArithmeticExpression operator +(BracketExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(BracketExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(BracketExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(BracketExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(BracketExpression exp, LiteralValue val)
        {
            return new ArithmeticExpression(exp, Operator.Mod, val);
        }


        public static ArithmeticExpression operator +(LiteralValue val, BracketExpression value)
        {
            return new ArithmeticExpression(val, Operator.Add, value);
        }
        public static ArithmeticExpression operator -(LiteralValue val, BracketExpression value)
        {
            return new ArithmeticExpression(val, Operator.Sub, value);
        }
        public static ArithmeticExpression operator *(LiteralValue val, BracketExpression value)
        {
            return new ArithmeticExpression(val, Operator.Mul, value);
        }
        public static ArithmeticExpression operator /(LiteralValue val, BracketExpression value)
        {
            return new ArithmeticExpression(val, Operator.Div, value);
        }
        public static ArithmeticExpression operator %(LiteralValue val, BracketExpression value)
        {
            return new ArithmeticExpression(val, Operator.Mod, value);
        }

        #endregion

        #region 逻辑运算符

        public static LogicExpression operator &(BracketExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(BracketExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        #region 逻辑运算符

        public static LogicExpression operator &(UnaryComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(UnaryComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
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

        #region 逻辑运算符

        public static LogicExpression operator &(ComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(ComparisonExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
    }

    /// <summary>
    /// Exists表达式
    /// </summary>
    public class ExistsExpression : UnaryExpression, IExistsExpression
    {
        public ExistsExpression(ISubQueryExpression subquery) : base(subquery, Operator.Exists)
        {
            SubQuery = subquery;
        }

        public ISubQueryExpression SubQuery { get; set; }

        #region 逻辑运算符

        public static LogicExpression operator &(ExistsExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(ExistsExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
    }

    /// <summary>
    /// Not Exists表达式
    /// </summary>
    public class NotExistsExpression : UnaryExpression, INotExistsExpression
    {
        public NotExistsExpression(ISubQueryExpression subquery) : base(subquery, Operator.NotExists)
        {
            SubQuery = subquery;
        }

        public ISubQueryExpression SubQuery { get; set; }

        #region 逻辑运算符

        public static LogicExpression operator &(NotExistsExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(NotExistsExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
    }

    /// <summary>
    /// 三元表达式
    /// </summary>
    public class TernaryExpression : ExpressionBase<TernaryExpression>, ITernaryExpression
    {
        public TernaryExpression(ITernaryOperator op, ISimpleValue value1, ISimpleValue value2, ISimpleValue value3)
        {
            Op = op;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public ITernaryOperator Op { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISimpleValue Value1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISimpleValue Value2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISimpleValue Value3 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override string Build()
        {
            if (Op == null)
            {
                throw new SqlSyntaxException(this, Error.OperatorMissing);
            }
            if (Value1 == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            if (Value2 == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            if (Value3 == null)
            {
                throw new SqlSyntaxException(this, Error.OperandMissing);
            }
            return string.Format(Op.Format, Value1.Expression, Value2.Expression, Value3.Expression);
        }
    }

    /// <summary>
    /// Between表达式
    /// </summary>
    public class BetweenExpression : TernaryExpression, IBetweenExpression
    {
        public BetweenExpression(ISimpleValue value, ISimpleValue lower, ISimpleValue upper) : base(Operator.Between, value, lower, upper)
        {
            Value = value;
            Lower = lower;
            Upper = upper;
        }

        public ISimpleValue Value { get; set; }

        public ISimpleValue Lower { get; set; }

        public ISimpleValue Upper { get; set; }

        protected override string Build()
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
            return string.Format(Op.Format, Value.Expression, Lower.Expression, Upper.Expression);
        }

        #region 逻辑运算符

        public static LogicExpression operator &(BetweenExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(BetweenExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
    }

    /// <summary>
    /// Not Between表达式
    /// </summary>
    public class NotBetweenExpression : TernaryExpression, INotBetweenExpression
    {
        public NotBetweenExpression(ISimpleValue value, ISimpleValue lower, ISimpleValue upper) : base(Operator.NotBetween, value, lower, upper)
        {
            Value = value;
            Lower = lower;
            Upper = upper;
        }

        public ISimpleValue Value { get; set; }

        public ISimpleValue Lower { get; set; }

        public ISimpleValue Upper { get; set; }

        protected override string Build()
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
            return string.Format(Op.Format, Value.Expression, Lower.Expression, Upper.Expression);
        }

        #region 逻辑运算符

        public static LogicExpression operator &(NotBetweenExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(NotBetweenExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
    }

    /// <summary>
    /// In表达式
    /// </summary>
    public class InExpression : BinaryExpression, IInExpression
    {
        public InExpression(ISimpleValue value, ICollection collection) : base(value, Operator.In, collection)
        {
            Value = value;
            Collection = collection;
        }

        public ISimpleValue Value { get; set; }

        public ICollection Collection { get; set; }

        protected override string Build()
        {
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            if (Collection == null)
            {
                throw new SqlSyntaxException(this, Error.CollectionMissing);
            }
            return string.Format(Operator.In.Format, Value.Expression, Collection.Expression);
        }

        #region 逻辑运算符

        public static LogicExpression operator &(InExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(InExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
    }

    /// <summary>
    /// Not In表达式
    /// </summary>
    public class NotInExpression : BinaryExpression, INotInExpression
    {
        public NotInExpression(ISimpleValue value, ICollection collection) : base(value, Operator.NotIn, collection)
        {
            Value = value;
            Collection = collection;
        }

        public ISimpleValue Value { get; set; }

        public ICollection Collection { get; set; }

        protected override string Build()
        {
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            if (Collection == null)
            {
                throw new SqlSyntaxException(this, Error.CollectionMissing);
            }
            return string.Format(Operator.NotIn.Format, Value.Expression, Collection.Expression);
        }

        #region 逻辑运算符

        public static LogicExpression operator &(NotInExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(NotInExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
    }

    /// <summary>
    /// 逻辑表达式
    /// </summary>
    public class LogicExpression : BinaryExpression, ILogicExpression
    {
        public LogicExpression(ISimpleValue a, ILogicOperator op, ISimpleValue b)
            : base(a, op, b)
        {
            if (op == LogicOperator.And)
            {
                if (a is ILogicExpression && (a as ILogicExpression).Op == LogicOperator.Or)
                {
                    Value1 = new BracketExpression(a);
                }
                if (b is ILogicExpression && (b as ILogicExpression).Op == LogicOperator.Or)
                {
                    Value2 = new BracketExpression(b);
                }
            }
        }

        #region 逻辑运算符

        public static LogicExpression operator &(LogicExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(LogicExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion
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


        public static ComparisonExpression operator ==(LiteralValue val, ArithmeticExpression value)
        {
            return new ComparisonExpression(val, Operator.Eq, value);
        }
        public static ComparisonExpression operator !=(LiteralValue val, ArithmeticExpression value)
        {
            return new ComparisonExpression(val, Operator.Neq, value);
        }
        public static ComparisonExpression operator >(LiteralValue val, ArithmeticExpression value)
        {
            return new ComparisonExpression(val, Operator.Gt, value);
        }
        public static ComparisonExpression operator <(LiteralValue val, ArithmeticExpression value)
        {
            return new ComparisonExpression(val, Operator.Lt, value);
        }
        public static ComparisonExpression operator >=(LiteralValue val, ArithmeticExpression value)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, value);
        }
        public static ComparisonExpression operator <=(LiteralValue val, ArithmeticExpression value)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, value);
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


        public static ArithmeticExpression operator +(LiteralValue val, ArithmeticExpression value)
        {
            return new ArithmeticExpression(val, Operator.Add, value);
        }
        public static ArithmeticExpression operator -(LiteralValue val, ArithmeticExpression value)
        {
            return new ArithmeticExpression(val, Operator.Sub, value);
        }
        public static ArithmeticExpression operator *(LiteralValue val, ArithmeticExpression value)
        {
            return new ArithmeticExpression(val, Operator.Mul, value);
        }
        public static ArithmeticExpression operator /(LiteralValue val, ArithmeticExpression value)
        {
            return new ArithmeticExpression(val, Operator.Div, value);
        }
        public static ArithmeticExpression operator %(LiteralValue val, ArithmeticExpression value)
        {
            return new ArithmeticExpression(val, Operator.Mod, value);
        }

        #endregion

        #region 逻辑运算符

        public static LogicExpression operator &(ArithmeticExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(ArithmeticExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
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
        public FunctionExpression(string name, IList<ISimpleValue> values)
        {
            Name = name;
            Values = values;
        }


        public string Name { get; set; }

        public IList<ISimpleValue> Values { get; set; }

        protected override string Build()
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

        #region 逻辑运算符

        public static LogicExpression operator &(FunctionExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(FunctionExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 聚合函数表达式
    /// </summary>
    public class AggregateFunctionExpression : FunctionExpression, IAggregateFunctionExpression
    {
        public static AggregateFunctionExpression Count(ISimpleValue field)
        {
            return new AggregateFunctionExpression("COUNT", field);
        }

        public static AggregateFunctionExpression Sum(ISimpleValue field)
        {
            return new AggregateFunctionExpression("SUM", field);
        }

        public static AggregateFunctionExpression Avg(ISimpleValue field)
        {
            return new AggregateFunctionExpression("AVG", field);
        }

        public static AggregateFunctionExpression Max(ISimpleValue field)
        {
            return new AggregateFunctionExpression("MAX", field);
        }

        public static AggregateFunctionExpression Min(ISimpleValue field)
        {
            return new AggregateFunctionExpression("MIN", field);
        }

        public AggregateFunctionExpression(string name, ISimpleValue value)
            : base(name, new List<ISimpleValue>() { value })
        {

        }

        #region 比较运算符

        public static ComparisonExpression operator ==(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ComparisonExpression(fun, Operator.LtOrEq, val);
        }


        public static ComparisonExpression operator ==(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Eq, val);
        }
        public static ComparisonExpression operator !=(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Neq, val);
        }
        public static ComparisonExpression operator >(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Gt, val);
        }
        public static ComparisonExpression operator <(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.Lt, val);
        }
        public static ComparisonExpression operator >=(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.GtOrEq, val);
        }
        public static ComparisonExpression operator <=(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ComparisonExpression(fun, Operator.LtOrEq, val);
        }


        public static ComparisonExpression operator ==(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Eq, fun);
        }
        public static ComparisonExpression operator !=(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Neq, fun);
        }
        public static ComparisonExpression operator >(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Gt, fun);
        }
        public static ComparisonExpression operator <(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.Lt, fun);
        }
        public static ComparisonExpression operator >=(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.GtOrEq, fun);
        }
        public static ComparisonExpression operator <=(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ComparisonExpression(val, Operator.LtOrEq, fun);
        }

        #endregion

        #region 算术运算符

        public static ArithmeticExpression operator +(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(AggregateFunctionExpression fun, ISimpleValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mod, val);
        }


        public static ArithmeticExpression operator +(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Add, val);
        }
        public static ArithmeticExpression operator -(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Sub, val);
        }
        public static ArithmeticExpression operator *(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mul, val);
        }
        public static ArithmeticExpression operator /(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Div, val);
        }
        public static ArithmeticExpression operator %(AggregateFunctionExpression fun, LiteralValue val)
        {
            return new ArithmeticExpression(fun, Operator.Mod, val);
        }


        public static ArithmeticExpression operator +(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Add, fun);
        }
        public static ArithmeticExpression operator -(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Sub, fun);
        }
        public static ArithmeticExpression operator *(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Mul, fun);
        }
        public static ArithmeticExpression operator /(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Div, fun);
        }
        public static ArithmeticExpression operator %(LiteralValue val, AggregateFunctionExpression fun)
        {
            return new ArithmeticExpression(val, Operator.Mod, fun);
        }

        #endregion

        #region 逻辑运算符

        public static LogicExpression operator &(AggregateFunctionExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.And, exp2);
        }

        public static LogicExpression operator |(AggregateFunctionExpression exp1, ISimpleValue exp2)
        {
            return new LogicExpression(exp1, Operator.Or, exp2);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
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

        protected override string Build()
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
        public InsertStatement(ITable table, IList<IField> fields, ICollection values)
        {
            Table = table;
            Fields = fields;
            Values = values;
        }

        public ITable Table { get; set; }

        public IList<IField> Fields { get; set; }

        public ICollection Values { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                List<string> list = new List<string>();
                if (Values is ICustomExpression) list.AddRange((Values as ICustomExpression).Params);
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

        protected override string Build()
        {
            if (Table == null)
            {
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            if (Fields == null || Fields.Count == 0)
            {
                throw new SqlSyntaxException(this, Error.FieldsMissing);
            }
            if (Values == null)
            {
                throw new SqlSyntaxException(this, Error.ValuesMissing);
            }
            return string.Format("INSERT INTO {0}({1}) VALUES{2}", Table.Expression, Fields.Join(",", p => p.Expression), Values.Expression);
        }
    }

    /// <summary>
    /// 删除语句
    /// </summary>
    public class DeleteStatement : ExpressionBase<DeleteStatement>, IDeleteStatement
    {
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
                if (Where != null) list.AddRange(Where.Filter.ResolveParams());
                return list;
            }
        }

        protected override string Build()
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
        public UpdateStatement(IAliasTableExpression table, ISetClause set, IWhereClause where)
        {
            Table = table;
            Set = set;
            Where = where;
        }

        public IAliasTableExpression Table { get; set; }

        public ISetClause Set { get; set; }

        public IWhereClause Where { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                var list = new List<string>();
                foreach (var item in Set.Sets)
                {
                    if (item.Value is ICustomExpression)
                    {
                        list.AddRange((item.Value as ICustomExpression).Params);
                    }
                    else if (item.Value is IParam)
                    {
                        list.Add((item.Value as IParam).Name);
                    }
                }
                if (Where != null) list.AddRange(Where.Filter.ResolveParams());
                return list.Distinct();
            }
        }

        protected override string Build()
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
    /// 更新赋值子句
    /// </summary>
    public class SetClause : ExpressionBase<SetClause>, ISetClause
    {
        public SetClause(IList<ISetExpression> sets)
        {
            Sets = sets;
        }
        public IList<ISetExpression> Sets { get; set; }

        protected override string Build()
        {
            if (Sets == null || Sets.Count == 0)
            {
                throw new SqlSyntaxException(this, Error.SetClauseEmpty);
            }
            return string.Format("SET {0}", Sets.Join(",", set => set.Expression));
        }
    }

    /// <summary>
    /// 更新赋值项
    /// </summary>
    public class SetExpression : ExpressionBase<SetExpression>, ISetExpression
    {
        public SetExpression(IField field, ISimpleValue value)
        {
            Field = field;
            Value = value;
        }

        public SetExpression(IField field)
            : this(field, new Param(field.Name))
        {
        }

        public IField Field { get; set; }

        public ISimpleValue Value { get; set; }

        protected override string Build()
        {
            if (Field == null)
            {
                throw new SqlSyntaxException(this, Error.FieldMissing);
            }
            if (Value == null)
            {
                throw new SqlSyntaxException(this, Error.ValueMissing);
            }
            return string.Format("{0}={1}", Field.Expression, Value.Expression);
        }
    }

    /// <summary>
    /// Select查询语句
    /// </summary>
    public class SelectStatement : ExpressionBase<SelectStatement>, ISelectStatement
    {
        public SelectStatement(IQueryStatement query) : this(query, null) { }
        public SelectStatement(IQueryStatement query, IOrderByClause orderBy)
        {
            Query = query;
            OrderBy = orderBy;
        }

        public IQueryStatement Query { get; set; }

        public IOrderByClause OrderBy { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                return Query.Params;
            }
        }

        protected override string Build()
        {
            if (Query == null)
            {
                throw new SqlSyntaxException(this, Error.QueryMissing);
            }
            if (OrderBy == null)
            {
                return Query.Expression;
            }
            else
            {
                return string.Format("{0} {1}", Query.Expression, OrderBy?.Expression).TrimEnd();
            }
        }
    }

    /// <summary>
    /// 简单查询
    /// </summary>
    public class SimpleQueryStatement : ExpressionBase<SimpleQueryStatement>, ISimpleQueryStatement
    {
        public SimpleQueryStatement(ISelectClause select, IFromClause from) : this(select, from, null, null) { }
        public SimpleQueryStatement(ISelectClause select, IFromClause from, IWhereClause where) : this(select, from, where, null) { }
        public SimpleQueryStatement(ISelectClause select, IFromClause from, IGroupByClause groupBy) : this(select, from, null, groupBy) { }
        public SimpleQueryStatement(ISelectClause select, IFromClause from, IWhereClause where, IGroupByClause groupBy)
        {
            Select = select;
            From = from;
            Where = where;
            GroupBy = groupBy;
        }

        public ISelectClause Select { get; set; }
        public IFromClause From { get; set; }
        public IWhereClause Where { get; set; }
        public IGroupByClause GroupBy { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                var _params = new List<string>();
                foreach (var item in Select.Items)
                {
                    _params.AddRange(item.Field.ResolveParams());
                }
                _params.AddRange(From.Dataset.ResolveParams());
                if (Where != null) _params.AddRange(Where.Filter.ResolveParams());
                if (GroupBy != null) _params.AddRange(GroupBy.Having.ResolveParams());
                return _params.Distinct();
            }
        }

        protected override string Build()
        {
            if (Select == null)
            {
                throw new SqlSyntaxException(this, Error.SelectClauseMissing);
            }
            if (From == null)
            {
                throw new SqlSyntaxException(this, Error.FromClauseMissing);
            }
            return string.Format("{0} {1}{2}{3}", Select.Expression, From.Expression,
                Where == null ? string.Empty : " " + Where.Expression,
                GroupBy == null ? string.Empty : " " + GroupBy?.Expression);
        }
    }

    /// <summary>
    /// 合并查询
    /// </summary>
    public class UnionQueryStatement : ExpressionBase<UnionQueryStatement>, IUnionQueryStatement
    {
        public UnionQueryStatement(IQueryStatement query1, IUnionOperator unionOp, ISimpleQueryStatement query2)
        {
            Query1 = query1;
            UnionOp = unionOp;
            Query2 = query2;
        }

        public IQueryStatement Query1 { get; set; }

        public IUnionOperator UnionOp { get; set; }

        public ISimpleQueryStatement Query2 { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                List<string> _params = new List<string>();
                _params.AddRange(Query1.Params);
                _params.AddRange(Query2.Params);
                return _params;
            }
        }

        protected override string Build()
        {
            if (Query1 == null)
            {
                throw new SqlSyntaxException(this, Error.UnionQuery1Missing);
            }
            if (UnionOp == null)
            {
                throw new SqlSyntaxException(this, Error.UnionOperatorMissing);
            }
            if (Query2 == null)
            {
                throw new SqlSyntaxException(this, Error.UnionQuery2Missing);
            }
            return string.Format(UnionOp.Format, Query1.Expression, Query2.Expression);
        }
    }

    #region Select

    /// <summary>
    /// Select子句
    /// </summary>
    public class SelectClause : ExpressionBase<SelectClause>, ISelectClause
    {
        public SelectClause(IList<ISelectItemExpression> items) : this(items, false) { }

        public SelectClause(IList<ISelectItemExpression> items, bool distinct)
        {
            Items = items;
            Distinct = distinct;
        }

        public bool Distinct { get; set; }

        public IList<ISelectItemExpression> Items { get; set; }

        protected override string Build()
        {
            if (Items == null || Items.Count == 0)
            {
                throw new SqlSyntaxException(this, Error.SelectClauseEmpty);
            }
            if (Distinct)
            {
                return string.Format("SELECT DISTINCT {0}", Items.Join(",", s => s.Expression));
            }
            else
            {
                return string.Format("SELECT {0}", Items.Join(",", s => s.Expression));
            }
        }
    }

    /// <summary>
    /// 查询项表达式
    /// </summary>
    public class SelectItemExpression : ExpressionBase<SelectItemExpression>, ISelectItemExpression
    {
        public SelectItemExpression(ISimpleValue field, string alias)
        {
            if (field is ISelectItemExpression)
            {
                throw new ArgumentException(nameof(field));
            }
            Field = field;
            Alias = alias;
        }

        public ISimpleValue Field { get; set; }

        public string Alias { get; set; }

        protected override string Build()
        {
            if (Field == null)
            {
                throw new SqlSyntaxException(this, Error.FieldMissing);
            }
            if (string.IsNullOrWhiteSpace(Alias))
            {
                return Field.Expression;
            }
            else
            {
                return string.Format("{0} AS {2}{1}{3}", Field.Expression, Alias, SqlExpression.Expression.OpenQuotationMark, SqlExpression.Expression.CloseQuotationMark);
            }
        }
    }

    /// <summary>
    /// 所有项 *
    /// </summary>
    public class AllFieldssExpression : Field
    {
        public AllFieldssExpression(string dataset)
            : base("*", dataset)
        {
        }
    }

    #endregion

    #region From

    /// <summary>
    /// From子句
    /// </summary>
    public class FromClause : ExpressionBase<FromClause>, IFromClause
    {
        public FromClause(IDataset dataset)
        {
            Dataset = dataset;
        }

        public IDataset Dataset { get; set; }

        protected override string Build()
        {
            if (Dataset == null)
            {
                throw new SqlSyntaxException(this, Error.DatasetMissing);
            }
            return string.Format("FROM {0}", Dataset.Expression);
        }
    }

    /// <summary>
    /// 表带别名
    /// </summary>
    public class AliasTableExpression : ExpressionBase<AliasTableExpression>, IAliasTableExpression
    {
        public AliasTableExpression(ITable table, string alias = null)
        {
            Table = table;
            Alias = alias;
        }

        /// <summary>
        /// 表
        /// </summary>
        public ITable Table { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        protected override string Build()
        {
            if (Table == null)
            {
                throw new SqlSyntaxException(this, Error.TableMissing);
            }
            if (string.IsNullOrWhiteSpace(Alias))
            {
                return Table.Expression;
            }
            else
            {
                return string.Format("{0} AS {2}{1}{3}", Table.Expression, Alias, SqlExpression.Expression.OpenQuotationMark, SqlExpression.Expression.CloseQuotationMark);
            }
        }
    }

    /// <summary>
    /// 子查询带别名
    /// </summary>
    public class AliasSubQueryExpression : ExpressionBase<AliasSubQueryExpression>, IAliasSubQueryExpression
    {
        public AliasSubQueryExpression(ISelectStatement subquery) : this(new SubQueryExpression(subquery)) { }
        public AliasSubQueryExpression(ISelectStatement subquery, string alias) : this(new SubQueryExpression(subquery), alias) { }
        public AliasSubQueryExpression(ISubQueryExpression subquery) : this(subquery, null) { }
        public AliasSubQueryExpression(ISubQueryExpression subquery, string alias)
        {
            SubQuery = subquery;
            Alias = alias;
        }

        /// <summary>
        /// 子查询
        /// </summary>
        public ISubQueryExpression SubQuery { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        protected override string Build()
        {
            if (SubQuery == null)
            {
                throw new SqlSyntaxException(this, Error.SubQueryMissing);
            }
            if (Alias == null)
            {
                throw new SqlSyntaxException(this, Error.AliasMissing);
            }
            return string.Format("{0} AS {2}{1}{3}", SubQuery.Expression, Alias, SqlExpression.Expression.OpenQuotationMark, SqlExpression.Expression.CloseQuotationMark);
        }
    }

    /// <summary>
    /// 查询联接
    /// </summary>
    public class JoinExpression : ExpressionBase<JoinExpression>, IJoinExpression
    {
        public JoinExpression(IDataset left, IJoinOperator joinOp, IDataset right) : this(left, joinOp, right, null) { }

        public JoinExpression(IDataset left, IJoinOperator joinOp, IDataset right, ISimpleValue on)
        {
            Left = left;
            JoinOp = joinOp;
            Right = right;
            On = on;
        }

        public IDataset Left { get; set; }

        public IJoinOperator JoinOp { get; set; }

        public IDataset Right { get; set; }

        public ISimpleValue On { get; set; }

        protected override string Build()
        {
            if (Left == null)
            {
                throw new SqlSyntaxException(this, Error.LeftDatasetMissing);
            }
            if (JoinOp == null)
            {
                throw new SqlSyntaxException(this, Error.OperatorMissing);
            }
            if (Right == null)
            {
                throw new SqlSyntaxException(this, Error.RightDatasetMissing);
            }
            if (On == null)
            {
                return string.Format(JoinOp.Format.Replace(" ON ", string.Empty), Left.Expression, Right.Expression, string.Empty);
            }
            else
            {
                return string.Format(JoinOp.Format, Left.Expression, Right.Expression, On.Expression);
            }
        }
    }

    #endregion

    /// <summary>
    /// GroupBy分组子句
    /// </summary>
    public class GroupByClause : ExpressionBase<GroupByClause>, IGroupByClause
    {
        public GroupByClause(IList<ISimpleValue> fields) : this(fields, null) { }

        public GroupByClause(IList<ISimpleValue> fields, ISimpleValue having)
        {
            Fields = fields;
            Having = having;
        }

        public IList<ISimpleValue> Fields { get; set; }

        public ISimpleValue Having { get; set; }

        protected override string Build()
        {
            if (Fields == null || Fields.Count == 0)
            {
                throw new SqlSyntaxException(this, Error.GroupByFieldsEmpty);
            }
            if (Having == null)
            {
                return string.Format("GROUP BY {0}", Fields.Join(",", val => val.Expression));
            }
            else
            {
                return string.Format("GROUP BY {0} HAVING {1}", Fields.Join(",", val => val.Expression), Having.Expression);
            }
        }
    }

    /// <summary>
    /// OrderBy排序子句
    /// </summary>
    public class OrderByClause : ExpressionBase<OrderByClause>, IOrderByClause
    {
        public OrderByClause(IList<IOrderExpression> orders)
        {
            Orders = orders;
        }

        public IList<IOrderExpression> Orders { get; set; }

        protected override string Build()
        {
            if (Orders == null || Orders.Count == 0)
            {
                throw new SqlSyntaxException(this, Error.OrderByFieldsMissingEmpty);
            }
            return string.Format("ORDER BY {0}", Orders.Join(",", order => order.Expression));
        }
    }

    /// <summary>
    /// 排序项
    /// </summary>
    public class OrderExpression : ExpressionBase<OrderExpression>, IOrderExpression
    {
        public OrderExpression(ISimpleValue field) : this(field, OrderEnum.Asc) { }

        public OrderExpression(ISimpleValue field, OrderEnum order)
        {
            Field = field;
            Order = order;
        }

        public ISimpleValue Field { get; set; }

        public OrderEnum Order { get; set; } = OrderEnum.Asc;

        protected override string Build()
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
        public BatchSqlStatement(IList<ISqlStatement> sqls)
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
            Sqls = list.ToArray();
        }

        public IList<ISqlStatement> Sqls { get; set; }

        public IEnumerable<string> Params
        {
            get
            {
                IEnumerable<string> _params = new List<string>();
                foreach (var sql in Sqls)
                {
                    _params = _params.Concat(sql.Params);
                }
                return _params.Distinct();
            }
        }

        protected override string Build()
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
    public class CustomExpression : ExpressionBase<CustomExpression>, ICustomExpression
    {
        string _expression;
        public CustomExpression(string expression)
        {
            _expression = expression;
        }

        public IEnumerable<string> Params
        {
            get
            {
                return this.ResolveParams();
            }
        }

        protected override string Build()
        {
            return _expression;
        }

        public static implicit operator CustomExpression(string expression)
        {
            return new CustomExpression(expression);
        }
    }

    static class ResolveParamsExtension
    {
        /// <summary>
        /// 获取ISimpleValue参数列表
        /// </summary>
        /// <param name="simpleValue"></param>
        /// <returns></returns>
        public static IEnumerable<string> ResolveParams(this ISimpleValue simpleValue)
        {
            var _params = new List<string>();
            if (simpleValue is IParam)
            {
                return new List<string>() { (simpleValue as IParam).Name };
            }
            else if (simpleValue is ICustomExpression)
            {
                return (simpleValue as ICustomExpression).ResolveParams();
            }
            else if (simpleValue is IBinaryExpression)
            {
                var binary = simpleValue as IBinaryExpression;
                _params.AddRange(binary.Value1.ResolveParams());
                _params.AddRange(binary.Value2.ResolveParams());
            }
            else if (simpleValue is IUnaryExpression)
            {
                var unary = simpleValue as IUnaryExpression;
                _params.AddRange(unary.Value.ResolveParams());
            }
            else if (simpleValue is ITernaryExpression)
            {
                var ternary = simpleValue as ITernaryExpression;
                _params.AddRange(ternary.Value1.ResolveParams());
                _params.AddRange(ternary.Value2.ResolveParams());
                _params.AddRange(ternary.Value3.ResolveParams());
            }
            else if (simpleValue is IFunctionExpression)
            {
                var function = simpleValue as IFunctionExpression;
                foreach (var value in function.Values)
                {
                    _params.AddRange(value.ResolveParams());
                }
            }
            else if (simpleValue is ISubQueryExpression)
            {
                return (simpleValue as ISubQueryExpression).Query.Params;
            }
            else if (simpleValue is IValueCollectionExpression)
            {
                foreach (var val in (simpleValue as IValueCollectionExpression).Values)
                {
                    _params.AddRange(val.ResolveParams());
                }
            }
            else if (simpleValue is IAliasSubQueryExpression)
            {
                _params.AddRange((simpleValue as IAliasSubQueryExpression).SubQuery.ResolveParams());
            }
            return _params.Distinct();
        }

        public static IEnumerable<string> ResolveParams(this IDataset dataset)
        {
            var _params = new List<string>();

            if (dataset is IAliasSubQueryExpression)
            {
                _params.AddRange((dataset as ISimpleValue).ResolveParams());
            }
            else if (dataset is IJoinExpression)
            {
                var join = dataset as IJoinExpression;
                _params.AddRange(join.Left.ResolveParams());
                _params.AddRange(join.Right.ResolveParams());
                _params.AddRange(join.On.ResolveParams());
            }
            return _params.Distinct();
        }

        /// <summary>
        /// 获取自定义表达式参数列表
        /// </summary>
        /// <param name="custom"></param>
        /// <returns></returns>
        public static List<string> ResolveParams(this ICustomExpression custom)
        {
            var list = new List<string>();
            var matchs = Regex.Matches(custom.Expression, "(?<=@)[_a-zA-Z]+[_a-zA-Z0-9]*(?=[^a-zA-Z0-9]|$)");
            foreach (Match match in matchs)
            {
                list.Add(match.Value);
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