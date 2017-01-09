using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.UnitTest
{
    public class TestSchema : SchemaBase<TestSchema>
    {
        private static ColumnExpression[] __PKs;
        private static ColumnExpression[] __All;
        private static ColumnExpression _oid;
        private static ColumnExpression _oname;
        private static ColumnExpression _age;
        private static ColumnExpression _gender;
        private static ColumnExpression _isdel;

        static TestSchema()
        {
            Table = new TableExpression("test") { Type = DBType.MySql };
            _oid = new ColumnExpression("oid") { Type = DBType.MySql, Table = Table };
            _oname = new ColumnExpression("oname") { Type = DBType.MySql, Table = Table };
            _age = new ColumnExpression("age") { Type = DBType.MySql, Table = Table };
            _gender = new ColumnExpression("gender") { Type = DBType.MySql, Table = Table };
            _isdel = new ColumnExpression("isdel") { Type = DBType.MySql, Table = Table };
            __PKs = new ColumnExpression[] { _oid };
            __All = new ColumnExpression[] { _oid, _oname, _age, _gender, _isdel };
        }

        public ColumnExpression oid { get { return _oid; } }
        public ColumnExpression oname { get { return _oname; } }
        public ColumnExpression age { get { return _age; } }
        public ColumnExpression gender { get { return _gender; } }
        public ColumnExpression isdel { get { return _isdel; } }

        public ColumnExpression[] PKs()
        {
            return __PKs;
        }

        public ColumnExpression[] All()
        {
            return __All;
        }
    }

    public class FooSchema : SchemaBase<FooSchema>
    {
        private static ColumnExpression[] __PKs;
        private static ColumnExpression[] __All;
        private static ColumnExpression _oid;
        private static ColumnExpression _oname;
        private static ColumnExpression _isdel;

        static FooSchema()
        {
            Table = new TableExpression("foo") { Type = DBType.MySql };
            _oid = new ColumnExpression("oid") { Type = DBType.MySql, Table = Table };
            _oname = new ColumnExpression("oname") { Type = DBType.MySql, Table = Table };
            _isdel = new ColumnExpression("isdel") { Type = DBType.MySql, Table = Table };
            __PKs = new ColumnExpression[] { _oid };
            __All = new ColumnExpression[] { _oid, _oname, _isdel };
        }

        public ColumnExpression oid { get { return _oid; } }
        public ColumnExpression oname { get { return _oname; } }
        public ColumnExpression isdel { get { return _isdel; } }

        public ColumnExpression[] PKs()
        {
            return __PKs;
        }

        public ColumnExpression[] All()
        {
            return __All;
        }
    }

    public class BarSchema : SchemaBase<BarSchema>
    {
        private static ColumnExpression[] __PKs;
        private static ColumnExpression[] __All;
        private static ColumnExpression _oid;
        private static ColumnExpression _oname;
        private static ColumnExpression _isdel;

        static BarSchema()
        {
            Table = new TableExpression("bar") { Type = DBType.MySql };
            _oid = new ColumnExpression("oid") { Type = DBType.MySql, Table = Table };
            _oname = new ColumnExpression("oname") { Type = DBType.MySql, Table = Table };
            _isdel = new ColumnExpression("isdel") { Type = DBType.MySql, Table = Table };
            __PKs = new ColumnExpression[] { _oid };
            __All = new ColumnExpression[] { _oid, _oname, _isdel };
        }

        public ColumnExpression oid { get { return _oid; } }
        public ColumnExpression oname { get { return _oname; } }
        public ColumnExpression isdel { get { return _isdel; } }

        public ColumnExpression[] PKs()
        {
            return __PKs;
        }

        public ColumnExpression[] All()
        {
            return __All;
        }
    }
}
