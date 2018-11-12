using System;
using System.Linq;
using System.Data;
using System.Text;

namespace SqlExpression
{
    public abstract class DbContext
    {
        public IDbConnection Connection { get; set; }

        public static string ToUpperCamalCase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            var segments = name.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                               .Select(segment => char.ToUpper(segment.First()) + segment.Substring(1));
            return string.Join("", segments);
        }

        public static string ToLowerCamelCase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            var segments = name.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                               .Select((segment, i) => (i > 0 ? char.ToUpper(segment.First()) : char.ToLower(segment.First())) + segment.Substring(1));

            return string.Join("", segments);
        }

        public static string ToSnakeCase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            var sb = new StringBuilder();
            bool first = true;
            bool lastIsLodash = false;
            foreach (var c in name)
            {
                if (c == '_')
                {
                    sb.Append(c);
                    lastIsLodash = true;
                }
                else if (char.IsUpper(c) && !first && !lastIsLodash)
                {
                    sb.Append('_');
                    sb.Append(char.ToLower(c));
                    lastIsLodash = false;
                }
                else
                {
                    sb.Append(c);
                    lastIsLodash = false;
                }
                first = false;
            }
            return sb.ToString().ToLower();
        }
    }
}
