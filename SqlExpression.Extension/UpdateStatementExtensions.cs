using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression.Extension
{
    public static class UpdateStatementExtensions
    {
        #region Update

        #region ShortCut

        public static UpdateStatement UpdateP(this ITableFilterExpression tableFilter, IEnumerable<IColumn> columns)
        {
            return UpdateVarParam(tableFilter, columns);
        }

        public static UpdateStatement UpdateP(this ITableFilterExpression tableFilter, params IColumn[] columns)
        {
            return UpdateVarParam(tableFilter, columns);
        }

        public static UpdateStatement UpdateP(this ITable table, IEnumerable<IColumn> columns)
        {
            return UpdateVarParam(table, columns);
        }

        public static UpdateStatement UpdateP(this ITable table, params IColumn[] columns)
        {
            return UpdateVarParam(table, columns);
        }

        #endregion

        public static UpdateStatement Update(this ITableFilterExpression tableFilter)
        {
            return new UpdateStatement(new AliasTableExpression(tableFilter.Table, null), null, tableFilter.Where);
        }

        public static UpdateStatement Update(this ITableFilterExpression tableFilter, IEnumerable<IColumn> columns)
        {
            var set = new SetClause(columns.Select(c => new SetExpression(c, null) as ISetExpression).ToList());
            return new UpdateStatement(new AliasTableExpression(tableFilter.Table, null), set, null);
        }

        public static UpdateStatement Update(this ITableFilterExpression tableFilter, params IColumn[] columns)
        {
            return Update(tableFilter.Table, columns.AsEnumerable());
        }

        public static UpdateStatement UpdateVarParam(this ITableFilterExpression tableFilter, IEnumerable<IColumn> columns)
        {
            var set = new SetClause(columns.Select(column => column.SetVarParam() as ISetExpression).ToList());
            return new UpdateStatement(new AliasTableExpression(tableFilter.Table, null), set, null);
        }

        public static UpdateStatement UpdateVarParam(this ITableFilterExpression tableFilter, params IColumn[] columns)
        {
            return UpdateVarParam(tableFilter.Table, columns.AsEnumerable());
        }

        public static UpdateStatement Update(this ITable table)
        {
            return Update(table, new List<IColumn>());
        }

        public static UpdateStatement Update(this ITable table, IEnumerable<IColumn> columns)
        {
            var set = new SetClause(columns.Select(c => new SetExpression(c, null) as ISetExpression).ToList());
            return new UpdateStatement(new AliasTableExpression(table, null), set, null);
        }

        public static UpdateStatement Update(this ITable table, params IColumn[] columns)
        {
            return Update(table, columns.AsEnumerable());
        }

        public static UpdateStatement UpdateVarParam(this ITable table, IEnumerable<IColumn> columns)
        {
            var set = new SetClause(columns.Select(column => column.SetVarParam() as ISetExpression).ToList());
            return new UpdateStatement(new AliasTableExpression(table, null), set, null);
        }

        public static UpdateStatement UpdateVarParam(this ITable table, params IColumn[] columns)
        {
            return UpdateVarParam(table, columns.AsEnumerable());
        }

        #endregion

        #region Values

        #region Shortcut

        public static IUpdateStatement ValsL(this IUpdateStatement update, params object[] values)
        {
            return ValuesVarLiteral(update, values);
        }
        public static IUpdateStatement ValsP(this IUpdateStatement update)
        {
            return ValuesVarParam(update);
        }
        public static IUpdateStatement ValsC(this IUpdateStatement update, params string[] customValues)
        {
            return ValuesVarCustom(update, customValues);
        }

        public static IUpdateStatement ValuesP(this IUpdateStatement update)
        {
            return ValuesVarParam(update);
        }

        public static IUpdateStatement ValuesC(this IUpdateStatement update, params string[] customValues)
        {
            return ValuesVarCustom(update, customValues);
        }

        #endregion

        public static IUpdateStatement ValuesVarLiteral(this IUpdateStatement update, params object[] values)
        {
            update.Set = update.Set.ValuesVarLiteral(values);
            return update;
        }

        public static IUpdateStatement ValuesVarParam(this IUpdateStatement update, params string[] paramNames)
        {
            update.Set = update.Set.ValuesVarParam(paramNames);
            return update;
        }

        public static IUpdateStatement ValuesVarCustom(this IUpdateStatement update, params string[] customValues)
        {
            update.Set = update.Set.ValuesVarCustom(customValues);
            return update;
        }

        #endregion

        #region Set

        #region Shortcut

        public static IUpdateStatement SetL(this IUpdateStatement update, IColumn column, object value)
        {
            return SetVarLiteral(update, column, value);
        }

        public static IUpdateStatement SetP(this IUpdateStatement update, IColumn column, string param = null)
        {
            return SetVarParam(update, column, param);
        }

        public static IUpdateStatement SetC(this IUpdateStatement update, IColumn column, string customValue)
        {
            return SetVarCustom(update, column, customValue);
        }

        #endregion

        public static IUpdateStatement Set(this IUpdateStatement update, ISetExpression setItem)
        {
            if (update.Set == null)
            {
                update.Set = new SetClause(new List<ISetExpression>());
            }
            update.Set = update.Set.SetItem(setItem);
            return update;
        }

        public static IUpdateStatement Set(this IUpdateStatement update, IColumn column, ISimpleValue value)
        {
            if (update.Set == null)
            {
                update.Set = new SetClause(new List<ISetExpression>());
            }
            update.Set = update.Set.SetItem(column, value);
            return update;
        }

        public static IUpdateStatement SetVarLiteral(this IUpdateStatement update, IColumn column, object value)
        {
            if (update.Set == null)
            {
                update.Set = new SetClause(new List<ISetExpression>());
            }
            update.Set = update.Set.SetItem(column, value);
            return update;
        }

        public static IUpdateStatement SetVarParam(this IUpdateStatement update, IColumn column, string param = null)
        {
            if (update.Set == null)
            {
                update.Set = new SetClause(new List<ISetExpression>());
            }
            update.Set = update.Set.SetItemVarParam(column, param);
            return update;
        }

        public static IUpdateStatement SetVarCustom(this IUpdateStatement update, IColumn column, string customValue)
        {
            if (update.Set == null)
            {
                update.Set = new SetClause(new List<ISetExpression>());
            }
            update.Set = update.Set.SetItemVarCustom(column, customValue);
            return update;
        }

        #endregion

        #region Where

        #region ShortCut
        public static IUpdateStatement WhereC(this IUpdateStatement update, string customFilter)
        {
            return WhereVarCustom(update, customFilter);
        }
        public static IUpdateStatement OrWhereC(this IUpdateStatement update, string customFilter)
        {
            return OrWhereVarCustom(update, customFilter);
        }
        #endregion

        public static IUpdateStatement Where(this IUpdateStatement update, ISimpleValue filter)
        {
            if (update.Where == null)
            {
                update.Where = new WhereClause(filter);
            }
            else
            {
                update.Where.Filter = new LogicExpression(update.Where.Filter, Operator.And, filter);
            }
            return update;
        }
        public static IUpdateStatement WhereVarCustom(this IUpdateStatement update, string customFilter)
        {
            return Where(update, new CustomExpression(customFilter));
        }

        public static IUpdateStatement OrWhere(this IUpdateStatement update, ISimpleValue filter)
        {
            if (update.Where == null)
            {
                update.Where = new WhereClause(filter);
            }
            else
            {
                update.Where.Filter = new LogicExpression(update.Where.Filter, Operator.Or, filter);
            }
            return update;
        }
        public static IUpdateStatement OrWhereVarCustom(this IUpdateStatement update, string customFilter)
        {
            return OrWhere(update, new CustomExpression(customFilter));
        }

        #endregion

        #region ISetClause

        #region Shortcut
        public static ISetClause Vals(this ISetClause set, IEnumerable<ISimpleValue> values)
        {
            return Values(set, values);
        }
        public static ISetClause ValsL(this ISetClause set, params object[] values)
        {
            return ValuesVarLiteral(set, values);
        }
        public static ISetClause ValsP(this ISetClause set, params string[] paramNames)
        {
            return ValuesVarParam(set, paramNames);
        }
        public static ISetClause ValsC(this ISetClause set, params string[] customValues)
        {
            return ValuesVarCustom(set, customValues);
        }

        public static ISetClause ValuesP(this ISetClause set, params string[] paramNames)
        {
            return ValuesVarParam(set, paramNames);
        }

        public static ISetClause ValuesC(this ISetClause set, params string[] customValues)
        {
            return ValuesVarCustom(set, customValues);
        }

        public static ISetClause SetP(this ISetClause set, IColumn column, string param = null)
        {
            return SetItemVarParam(set, column, param);
        }

        public static ISetClause SetC(this ISetClause set, IColumn column, string customValue)
        {
            return SetItemVarCustom(set, column, customValue);
        }

        #endregion

        public static ISetClause Values(this ISetClause set, IEnumerable<ISimpleValue> values)
        {
            var list = values.ToList();
            for (int i = 0, j = 0; i < set.Sets.Count && j < list.Count;)
            {
                var setitem = set.Sets[i++];
                // 仅赋值未赋值的Column，不覆盖
                if (setitem.Value == null) setitem.Value = list[j++];
            }
            return set;
        }

        public static ISetClause ValuesVarLiteral(this ISetClause set, params object[] values)
        {
            return Values(set, values.Select(val => val is ISimpleValue ? val as ISimpleValue : new LiteralValue(val)));
        }

        public static ISetClause ValuesVarParam(this ISetClause set, params string[] paramNames)
        {
            return Values(set, set.Sets.Where(s => s.Value == null).Select((s, i) => s.Column.ToParam(i < paramNames.Length ? paramNames[i] : null)));
        }

        public static ISetClause ValuesVarCustom(this ISetClause set, params string[] customValues)
        {
            return Values(set, customValues.Select(val => new CustomExpression(val)));
        }

        public static ISetClause SetItem(this ISetClause set, ISetExpression setItem)
        {
            var list = (set.Sets ?? new List<ISetExpression>());
            var originSetItem = list.FirstOrDefault(i => i.Column.Name == setItem.Column.Name);
            if (originSetItem == null)
            {
                list.Add(setItem);
            }
            else
            {
                originSetItem.Value = setItem.Value;
            }
            set.Sets = list;
            return set;
        }

        public static ISetClause SetItem(this ISetClause set, IColumn column, object value)
        {
            return SetItem(set, column.Set(value));
        }

        public static ISetClause SetItemVarParam(this ISetClause set, IColumn column, string param = null)
        {
            return SetItem(set, column.SetVarParam(param));
        }

        public static ISetClause SetItemVarCustom(this ISetClause set, IColumn column, string customValue)
        {
            return SetItem(set, column.SetVarCustom(customValue));
        }

        #endregion

        #region ISetItemExpression

        #region Shortcut

        public static ISetExpression ValL(this ISetExpression setItem, object value)
        {
            return ValueVarLiteral(setItem, value);
        }

        public static ISetExpression ValP(this ISetExpression setItem, string param = null)
        {
            return ValueVarParam(setItem, param);
        }

        public static ISetExpression ValC(this ISetExpression setItem, string customValue)
        {
            return ValueVarCustom(setItem, customValue);
        }

        public static ISetExpression ValueL(this ISetExpression setItem, object value)
        {
            return ValueVarLiteral(setItem, value);
        }

        public static ISetExpression ValueP(this ISetExpression setItem, string param = null)
        {
            return ValueVarParam(setItem, param);
        }

        public static ISetExpression ValueC(this ISetExpression setItem, string customValue)
        {
            return ValueVarCustom(setItem, customValue);
        }

        #endregion

        public static ISetExpression ValueVarLiteral(this ISetExpression setItem, object value)
        {
            setItem.Value = new LiteralValue(value);
            return setItem;
        }

        public static ISetExpression ValueVarParam(this ISetExpression setItem, string param = null)
        {
            setItem.Value = setItem.Column.ToParam(param);
            return setItem;
        }

        public static ISetExpression ValueVarCustom(this ISetExpression setItem, string customValue)
        {
            setItem.Value = new CustomExpression(customValue);
            return setItem;
        }

        #endregion
    }
}