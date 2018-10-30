﻿using System;
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
        Asc = 0,
        /// <summary>
        /// 降序
        /// </summary>
        Desc = 1
    }

    /// <summary>
    /// 表达式接口
    /// 所有的SQL语句组成都继承该接口
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
    public interface IValue : IExpression { }

    /// <summary>
    /// 简单值
    /// 相对于ICollection，IDataset
    /// </summary>
    public interface ISimpleValue : IValue { }

    /// <summary>
    /// 布尔值
    /// </summary>
    public interface IBoolValue : ISimpleValue { }

    /// <summary>
    /// 值列表
    /// </summary>
    public interface ICollection : IValue { }

    /// <summary>
    /// 表
    /// </summary>
    public interface ITable : IExpression
    {
        /// <summary>
        /// 表名
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// 列（字段）
    /// </summary>
    public interface IField : ISimpleValue
    {
        /// <summary>
        /// 列（字段）名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 所属数据集
        /// </summary>
        string DatasetAlias { get; set; }
    }

    /// <summary>
    /// 字面值
    /// </summary>
    public interface ILiteralValue : ISimpleValue
    {
        /// <summary>
        /// 值
        /// </summary>
        object Value { get; set; }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public interface IParam : ISimpleValue
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// 一元表达式
    /// </summary>
    public interface IUnaryExpression : ISimpleValue
    {
        /// <summary>
        /// 运算符
        /// </summary>
        IUnaryOperator Op { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        ISimpleValue Value { get; set; }
    }

    /// <summary>
    /// 二元表达式
    /// </summary>
    public interface IBinaryExpression : ISimpleValue
    {
        /// <summary>
        /// 操作符
        /// </summary>
        IBinaryOperator Op { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        ISimpleValue Value1 { get; set; }

        /// <summary>
        /// 操作数
        /// </summary>
        ISimpleValue Value2 { get; set; }
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
    /// Exists表达式：子查询结果集数量大于0
    /// </summary>
    public interface IExistsExpression : IBoolValue
    {
        /// <summary>
        /// 子查询
        /// </summary>
        ISubQueryExpression SubQuery { get; set; }
    }

    /// <summary>
    /// Not Exists表达式：子查询结果集数量等于0
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
        ISimpleValue Value { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        ISimpleValue Lower { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        ISimpleValue Upper { get; set; }
    }

    /// <summary>
    /// Not Between表达式
    /// </summary>
    public interface INotBetweenExpression : IBoolValue
    {
        ISimpleValue Value { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        ISimpleValue Lower { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        ISimpleValue Upper { get; set; }
    }

    /// <summary>
    /// In表达式
    /// </summary>
    public interface IInExpression : IBoolValue
    {
        /// <summary>
        /// 值
        /// </summary>
        ISimpleValue Value { get; set; }

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
        ISimpleValue Value { get; set; }

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
    public interface IFunctionExpression : ISimpleValue
    {
        /// <summary>
        /// 函数名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        IList<ISimpleValue> Values { get; set; }
    }

    /// <summary>
    /// 值集合
    /// </summary>
    public interface IValueCollectionExpression : ICollection
    {
        IList<ISimpleValue> Values { get; set; }
    }

    /// <summary>
    /// 子查询
    /// </summary>
    public interface ISubQueryExpression : ICollection, ISimpleValue
    {
        /// <summary>
        /// 子查询语句
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
    /// 数据集别名
    /// </summary>
    public interface IDatasetWithAlias : IExpression
    {
        /// <summary>
        /// 别名
        /// </summary>
        string Alias { get; set; }
    }

    /// <summary>
    /// 表别名
    /// </summary>
    public interface ITableAliasExpression : IDatasetWithAlias
    {
        /// <summary>
        /// 表
        /// </summary>
        ITable Table { get; set; }
    }

    /// <summary>
    /// 子查询别名
    /// </summary>
    public interface ISubQueryAliasExpression : IDatasetWithAlias, ISimpleValue
    {
        /// <summary>
        /// 数据集
        /// </summary>
        ISubQueryExpression SubQuery { get; set; }
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
        ISimpleValue Filter { get; set; }
    }

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
        IList<IField> Fields { get; set; }

        /// <summary>
        /// 插入值
        /// </summary>
        ICollection Values { get; set; }
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
        IList<ISetFieldExpression> SetFields { get; set; }
    }

    /// <summary>
    /// 字段更新项
    /// </summary>
    public interface ISetFieldExpression : IExpression
    {
        /// <summary>
        /// 更新字段
        /// </summary>
        IField Field { get; set; }
        /// <summary>
        /// 更新值
        /// </summary>
        ISimpleValue Value { get; set; }
    }

    #endregion

    #region SelectStatement

    /// <summary>
    /// select语句
    /// </summary>
    public interface ISelectStatement : ISingleSelectStatement
    {
        /// <summary>
        /// Union项
        /// </summary>
        IList<IUnionExpression> Unions { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        IOrderByClause OrderBy { get; set; }
    }

    /// <summary>
    /// 简单select语句
    /// </summary>
    public interface ISingleSelectStatement : ISqlStatement
    {
        /// <summary>
        /// 是否去重
        /// </summary>
        bool Distinct { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        IList<ISelectItemExpression> Fields { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        IFromClause From { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IWhereClause Where { get; set; }

        /// <summary>
        /// 分组方式
        /// </summary>
        IGroupByClause GroupBy { get; set; }
    }
    
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
        ISingleSelectStatement Select { get; set; }
    }


    /// <summary>
    /// 查询项表达式
    /// </summary>
    public interface ISelectItemExpression : IExpression
    {
        /// <summary>
        /// 查询项
        /// </summary>
        ISimpleValue Field { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        string Alias { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IFromClause : IExpression
    {
        /// <summary>
        /// 表
        /// </summary>
        IDatasetWithAlias Table { get; set; }

        /// <summary>
        /// 表连接
        /// </summary>
        IList<IJoinExpression> Joins { get; set; }
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
        /// </summary>
        IDatasetWithAlias Table { get; set; }

        /// <summary>
        /// 连接条件
        /// </summary>
        ISimpleValue On { get; set; }
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
        IList<ISimpleValue> Fields { get; set; }

        /// <summary>
        /// 分组条件
        /// </summary>
        ISimpleValue Having { get; set; }
    }

    /// <summary>
    /// 排序子句
    /// </summary>
    public interface IOrderByClause : IExpression
    {
        /// <summary>
        /// 字段排序方式
        /// </summary>
        IList<IOrderExpression> Orders { get; set; }
    }

    /// <summary>
    /// 字段排序方式
    /// </summary>
    public interface IOrderExpression : IExpression
    {
        /// <summary>
        /// 字段
        /// </summary>
        ISimpleValue Field { get; set; }
        /// <summary>
        /// 升序|降序
        /// </summary>
        OrderEnum Order { get; set; }
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
        IList<ISqlStatement> Sqls { get; set; }
    }

    #endregion

    #region CustomerExpression

    /// <summary>
    /// 自定义值表达式
    /// </summary>
    public interface ICustomerValue : IValue { }

    /// <summary>
    /// 自定义简单值表达式
    /// </summary>
    public interface ICustomerSimpleValue : ISimpleValue { }

    /// <summary>
    /// 自定义sql语句
    /// </summary>
    public interface ICustomerSqlStatement : ISqlStatement { }

    /// <summary>
    /// 自定义表达式
    /// </summary>
    public interface ICustomerExpression : IExpression, ICustomerValue, ICustomerSimpleValue, ICustomerSqlStatement { }

    #endregion
}