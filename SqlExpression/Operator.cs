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
    public interface IOperator { }

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
        public static ComparisonOperator In = new ComparisonOperator(" IN ");
        public static ComparisonOperator NotIn = new ComparisonOperator(" NOT IN ");
        public static ComparisonOperator Like = new ComparisonOperator(" LIKE ");
        public static ComparisonOperator NotLike = new ComparisonOperator(" NOT LIKE ");
        public static ComparisonOperator Is = new ComparisonOperator(" IS ");
        public static ComparisonOperator IsNot = new ComparisonOperator(" IS NOT ");
        public static UnaryComparisonOperator IsNull = new UnaryComparisonOperator(" IS NULL");
        public static UnaryComparisonOperator IsNotNull = new UnaryComparisonOperator(" IS NOT NULL");
        public static UnaryComparisonOperator IsTrue = new UnaryComparisonOperator(" IS TRUE");
        public static UnaryComparisonOperator IsNotTrue = new UnaryComparisonOperator(" IS NOT TRUE");
        public static UnaryComparisonOperator IsFalse = new UnaryComparisonOperator(" IS FALSE");
        public static UnaryComparisonOperator IsNotFalse = new UnaryComparisonOperator(" IS NOT FALSE");

        public static LogicOperator Or = new LogicOperator(" OR ");
        public static LogicOperator And = new LogicOperator(" AND ");

        public static ArithmeticOperator Add = new ArithmeticOperator("+");
        public static ArithmeticOperator Sub = new ArithmeticOperator("-");
        public static ArithmeticOperator Mul = new ArithmeticOperator("*");
        public static ArithmeticOperator Div = new ArithmeticOperator("/");
        public static ArithmeticOperator Mod = new ArithmeticOperator("%");

        public static UnionOperator Union = new UnionOperator("UNION");
        public static UnionOperator UnionAll = new UnionOperator("UNION ALL");
        
        public static JoinOperator InnerJoin = new JoinOperator("INNER JOIN");
        public static JoinOperator LeftJoin = new JoinOperator("LEFT JOIN");
        public static JoinOperator RightJoin = new JoinOperator("RIGHT JOIN");
        public static JoinOperator FullJoin = new JoinOperator("FULL JOIN");

        public Operator(string literal)
        {
            _literal = literal;
        }

        protected string _literal;
        public override string ToString()
        {
            return _literal;
        }
    }

    public class UnaryOperator : Operator, IUnaryOperator
    {
        public UnaryOperator(string literal) : base(literal) { }
    }

    public class BinaryOperator : Operator, IBinaryOperator
    {
        public BinaryOperator(string literal) : base(literal) { }
    }

    public class ComparisonOperator : BinaryOperator, IComparisonOperator
    {
        public ComparisonOperator(string literal) : base(literal) { }
    }

    public class UnaryComparisonOperator : UnaryOperator, IUnaryComparisonOperator
    {
        public UnaryComparisonOperator(string literal) : base(literal) { }
    }

    public class LogicOperator : Operator, ILogicOperator
    {
        public LogicOperator(string literal) : base(literal) { }
    }

    public class ArithmeticOperator : Operator, IArithmeticOperator
    {
        public ArithmeticOperator(string literal) : base(literal) { }
    }

    public class UnionOperator : Operator, IUnionOperator
    {
        public UnionOperator(string literal) : base(literal) { }
    }

    public class JoinOperator : Operator, IJoinOperator
    {
        public JoinOperator(string literal) : base(literal) { }
    }
}
