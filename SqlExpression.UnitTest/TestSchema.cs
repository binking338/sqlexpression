using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExpression.UnitTest
{
    public class TestSchema : SchemaBase<TestSchema>
    {
        private static Field[] __PKs;
        private static Field[] __All;
        private static Field _oid;
        private static Field _oname;
        private static Field _age;
        private static Field _gender;
        private static Field _isdel;

        static TestSchema()
        {
            Table = new Table("test");
            _oid = new Field("oid") { DatasetAlias = new DatasetAlias(Table.Name) };
            _oname = new Field("oname") { DatasetAlias = new DatasetAlias(Table.Name)  };
            _age = new Field("age") { DatasetAlias = new DatasetAlias(Table.Name) };
            _gender = new Field("gender") {  DatasetAlias = new DatasetAlias(Table.Name) };
            _isdel = new Field("isdel") {  DatasetAlias = new DatasetAlias(Table.Name) };
            __PKs = new Field[] { _oid };
            __All = new Field[] { _oid, _oname, _age, _gender, _isdel };
        }

        public Field oid { get { return _oid; } }
        public Field oname { get { return _oname; } }
        public Field age { get { return _age; } }
        public Field gender { get { return _gender; } }
        public Field isdel { get { return _isdel; } }

        public Field[] PKs()
        {
            return __PKs;
        }

        public Field[] All()
        {
            return __All;
        }
    }

    public class FooSchema : SchemaBase<FooSchema>
    {
        private static Field[] __PKs;
        private static Field[] __All;
        private static Field _oid;
        private static Field _oname;
        private static Field _isdel;

        static FooSchema()
        {
            Table = new Table("foo");
            _oid = new Field("oid") {  DatasetAlias = new DatasetAlias(Table.Name) };
            _oname = new Field("oname") {  DatasetAlias = new DatasetAlias(Table.Name) };
            _isdel = new Field("isdel") {  DatasetAlias = new DatasetAlias(Table.Name) };
            __PKs = new Field[] { _oid };
            __All = new Field[] { _oid, _oname, _isdel };
        }

        public Field oid { get { return _oid; } }
        public Field oname { get { return _oname; } }
        public Field isdel { get { return _isdel; } }

        public Field[] PKs()
        {
            return __PKs;
        }

        public Field[] All()
        {
            return __All;
        }
    }

    public class BarSchema : SchemaBase<BarSchema>
    {
        private static Field[] __PKs;
        private static Field[] __All;
        private static Field _oid;
        private static Field _oname;
        private static Field _isdel;

        static BarSchema()
        {
            Table = new Table("bar");
            _oid = new Field("oid") {  DatasetAlias = new DatasetAlias(Table.Name) };
            _oname = new Field("oname") {  DatasetAlias = new DatasetAlias(Table.Name) };
            _isdel = new Field("isdel") {  DatasetAlias = new DatasetAlias(Table.Name) };
            __PKs = new Field[] { _oid };
            __All = new Field[] { _oid, _oname, _isdel };
        }

        public Field oid { get { return _oid; } }
        public Field oname { get { return _oname; } }
        public Field isdel { get { return _isdel; } }

        public Field[] PKs()
        {
            return __PKs;
        }

        public Field[] All()
        {
            return __All;
        }
    }
}
