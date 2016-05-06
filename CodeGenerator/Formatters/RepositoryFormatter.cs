using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using CodeGenerator.Models;

namespace CodeGenerator.Formatters
{
    public class RepositoryFormatter
    {
        // ToDo: Implement custom  token formatter through delegates
        // So we can do ImplementsInterfaceFormatter = (token) => token.PluralToSingular(),
        public Func<string, string> TokenFormatter { get; set; }

        public RepositoryFormatter(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider ?? new DatabaseFormatProvider();
        }

        public string Format(string pattern, object entity)
        {
            return pattern != null ? TokensToValues(pattern, entity) : string.Empty;
        }

        private string TokensToValues(string tokenString, object entity)
        {
            if (entity == null) throw new ArgumentNullException();

            var type = entity.GetType();
            var cache = new Dictionary<string, string>();

            return _tokenPatternRegex.Replace(tokenString, match =>
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

                return string.Format(_formatProvider, formatPattern, value);
            });
        }

        private readonly IFormatProvider _formatProvider;
        private readonly Regex _tokenPatternRegex = new Regex(@"(\{+)([^\}]+)(\}+)", RegexOptions.Compiled);
    }
}