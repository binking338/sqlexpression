using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression
{
    internal class Error
    {
        public const string TableNameMissing = "table name missing";
        public const string AliasNameMissing = "alias name missing";
        public const string FieldNameMissing = "field name missing";
        public const string ParamNameMissing = "param name missing";
        public const string CollectionMissing = "collection missing";
        public const string CollectionValuesMissing = "collection values missing";
        public const string OperatorMissing = "operator missing";
        public const string OperandMissing = "operand missing";
        public const string FunctionNameMissing = "function name missing";
        public const string FilterMissing = "filter missing";
        public const string TableMissing = "table missing";
        public const string FieldMissing = "field missing";
        public const string ValueMissing = "value missing";
        public const string FieldsMissing = "fields missing";
        public const string ValuesMissing = "values missing";
        public const string SetClauseMissing = "set clause missing";
        public const string SetClauseFieldsMissing = "set clause fields missing";
        public const string SelectFieldsMissing = "select fields missing";
        public const string SelectFieldNameMissing = "select field name missing";
        public const string GroupByFieldsMissing = "group by fields missing";
        public const string UnionSelectMissing = "union select missing";
        public const string UnionOperatorMissing = "union operator missing";
        public const string OrderByFieldsMissing = "order by fields missing";
        public const string SubQueryMissing = "subquery missing";
        public const string AliasMissing = "alias missing";
        public const string QueryMissing = "query missing";
        public const string BetweenLowerMissing = "between lower missing";
        public const string BetweenUpperMissing = "between upper missing";

        public const string SetValueError = "only IValueCollectionExpression can set ISimpleValue";
    }
}
