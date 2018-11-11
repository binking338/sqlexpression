using System;
using System.Data;
using Microsoft.Extensions.Logging;

namespace SqlExpression
{
    public abstract class DbContext
    {
        public IDbConnection Connection { get; set; }
    }
}
