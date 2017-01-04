using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.UnitTest
{
    public class TestSchema : SchemaBase<TestSchema>
    {
        private static PropertyExpression[] __PKs;
        private static PropertyExpression[] __All;
        private static PropertyExpression _oid;
        private static PropertyExpression _oname;
        private static PropertyExpression _age;
        private static PropertyExpression _isdel;

        static TestSchema()
        {
            Table = new TableExpression("test");
            _oid = new PropertyExpression("oid") { Type = DBType.MySql, Table = Table };
            _oname = new PropertyExpression("oname") { Type = DBType.MySql, Table = Table };
            _age = new PropertyExpression("age") { Type = DBType.MySql, Table = Table };
            _isdel = new PropertyExpression("isdel") { Type = DBType.MySql, Table = Table };
            __PKs = new PropertyExpression[] { _oid };
            __All = new PropertyExpression[] { _oid, _oname, _age, _isdel };
        }

        public PropertyExpression oid { get { return _oid; } }
        public PropertyExpression oname { get { return _oname; } }
        public PropertyExpression age { get { return _age; } }
        public PropertyExpression isdel { get { return _isdel; } }

        public PropertyExpression[] PKs()
        {
            return __PKs;
        }

        public PropertyExpression[] All()
        {
            return __All;
        }
    }
}
