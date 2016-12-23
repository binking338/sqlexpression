using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    /// <summary>
    /// 排序枚举
    /// </summary>
    public enum OrderEnum
    {
        /// <summary>
        /// 升序
        /// </summary>
        Asc,
        /// <summary>
        /// 降序
        /// </summary>
        Desc
    }

    /// <summary>
    /// 表达式接口 IExpression
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// 表达式文本
        /// </summary>
        string Expression { get; }
    }

    /// <summary>
    /// 表
    /// </summary>
    public interface ITableExpression : IExpression
    {
        string Name { get; set; }
    }

    /// <summary>
    /// 值
    /// </summary>
    public interface IValueExpression : IExpression { }

    /// <summary>
    /// 属性（字段）
    /// </summary>
    public interface IPropertyExpression : ISelectableValueExpression
    {
        /// <summary>
        /// 属性（字段）名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 所属表
        /// </summary>
        ITableExpression Table { get; set; }
    }
    
    /// <summary>
    /// 字面值
    /// </summary>
    public interface ILiteralValueExpression : ISelectableValueExpression
    {
        /// <summary>
        /// 值
        /// </summary>
        object Value { get; set; }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public interface IParamExpression : IValueExpression
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// 自定义值
    /// </summary>
    public interface ICustomerValueExpression : ISelectableValueExpression { }

    /// <summary>
    /// 一元表达式
    /// </summary>
    public interface IUnaryExpression : ISelectableValueExpression
    {
        /// <summary>
        /// 操作数
        /// </summary>
        IExpression A { get; set; }

        /// <summary>
        /// 运算符
        /// </summary>
        IUnaryOperator Op { get; set; }
    }

    /// <summary>
    /// 二元表达式
    /// </summary>
    public interface IBinaryExpression : ISelectableValueExpression
    {
        /// <summary>
        /// 操作数
        /// </summary>
        IExpression A { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        IExpression B { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        IBinaryOperator Op { get; set; }
    }

    /// <summary>
    /// 算术表达式
    /// </summary>
    public interface IArithmeticExpression : IBinaryExpression, ISelectableValueExpression { }

    /// <summary>
    /// 函数表达式
    /// </summary>
    public interface IFunctionExpression : ISelectableValueExpression
    {
        /// <summary>
        /// 函数名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        IValueExpression[] Values { get; set; }
    }

    /// <summary>
    /// 聚合函数表达式
    /// </summary>
    public interface IAggregateFunctionExpression : IFunctionExpression { }

    /// <summary>
    /// sql语句
    /// </summary>
    public interface ISqlStatement : IExpression
    {
        /// <summary>
        /// 参数列表
        /// </summary>
        IEnumerable<string> Params { get; }
    }

    /// <summary>
    /// 批量sql语句
    /// </summary>
    public interface IBatchSqlStatement : ISqlStatement
    {
        /// <summary>
        /// sql数组
        /// </summary>
        ISqlStatement[] Sqls { get; set; }
    }

    /// <summary>
    /// 自定义sql语句
    /// </summary>
    public interface ICustomerSqlStatement : ISqlStatement { }

    /// <summary>
    /// insert语句
    /// </summary>
    public interface IInsertStatement : ISqlStatement
    {
        /// <summary>
        /// 表
        /// </summary>
        ITableExpression Table { get; set; }

        /// <summary>
        /// 插入字段
        /// </summary>
        IPropertyExpression[] Properties { get; set; }

        /// <summary>
        /// 插入值
        /// </summary>
        IValueExpression[] Values { get; set; }
    }

    /// <summary>
    /// delete语句
    /// </summary>
    public interface IDeleteStatement : ISqlStatement
    {
        /// <summary>
        /// 表
        /// </summary>
        ITableExpression Table { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        IWhereClause Where { get; set; }

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
    /// update语句
    /// </summary>
    public interface IUpdateStatement : ISqlStatement
    {
        /// <summary>
        /// 表
        /// </summary>
        ITableExpression Table { get; set; }

        /// <summary>
        /// 字段赋值子句
        /// </summary>
        ISetClause Set { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        IWhereClause Where { get; set; }

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
    /// 赋值子句（Update）
    /// </summary>
    public interface ISetClause : IExpression
    {
        /// <summary>
        /// 字段更新项
        /// </summary>
        ISetItemExpression[] Sets { get; set; }
    }

    /// <summary>
    /// 字段更新项
    /// </summary>
    public interface ISetItemExpression : IExpression
    {
        /// <summary>
        /// 更新字段
        /// </summary>
        IPropertyExpression Property { get; set; }
        /// <summary>
        /// 更新值
        /// </summary>
        IValueExpression Value { get; set; }
    }

    /// <summary>
    /// select语句
    /// </summary>
    public interface ISelectStatement : ISqlStatement, ISelectableValueExpression
    {
        /// <summary>
        /// 表
        /// </summary>
        ITableExpression[] Tables { get; set; }

        /// <summary>
        /// 项
        /// </summary>
        ISelectItemExpression[] Items { get; set; }

        /// <summary>
        /// 表连接
        /// </summary>
        IJoinExpression[] Joins { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        IWhereClause Where { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        IOrderByClause OrderBy { get; set; }

        /// <summary>
        /// 分组方式
        /// </summary>
        IGroupByClause GroupBy { get; set; }

        /// <summary>
        /// 分组条件
        /// </summary>
        IHavingClause Having { get; set; }

        /// <summary>
        /// 限制行数(MySql)
        /// </summary>
        ILimitClause Limit { get; set; }
    }

    /// <summary>
    /// 合并项
    /// </summary>
    public interface IUnionItemExpression : IExpression
    {
        /// <summary>
        /// union运算符
        /// </summary>
        IUnionOperator UnionOp { get; set; }

        /// <summary>
        /// select语句
        /// </summary>
        ISelectStatement Select { get; set; }
    }

    /// <summary>
    /// 合并查询语句
    /// </summary>
    public interface IUnionStatement : ISqlStatement
    {
        /// <summary>
        /// select语句
        /// </summary>
        ISelectStatement Select { get; set; }

        /// <summary>
        /// union项
        /// </summary>
        IUnionItemExpression[] UnionItems { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        IOrderByClause OrderBy { get; set; }
    }

    /// <summary>
    /// select的查询项
    /// </summary>
    public interface ISelectItemExpression : IExpression { }

    /// <summary>
    /// 可作为查询项的值
    /// </summary>
    public interface ISelectableValueExpression : IValueExpression, ISelectItemExpression { }

    /// <summary>
    /// 查询项别名
    /// </summary>
    public interface IAsExpression : ISelectItemExpression
    {
        /// <summary>
        /// 查询项
        /// </summary>
        ISelectItemExpression SelectItem { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        IPropertyExpression AsProperty { get; set; }
    }

    /// <summary>
    /// 去重
    /// </summary>
    public interface IDistinctExpression : ISelectItemExpression
    {
        /// <summary>
        /// 去重字段
        /// </summary>
        IPropertyExpression[] Properties { get; set; }
    }

    /// <summary>
    /// 自定义查询项
    /// </summary>
    public interface ICustomerSelectItemExpression : ISelectItemExpression { }

    /// <summary>
    /// 表连接
    /// </summary>
    public interface IJoinExpression : IExpression
    {
        /// <summary>
        /// 连接运算符
        /// </summary>
        IJoinOperator JoinOp { get; set; }
        
        /// <summary>
        /// 连接表
        /// </summary>
        ITableExpression Table { get; set; }

        /// <summary>
        /// 连接条件
        /// </summary>
        IFilterExpression On { get; set; }
    }

    /// <summary>
    /// 分组子句
    /// </summary>
    public interface IGroupByClause : IExpression
    {
        /// <summary>
        /// 分组字段
        /// </summary>
        IPropertyExpression Property { get; set; }
    }

    /// <summary>
    /// 分组条件
    /// </summary>
    public interface IHavingClause : IExpression
    {
        /// <summary>
        /// 分组条件
        /// </summary>
        IFilterExpression Filter { get; set; }
    }

    /// <summary>
    /// 字段排序方式
    /// </summary>
    public interface IOrderExpression : IExpression
    {
        /// <summary>
        /// 字段
        /// </summary>
        ISelectableValueExpression Property { get; set; }
        /// <summary>
        /// 升序|降序
        /// </summary>
        OrderEnum Order { get; set; }
    }

    /// <summary>
    /// 排序子句
    /// </summary>
    public interface IOrderByClause : IExpression
    {
        /// <summary>
        /// 字段排序方式
        /// </summary>
        IOrderExpression[] Orders { get; set; }
    }

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
    /// where子句
    /// </summary>
    public interface IWhereClause : IExpression
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        IFilterExpression Filter { get; set; }
    }

    /// <summary>
    /// 过滤条件表达式
    /// </summary>
    public interface IFilterExpression : ISelectableValueExpression { }

    /// <summary>
    /// 逻辑表达式
    /// </summary>
    public interface ILogicExpression : IBinaryExpression, IFilterExpression { }

    /// <summary>
    /// 二元比较表达式
    /// </summary>
    public interface IComparisonExpression : IBinaryExpression, IFilterExpression { }

    /// <summary>
    /// 一元比较表达式
    /// </summary>
    public interface IUnaryComparisonExpression : IUnaryExpression, IFilterExpression { }

    /// <summary>
    /// 自定义过滤条件表达式
    /// </summary>
    public interface ICustomerFilterExpression : IFilterExpression { }

    /// <summary>
    /// 自定义表达式
    /// </summary>
    public interface ICustomerExpression : IExpression, ICustomerSqlStatement, ICustomerValueExpression, ICustomerSelectItemExpression, ICustomerFilterExpression { }
}