using System;
using System.Collections.Generic;
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

        public static string ConvertTableName(this string name)
        {
            var singularName = Strings.ToProper(Strings.PluralToSingular(name));

            var words = new List<string>
            {
                "source", "condition", "location", "status", "type", "site"
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
    }
}