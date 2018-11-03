using System;
using System.Collections.Generic;
using System.Text;
using SqlExpression;

namespace SqlExpression.UnitTest.Scheme
{
    public class Foo
    {
        public Foo(string alias)
        {
            Oid = new Field("oid", alias);
            Oname = new Field("oname", alias);
            Age = new Field("age", alias);
            Gender = new Field("gender", alias);
            Isdel = new Field("isdel", alias);
        }

        public Field Oid { get; set; }

        public Field Oname { get; set; }

        public Field Age { get; set; }

        public Field Gender { get; set; }

        public Field Isdel { get; set; }
    }

    public class Schema<T> where T : class
    {
        public Table Table { get; set; }

        public T Fields { get; set; }
    }
}
