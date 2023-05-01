using System;
using System.Linq;
using System.Reflection;

namespace PlacowkaOswiatowaQuiz.Shared.Extensions
{
    public static class PropertyUtil
    {
        public static TTarget Map<TTarget, TSource>(this TTarget targetObject, TSource sourceObject)
        {
            CopyProperties(targetObject, sourceObject);
            return targetObject;
        }

        public static void CopyProperties<T, T2>(T targetObject, T2 sourceObject)
        {
            foreach (var property in typeof(T).GetProperties().Where(p => p.CanWrite))
            {
                Func<PropertyInfo, bool> CheckIfPropertyExistInSource =
                    prop =>
                    string.Equals(property.Name, prop.Name, StringComparison.InvariantCultureIgnoreCase)
                    && prop.PropertyType.Equals(property.PropertyType);

                if (sourceObject.GetType().GetProperties().Any(CheckIfPropertyExistInSource))
                {
                    property.SetValue(targetObject, sourceObject.GetPropertyValue(property.Name), null);
                }
            }
        }

        private static object GetPropertyValue<T>(this T source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName).GetValue(source, null);
        }
    }
}

