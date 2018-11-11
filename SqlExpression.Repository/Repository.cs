using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SqlExpression;

namespace SqlExpression
{
    public class Respository<TSchema, TEntity>
        where TSchema : SchemaExpression<TSchema>, IAliasTableExpression, new()
        where TEntity : class, new()
    {
        protected TSchema schema = null;

        public Respository()
        {
            schema = new TSchema();
        }

        public virtual TSchema Schema
        {
            get { return schema; }
            set { schema = value; }
        }

        public virtual object Insert(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null)
        {
            throw new NotImplementedException();
        }

        public virtual bool Update(TEntity entity, Func<TSchema, IEnumerable<IColumn>> columns = null)
        {
            throw new NotImplementedException();
        }

        public virtual int Update(object columns, Func<TSchema, ISimpleValue> filter, object param = null)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(IEnumerable<object> primaryKeys)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Get(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> GetList(IEnumerable<object> primaryKeys)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> Query(Func<TSchema, ISimpleValue> filter, object param = null)
        {
            throw new NotImplementedException();
        }
    }
}
