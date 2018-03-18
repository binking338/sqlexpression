using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.UnitTest
{
    public class TestSchema : SchemaBase<TestSchema>
    {
        private static Column[] __PKs;
        private static Column[] __All;
        private static Column _oid;
        private static Column _oname;
        private static Column _age;
        private static Column _gender;
        private static Column _isdel;

        static TestSchema()
        {
            Table = new Table("test") { Type = DBType.MySql };
            _oid = new Column("oid") { Type = DBType.MySql, Table = Table };
            _oname = new Column("oname") { Type = DBType.MySql, Table = Table };
            _age = new Column("age") { Type = DBType.MySql, Table = Table };
            _gender = new Column("gender") { Type = DBType.MySql, Table = Table };
            _isdel = new Column("isdel") { Type = DBType.MySql, Table = Table };
            __PKs = new Column[] { _oid };
            __All = new Column[] { _oid, _oname, _age, _gender, _isdel };
        }

        public Column oid { get { return _oid; } }
        public Column oname { get { return _oname; } }
        public Column age { get { return _age; } }
        public Column gender { get { return _gender; } }
        public Column isdel { get { return _isdel; } }

        public Column[] PKs()
        {
            return __PKs;
        }

        public Column[] All()
        {
            return __All;
        }
    }

    public class FooSchema : SchemaBase<FooSchema>
    {
        private static Column[] __PKs;
        private static Column[] __All;
        private static Column _oid;
        private static Column _oname;
        private static Column _isdel;

        static FooSchema()
        {
            Table = new Table("foo") { Type = DBType.MySql };
            _oid = new Column("oid") { Type = DBType.MySql, Table = Table };
            _oname = new Column("oname") { Type = DBType.MySql, Table = Table };
            _isdel = new Column("isdel") { Type = DBType.MySql, Table = Table };
            __PKs = new Column[] { _oid };
            __All = new Column[] { _oid, _oname, _isdel };
        }

        public Column oid { get { return _oid; } }
        public Column oname { get { return _oname; } }
        public Column isdel { get { return _isdel; } }

        public Column[] PKs()
        {
            return __PKs;
        }

        public Column[] All()
        {
            return __All;
        }
    }

    public class BarSchema : SchemaBase<BarSchema>
    {
        private static Column[] __PKs;
        private static Column[] __All;
        private static Column _oid;
        private static Column _oname;
        private static Column _isdel;

        static BarSchema()
        {
            Table = new Table("bar") { Type = DBType.MySql };
            _oid = new Column("oid") { Type = DBType.MySql, Table = Table };
            _oname = new Column("oname") { Type = DBType.MySql, Table = Table };
            _isdel = new Column("isdel") { Type = DBType.MySql, Table = Table };
            __PKs = new Column[] { _oid };
            __All = new Column[] { _oid, _oname, _isdel };
        }

        public Column oid { get { return _oid; } }
        public Column oname { get { return _oname; } }
        public Column isdel { get { return _isdel; } }

        public Column[] PKs()
        {
            return __PKs;
        }

        public Column[] All()
        {
            return __All;
        }
    }
}
