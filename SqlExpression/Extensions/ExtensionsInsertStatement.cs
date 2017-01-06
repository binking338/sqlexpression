using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExpression
{
    public static class ExtensionsInsertStatement
    {
        #region Insert

        #region ShortCut

        public static InsertStatement InsertP(this ITableExpression table, IEnumerable<IPropertyExpression> properties)
        {
            return InsertVarParam(table, properties);
        }

        public static InsertStatement InsertP(this ITableExpression table, params IPropertyExpression[] properties)
        {
            return InsertVarParam(table, properties);
        }

        public static InsertStatement InsertP(this ITableExpression table, params PropertyExpression[] properties)
        {
            return InsertVarParam(table, properties);
        }

        public static IInsertStatement ValuesP(this IInsertStatement insert)
        {
            return ValuesVarParam(insert);
        }

        public static IInsertStatement SetP(this IInsertStatement insert, IPropertyExpression property, string param = null)
        {
            return SetVarParam(insert, property, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, IPropertyExpression property, string customer)
        {
            return SetVarCustomer(insert, property, customer);
        }

        public static IInsertStatement SetP(this IInsertStatement insert, PropertyExpression property, string param = null)
        {
            return SetVarParam(insert, property, param);
        }

        public static IInsertStatement SetC(this IInsertStatement insert, PropertyExpression property, string customer)
        {
            return SetVarCustomer(insert, property, customer);
        }

        #endregion

        public static InsertStatement Insert(this ITableExpression table, IEnumerable<IPropertyExpression> properties)
        {
            return new InsertStatement(table, properties.ToArray(), new IValueExpression[properties.Count()]);
        }

        public static InsertStatement Insert(this ITableExpression table, params IPropertyExpression[] properties)
        {
            return Insert(table, properties.AsEnumerable());
        }

        public static InsertStatement Insert(this ITableExpression table, params PropertyExpression[] properties)
        {
            return Insert(table, properties.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITableExpression table, IEnumerable<IPropertyExpression> properties)
        {
            var _params = properties.Select(prop => prop.ToParam());
            return new InsertStatement(table, properties.ToArray(), _params.ToArray());
        }

        public static InsertStatement InsertVarParam(this ITableExpression table, params IPropertyExpression[] properties)
        {
            return InsertVarParam(table, properties.AsEnumerable());
        }

        public static InsertStatement InsertVarParam(this ITableExpression table, params PropertyExpression[] properties)
        {
            return InsertVarParam(table, properties.AsEnumerable());
        }

        public static IInsertStatement Into(this IInsertStatement insert, ITableExpression table)
        {
            insert.Table = table;
            return insert;
        }

        public static IInsertStatement Into(this IInsertStatement insert, TableExpression table)
        {
            insert.Table = table;
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, IEnumerable<IPropertyExpression> columns)
        {
            insert.Properties = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, params IPropertyExpression[] columns)
        {
            insert.Properties = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Columns(this IInsertStatement insert, params PropertyExpression[] columns)
        {
            insert.Properties = columns.ToArray();
            return insert;
        }

        public static IInsertStatement Values(this IInsertStatement insert, IEnumerable<IValueExpression> values)
        {
            if (insert.Properties == null)
            {
                return insert;
            }
            else if (insert.Values == null)
            {
                insert.Values = values.Count() > insert.Properties.Length
                    ? values.Take(insert.Properties.Length).ToArray()
                    : values.Concat(new IValueExpression[insert.Properties.Length - values.Count()]).ToArray();
            }
            else
            {
                var vals = insert.Values.ToList();
                var props = insert.Properties.ToList();
                if (vals.Count > props.Count)
                {
                    vals.RemoveRange(props.Count, vals.Count - props.Count);
                }
                else if (vals.Count < props.Count)
                {
                    vals.AddRange(new IValueExpression[props.Count - vals.Count]);
                }
                for (int i = 0, j = 0; i < props.Count && j < values.Count();)
                {
                    var prop = props[i++];
                    // 避免覆盖
                    if (vals[i] == null) vals[i] = values.ElementAt(j++);
                }
                insert.Values = vals.ToArray();
            }
            return insert;
        }

        public static IInsertStatement Values(this IInsertStatement insert, params object[] values)
        {
            var setableValues = values.Select(val => val is IValueExpression ? val as IValueExpression : new LiteralValueExpression(val));
            return Values(insert, setableValues);
        }

        public static IInsertStatement ValuesVarParam(this IInsertStatement insert)
        {
            if (insert.Properties == null) return insert;
            var vals = insert.Properties.Select(prop => new ParamExpression(prop.Name) as IValueExpression).ToArray();
            if (insert.Values != null)
            {
                for (var i = 0; i < vals.Length && i < insert.Values.Length; i++)
                {
                    if (insert.Values[i] != null) vals[i] = insert.Values[i];
                }
            }
            insert.Values = vals.ToArray();
            return insert;
        }

        public static IInsertStatement ValuesFillNull(this IInsertStatement insert)
        {
            if (insert.Properties == null) return insert;
            var vals = insert.Properties.Select(prop => new LiteralValueExpression(null) as IValueExpression).ToArray();
            if (insert.Values != null)
            {
                for (var i = 0; i < vals.Length && i < insert.Values.Length; i++)
                {
                    if (insert.Values[i] != null) vals[i] = insert.Values[i];
                }
            }
            insert.Values = vals.ToArray();
            return insert;
        }

        public static IInsertStatement Set(this IInsertStatement insert, IPropertyExpression property, IValueExpression value)
        {
            var props = insert.Properties.ToList();
            var vals = insert.Values.ToList();
            props.Add(property);
            vals.Add(value);
            insert.Properties = props.ToArray();
            insert.Values = vals.ToArray();
            return insert;
        }
        public static IInsertStatement Set(this IInsertStatement insert, IPropertyExpression property, object value)
        {
            return Set(insert, property, value is IValueExpression ? value as IValueExpression : new LiteralValueExpression(value));
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, IPropertyExpression property, string param = null)
        {
            return Set(insert, property, property.ToParam(param));
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, IPropertyExpression property, string customer)
        {
            return Set(insert, property, new CustomerExpression(customer));
        }

        public static IInsertStatement Set(this IInsertStatement insert, PropertyExpression property, IValueExpression value)
        {
            return Set(insert, property as IPropertyExpression, value);
        }
        public static IInsertStatement Set(this IInsertStatement insert, PropertyExpression property, object value)
        {
            return Set(insert, property as IPropertyExpression, value);
        }
        public static IInsertStatement SetVarParam(this IInsertStatement insert, PropertyExpression property, string param = null)
        {
            return SetVarParam(insert, property as IPropertyExpression, param);
        }
        public static IInsertStatement SetVarCustomer(this IInsertStatement insert, PropertyExpression property, string customer)
        {
            return SetVarCustomer(insert, property as IPropertyExpression, customer);
        }

        #endregion
    }
}
