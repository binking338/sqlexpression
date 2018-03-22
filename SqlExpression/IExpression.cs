using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        /// <summary>
        /// 通用
        /// </summary>
        Common = 0,
        /// <summary>
        /// mysql
        /// </summary>
        MySql = 1
    }

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
    /// 值
    /// </summary>
    public interface IValue : IExpression, IFilterExpression { }

    /// <summary>
    /// 布尔值
    /// </summary>
    public interface IBoolValue : IValue { }

    /// <summary>
    /// 数据集（一般二维）
    /// </summary>
    public interface IDataset : IExpression { }

    /// <summary>
    /// 列表集（一维）
    /// <see cref="ISubQueryExpression"/>
    /// <see cref="ValueCollectionExpression"/>
    /// </summary>
    public interface ICollection : IValue { }

    /// <summary>
    /// 表
    /// </summary>
    public interface ITable : IDataset
    {
        /// <summary>
        /// 表名
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// 表别名
    /// </summary>
    public interface ITableAlias : ITable
    {
    }

    /// <summary>
    /// 属性（字段）
    /// </summary>
    public interface IColumn : IValue
    {
        /// <summary>
        /// 属性（字段）名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 所属表
        /// </summary>
        ITable Table { get; set; }
    }

    /// <summary>
    /// 字面值
    /// </summary>
    public interface ILiteralValue : IValue
    {
        /// <summary>
        /// 值
        /// </summary>
        object Value { get; set; }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public interface IParam : IValue
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// 一元表达式
    /// </summary>
    public interface IUnaryExpression : IValue
    {
        /// <summary>
        /// 运算符
        /// </summary>
        IUnaryOperator Op { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        IValue A { get; set; }

        /// <summary>
        /// 是否括号括起来
        /// </summary>
        bool WithBracket { get; set; }
    }

    /// <summary>
    /// 二元表达式
    /// </summary>
    public interface IBinaryExpression : IValue
    {
        /// <summary>
        /// 操作符
        /// </summary>
        IBinaryOperator Op { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        IValue A { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        IValue B { get; set; }

        /// <summary>
        /// 是否括号括起来
        /// </summary>
        bool WithBracket { get; set; }
    }

    /// <summary>
    /// 逻辑表达式
    /// </summary>
    public interface ILogicExpression : IBinaryExpression, IBoolValue { }

    /// <summary>
    /// 一元比较表达式
    /// </summary>
    public interface IUnaryComparisonExpression : IUnaryExpression, IBoolValue { }

    /// <summary>
    /// 二元比较表达式
    /// </summary>
    public interface IComparisonExpression : IBinaryExpression, IBoolValue { }

    /// <summary>
    /// 子查询结果集数量大于0
    /// </summary>
    public interface IExistsExpression : IBoolValue
    {
        /// <summary>
        /// 子查询
        /// </summary>
        ISubQueryExpression SubQuery { get; set; }
    }

    /// <summary>
    /// 子查询结果集数量等于0
    /// </summary>
    public interface INotExistsExpression : IBoolValue
    {
        /// <summary>
        /// 子查询
        /// </summary>
        ISubQueryExpression SubQuery { get; set; }
    }

    /// <summary>
    /// Between表达式
    /// </summary>
    public interface IBetweenExpression : IBoolValue
    {
        IValue Value { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        IValue Lower { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        IValue Upper { get; set; }
    }

    /// <summary>
    /// Not Between表达式
    /// </summary>
    public interface INotBetweenExpression : IBoolValue
    {
        IValue Value { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        IValue Lower { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        IValue Upper { get; set; }
    }

    /// <summary>
    /// In表达式
    /// </summary>
    public interface IInExpression : IBoolValue
    {
        /// <summary>
        /// 值
        /// </summary>
        IValue Value { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        ICollection Collection { get; set; }
    }

    /// <summary>
    /// Not In表达式
    /// </summary>
    public interface INotInExpression : IBoolValue
    {
        /// <summary>
        /// 值
        /// </summary>
        IValue Value { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        ICollection Collection { get; set; }
    }

    /// <summary>
    /// 算术表达式
    /// </summary>
    public interface IArithmeticExpression : IBinaryExpression { }

    /// <summary>
    /// 函数表达式
    /// </summary>
    public interface IFunctionExpression : IValue
    {
        /// <summary>
        /// 函数名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        IValue[] Values { get; set; }
    }

    /// <summary>
    /// 值集合
    /// </summary>
    public interface IValueCollectionExpression : ICollection
    {
        IValue[] Values { get; set; }
    }

    /// <summary>
    /// 子查询
    /// </summary>
    public interface ISubQueryExpression : ICollection, IDataset
    {
        /// <summary>
        /// 子查询语句，返回单列
        /// </summary>
        ISelectStatement Query { get; set; }
    }

    #region Statement

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
    /// 表别名
    /// </summary>
    public interface ITableAliasExpression : IExpression
    {
        /// <summary>
        /// 表
        /// </summary>
        IDataset Table { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        ITableAlias As { get; set; }
    }

    #region WhereClause

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
    /// todo:直接去掉 使用IValue ？
    /// </summary>
    public interface IFilterExpression : IExpression { }

    #endregion

    /// <summary>
    /// insert语句
    /// </summary>
    public interface IInsertStatement : ISqlStatement
    {
        /// <summary>
        /// 表
        /// </summary>
        ITable Table { get; set; }

        /// <summary>
        /// 插入字段
        /// </summary>
        IColumn[] Columns { get; set; }

        /// <summary>
        /// 插入值
        /// todo:支持子查询
        /// </summary>
        ICollection[] Values { get; set; }
    }

    /// <summary>
    /// delete语句
    /// </summary>
    public interface IDeleteStatement : ISqlStatement
    {
        /// <summary>
        /// 表
        /// </summary>
        ITable Table { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        IWhereClause Where { get; set; }
    }

    #region UpdateStatement

    /// <summary>
    /// update语句
    /// </summary>
    public interface IUpdateStatement : ISqlStatement
    {
        /// <summary>
        /// 表
        /// </summary>
        ITableAliasExpression Table { get; set; }

        /// <summary>
        /// 字段赋值子句
        /// </summary>
        ISetClause Set { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        IWhereClause Where { get; set; }
    }

    /// <summary>
    /// 赋值子句（Update）
    /// </summary>
    public interface ISetClause : IExpression
    {
        /// <summary>
        /// 字段更新项
        /// </summary>
        ISetFieldExpression[] SetFields { get; set; }
    }

    /// <summary>
    /// 字段更新项
    /// </summary>
    public interface ISetFieldExpression : IExpression
    {
        /// <summary>
        /// 更新字段
        /// </summary>
        IColumn Column { get; set; }
        /// <summary>
        /// 更新值
        /// </summary>
        IValue Value { get; set; }
    }

    #endregion

    #region SelectStatement

    /// <summary>
    /// select语句
    /// </summary>
    public interface ISelectStatement : ISqlStatement
    {
        /// <summary>
        /// 表
        /// </summary>
        ITableAliasExpression[] Tables { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        ISelectFieldExpression[] Fields { get; set; }

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
    }

    #region UnionStatement

    /// <summary>
    /// 合并项
    /// </summary>
    public interface IUnionExpression : IExpression
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
        IUnionExpression[] Unions { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        IOrderByClause OrderBy { get; set; }
    }

    #endregion

    /// <summary>
    /// 查询项别名
    /// </summary>
    public interface ISelectFieldAlias : IValue
    {
        string Name { get; set; }
    }

    /// <summary>
    /// 查询项表达式
    /// </summary>
    public interface ISelectFieldExpression : IExpression
    {
        /// <summary>
        /// 查询项
        /// </summary>
        IValue Field { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        ISelectFieldAlias Alias { get; set; }
    }

    /// <summary>
    /// 去重
    /// </summary>
    public interface IDistinctExpression : IValue
    {
        /// <summary>
        /// 去重字段
        /// </summary>
        IValue[] SelectFields { get; set; }
    }

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
        /// todo:支持子查询
        /// </summary>
        ITableAliasExpression Table { get; set; }

        /// <summary>
        /// 连接条件
        /// </summary>
        IFilterExpression On { get; set; }
    }

    /// <summary>
    /// 聚合函数表达式
    /// </summary>
    public interface IAggregateFunctionExpression : IFunctionExpression { }

    /// <summary>
    /// 分组子句
    /// </summary>
    public interface IGroupByClause : IExpression
    {
        /// <summary>
        /// 分组字段
        /// </summary>
        IValue[] Fields { get; set; }
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
        IValue Field { get; set; }
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

    #endregion

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

    #endregion

    #region CustomerExpression

    /// <summary>
    /// 自定义值表达式
    /// </summary>
    public interface ICustomerValue : IValue, IBoolValue { }

    /// <summary>
    /// 自定义过滤条件表达式
    /// </summary>
    public interface ICustomerFilterExpression : IFilterExpression { }

    /// <summary>
    /// 自定义sql语句
    /// </summary>
    public interface ICustomerSqlStatement : ISqlStatement { }

    /// <summary>
    /// 自定义表达式
    /// </summary>
    public interface ICustomerExpression : IExpression, ICustomerValue, ICustomerFilterExpression, ICustomerSqlStatement { }

    #endregion
}