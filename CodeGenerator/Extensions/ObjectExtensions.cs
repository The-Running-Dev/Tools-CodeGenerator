using System.Reflection;

namespace CodeGenerator.Extensions
{
    /// <summary>
    /// Contains extensions for the Object type
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets the property value for the instance of an object
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="obj">The object instance</param>
        /// <param name="name">The name of the property</param>
        /// <returns>Strongly typed property value</returns>
        public static T PropertyValue<T>(this object obj, string name)
        {
            var retval = PropertyValue(obj, name);

            if (retval == null) { return default(T); }

            return (T)retval;
        }

        /// <summary>
        /// Gets the property value for the instance of an object
        /// </summary>
        /// <param name="obj">The object instance</param>
        /// <param name="name">The name of the property</param>
        /// <returns>The property value</returns>
        public static object PropertyValue(this object obj, string name)
        {
            foreach (var part in name.Split('.'))
            {
                if (obj == null) { return null; }

                var type = obj.GetType();
                var info = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }
    }
}