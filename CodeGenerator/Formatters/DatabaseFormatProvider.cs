using System;

using CodeGenerator.Extensions;

namespace CodeGenerator.Formatters
{
    public class DatabaseFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Check whether this is an appropriate callback
            if (!Equals(formatProvider))
            {
                return null;
            }

            var sourceString = arg.ToString();

            if (format == "U")
                sourceString = sourceString.ToUpper();
            else if (format == "L")
                sourceString = sourceString.ToLower();
            else if (format == "S")
                sourceString = sourceString.PluralToSingular();
            else if (format == "P")
                sourceString = SubSonic.Sugar.Strings.SingularToPlural(sourceString);

            return sourceString;
        }
    }
}