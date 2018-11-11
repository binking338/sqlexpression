using System;
using System.Data;

namespace SqlExpression
{
    public abstract class DbContext
    {
        public IDbConnection Connection { get; set; }
    }
}
