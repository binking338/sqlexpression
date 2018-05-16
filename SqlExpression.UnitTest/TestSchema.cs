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
            Table = new Table("test");
            _oid = new Column("oid") { Dataset = new DatasetAlias(Table.Name) };
            _oname = new Column("oname") { Dataset = new DatasetAlias(Table.Name)  };
            _age = new Column("age") { Dataset = new DatasetAlias(Table.Name) };
            _gender = new Column("gender") {  Dataset = new DatasetAlias(Table.Name) };
            _isdel = new Column("isdel") {  Dataset = new DatasetAlias(Table.Name) };
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
            Table = new Table("foo");
            _oid = new Column("oid") {  Dataset = new DatasetAlias(Table.Name) };
            _oname = new Column("oname") {  Dataset = new DatasetAlias(Table.Name) };
            _isdel = new Column("isdel") {  Dataset = new DatasetAlias(Table.Name) };
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
            Table = new Table("bar");
            _oid = new Column("oid") {  Dataset = new DatasetAlias(Table.Name) };
            _oname = new Column("oname") {  Dataset = new DatasetAlias(Table.Name) };
            _isdel = new Column("isdel") {  Dataset = new DatasetAlias(Table.Name) };
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
