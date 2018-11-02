using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    /// <summary>
    /// 运算符
    /// </summary>
    public interface IOperator
    {
        string Format { get; }
    }

    /// <summary>
    /// 一元运算符
    /// </summary>
    public interface IUnaryOperator : IOperator { }

    /// <summary>
    /// 二元运算符
    /// </summary>
    public interface IBinaryOperator : IOperator { }

    /// <summary>
    /// 一元比较运算符
    /// </summary>
    public interface IUnaryComparisonOperator : IUnaryOperator { }

    /// <summary>
    /// 比较运算符
    /// </summary>
    public interface IComparisonOperator : IBinaryOperator { }

    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public interface ILogicOperator : IBinaryOperator { }

    /// <summary>
    /// 算术运算符
    /// </summary>
    public interface IArithmeticOperator : IBinaryOperator { }

    public interface ITernaryOperator : IOperator { }

    /// <summary>
    /// 合并操作符
    /// </summary>
    public interface IUnionOperator : IOperator { }

    /// <summary>
    /// 联合查询运算符
    /// </summary>
    public interface IJoinOperator : IOperator { }

    /// <summary>
    /// 运算符
    /// </summary>
    public class Operator : IOperator
    {
        public static ComparisonOperator Eq = new ComparisonOperator("=");
        public static ComparisonOperator Neq = new ComparisonOperator("<>");
        public static ComparisonOperator Gt = new ComparisonOperator(">");
        public static ComparisonOperator GtOrEq = new ComparisonOperator(">=");
        public static ComparisonOperator Lt = new ComparisonOperator("<");
        public static ComparisonOperator LtOrEq = new ComparisonOperator("<=");
        public static ComparisonOperator In = new ComparisonOperator("IN", "{0} IN {1}");
        public static ComparisonOperator NotIn = new ComparisonOperator("NOT IN", "{0} NOT IN {1}");
        public static ComparisonOperator Like = new ComparisonOperator("LIKE", "{0} LIKE {1}");
        public static ComparisonOperator NotLike = new ComparisonOperator("NOT LIKE", "{0} NOT LIKE {1}");
        public static ComparisonOperator Is = new ComparisonOperator("IS", "{0} IS {1}");
        public static ComparisonOperator IsNot = new ComparisonOperator("IS NOT", " IS NOT {1}");
        public static UnaryComparisonOperator IsNull = new UnaryComparisonOperator("IS NULL", "{0} IS NULL");
        public static UnaryComparisonOperator IsNotNull = new UnaryComparisonOperator("IS NOT NULL", "{0} IS NOT NULL");
        public static UnaryComparisonOperator IsTrue = new UnaryComparisonOperator("IS TRUE", "{0} IS TRUE");
        public static UnaryComparisonOperator IsNotTrue = new UnaryComparisonOperator("IS NOT TRUE", "{0} IS NOT TRUE");
        public static UnaryComparisonOperator IsFalse = new UnaryComparisonOperator("IS FALSE", "{0} IS FALSE");
        public static UnaryComparisonOperator IsNotFalse = new UnaryComparisonOperator("IS NOT FALSE", "{0} IS NOT FALSE");

        public static UnaryOperator Bracket = new UnaryOperator("()", "({0})");
        public static UnaryOperator Exists = new UnaryOperator("EXISTS", "EXISTS {0}");
        public static UnaryOperator NotExists = new UnaryOperator("NOT EXISTS", "NOT EXISTS {0}");

        public static TernaryOperator Between = new TernaryOperator("BETWEEN", "{0} BETWEEN {1} AND {2}");
        public static TernaryOperator NotBetween = new TernaryOperator("NOT BETWEEN", "{0} NOT BETWEEN {1} AND {2}");

        public static LogicOperator Or = new LogicOperator("OR", "{0} OR {1}");
        public static LogicOperator And = new LogicOperator("AND", "{0} AND {1}");

        public static ArithmeticOperator Add = new ArithmeticOperator("+");
        public static ArithmeticOperator Sub = new ArithmeticOperator("-");
        public static ArithmeticOperator Mul = new ArithmeticOperator("*");
        public static ArithmeticOperator Div = new ArithmeticOperator("/");
        public static ArithmeticOperator Mod = new ArithmeticOperator("%");

        public static UnionOperator Union = new UnionOperator("UNION", "{0} UNION {1}");
        public static UnionOperator UnionAll = new UnionOperator("UNION ALL", "{0} UNION ALL {1}");

        public static JoinOperator InnerJoin = new JoinOperator("INNER JOIN", "{0} INNER JOIN {1} ON {2}");
        public static JoinOperator LeftJoin = new JoinOperator("LEFT JOIN", "{0} LEFT JOIN  {1} ON {2}");
        public static JoinOperator RightJoin = new JoinOperator("RIGHT JOIN", "{0} RIGHT JOIN  {1} ON {2}");
        public static JoinOperator FullJoin = new JoinOperator("FULL JOIN", "{0} FULL JOIN  {1} ON {2}");

        public Operator(string literal, string format)
        {
            _literal = literal;
            _format = format;
        }

        protected string _literal;
        protected string _format;

        public string Format
        {
            get
            {
                return _format;
            }
        }
        public override string ToString()
        {
            return _literal;
        }
    }

    public class UnaryOperator : Operator, IUnaryOperator
    {
        public UnaryOperator(string literal, string format) : base(literal, format) { }
    }

    public class BracketOperator : UnaryOperator
    {
        public BracketOperator(string literal, string format) : base(literal, format) { }
    }

    public class BinaryOperator : Operator, IBinaryOperator
    {
        public BinaryOperator(string literal) : base(literal, string.Format("{{0}}{0}{{1}}", literal)) { }
        public BinaryOperator(string literal, string format) : base(literal, format) { }
    }

    public class ComparisonOperator : BinaryOperator, IComparisonOperator
    {
        public ComparisonOperator(string literal) : base(literal) { }
        public ComparisonOperator(string literal, string format) : base(literal, format) { }
    }

    public class UnaryComparisonOperator : UnaryOperator, IUnaryComparisonOperator
    {
        public UnaryComparisonOperator(string literal, string format) : base(literal, format) { }
    }

    public class LogicOperator : BinaryOperator, ILogicOperator
    {
        public LogicOperator(string literal) : base(literal) { }
        public LogicOperator(string literal, string format) : base(literal, format) { }
    }

    public class ArithmeticOperator : BinaryOperator, IArithmeticOperator
    {
        public ArithmeticOperator(string literal) : base(literal) { }
    }

    public class TernaryOperator : Operator, ITernaryOperator
    {
        public TernaryOperator(string literal, string format) : base(literal, format) { }
    }

    public class UnionOperator : BinaryOperator, IUnionOperator
    {
        public UnionOperator(string literal, string format) : base(literal, format) { }
    }

    public class JoinOperator : TernaryOperator, IJoinOperator
    {
        public JoinOperator(string literal, string format) : base(literal, format) { }
    }
}
