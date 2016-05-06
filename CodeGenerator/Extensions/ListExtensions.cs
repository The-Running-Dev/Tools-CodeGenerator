using System.Linq;
using System.Collections.Generic;

namespace CodeGenerator.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringList"></param>
        /// <param name="formatString"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string AsString(this List<string> stringList, string formatString = "{0}", string delimiter = ",")
        {
            return string.Join(delimiter, stringList.Select(item => string.Format(formatString, item)));
        }
    }
}