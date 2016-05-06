using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

using SubSonic.Sugar;

namespace CodeGenerator.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEqualTo(this string firstString, string secondString)
        {
            if (firstString.IsEmpty())
            {
                firstString = string.Empty;
            }

            return firstString.Equals(secondString, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsEmpty(this string stringValue)
        {
            return string.IsNullOrWhiteSpace(stringValue);
        }

        public static bool IsNotEmpty(this string stringValue)
        {
            return !string.IsNullOrWhiteSpace(stringValue);
        }

        public static string ToDataType(this string dataTypeName)
        {
            switch (dataTypeName.ToLower())
            {
                case "bigint":
                    return "long";
                case "binary":
                case "image":
                case "varbinary":
                    return "byte[]";
                case "bit":
                    return "bool";
                case "char":
                    return "char";
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    return "DateTime";
                case "decimal":
                case "money":
                case "numeric":
                    return "decimal";
                case "float":
                    return "double";
                case "int":
                    return "int";
                case "nchar":
                case "nvarchar":
                case "text":
                case "mediumtext":
                case "longtext":
                case "varchar":
                case "xml":
                    return "string";
                case "real":
                    return "single";
                case "smallint":
                    return "short";
                case "tinyint":
                    return "byte";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    return null;
            }
        }

        public static string PluralToSingular(this string name)
        {
            var singularName = Strings.ToProper(Strings.PluralToSingular(name));

            var words = new List<string>
            {
                "source", "condition", "location", "status", "type", "site", "variable", "account",
                "externalxref", "nocosmetic", "unit", "value", "format", "file", "tag", "medium", "template",
                "shipment", "data", "item", "group", "key", "info", "tracking", "container", "piece", "set", "transfer", "state"
            };

            foreach (var word in words)
            {
                if (Regex.IsMatch(name, word))
                {
                    return Regex.Replace(singularName, word, delegate (Match match)
                    {
                        var v = match.ToString();

                        return char.ToUpper(v[0]) + v.Substring(1);
                    });
                }
            }

            return singularName;
        }

        public static string ToProperCaseWord(this string source)
        {
            var words = new List<string>
            {
                "ID"
            };

            foreach (var word in words)
            {
                if (Regex.IsMatch(source, word))
                {
                    return Regex.Replace(source, word, delegate (Match match)
                    {
                        var v = match.ToString();

                        return char.ToUpper(v[0]) + v.Substring(1).ToLower();
                    });
                }
            }

            return source;
        }

        public static bool IsTokenString(this string tokenString)
        {
            return Regex.IsMatch(tokenString, @"{(\w+)}");
        }

        /// <summary>
        /// Replaces tokens like {PublicProperty} with the instance's PublicProperty value,
        /// and optionally applies onEachToken to each parsed token value
        /// </summary>
        /// <param name="tokenString"></param>
        /// <param name="entity"></param>
        /// <param name="onEachToken">The function to call on each token value</param>
        /// <returns></returns>
        public static string TokensToValue(this string tokenString, object entity, Func<string, string> onEachToken = null)
        {
            var values = tokenString;

            // Match all {PublicProperty}
            foreach (var m in Regex.Matches(tokenString, @"{(\w+)}"))
            {
                // Get the property name by removing the curly braces
                var propertyName = m.ToString().Trim('{', '}');

                // Get the value of the property
                var value = entity.PropertyValue<string>(propertyName);

                // If onEachToken is specified, apply it, otherwise use "value"
                values = values.Replace(m.ToString(), onEachToken != null ? onEachToken(value) : value);
            }

            return values;
        }

        /// <summary>
        /// Replaces tokens like {PublicProperty} with the instance's PublicProperty value,
        /// and optionally applies onEachToken to each parsed token value
        /// </summary>
        /// <param name="tokenString"></param>
        /// <param name="entity"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        private static string TokensToValues(this string tokenString, object entity, IFormatProvider formatProvider)
        {
            if (entity == null) throw new ArgumentNullException();

            var type = entity.GetType();
            var cache = new Dictionary<string, string>();

            return TokenPatternRegex.Replace(tokenString, match =>
            {
                var formatPattern = "{0}";

                int lCount = match.Groups[1].Value.Length,
                    rCount = match.Groups[3].Value.Length;

                if (lCount % 2 != rCount % 2) throw new InvalidOperationException("Unbalanced Braces");

                string key = match.Groups[2].Value, value;

                if (lCount % 2 == 0)
                {
                    value = key;
                }
                else
                {
                    if (key.Contains(":"))
                    {
                        // Get the format specifier
                        var formatSpecifier = key.Split(':')[1];

                        // Set the format pattern to {0:formatSpecifier}
                        formatPattern = $"{{0:{formatSpecifier}}}";

                        // Set the key to the first part of the format pattern
                        key = key.Split(':')[0];
                    }

                    if (!cache.TryGetValue(key, out value))
                    {
                        var prop = type.GetProperty(key);

                        if (prop != null)
                        {
                            value = Convert.ToString(prop.GetValue(entity, null));

                            cache.Add(key, value);
                        }
                    }
                }

                return string.Format(formatProvider, formatPattern, value);
            });
        }

        private static readonly Regex TokenPatternRegex = new Regex(@"(\{+)([^\}]+)(\}+)", RegexOptions.Compiled);
    }
}