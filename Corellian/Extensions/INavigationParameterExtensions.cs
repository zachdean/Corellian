using System;
using System.Collections.Generic;

namespace Corellian
{
    public static class INavigationParameterExtensions
    {
        /// <summary>
        /// Gets the parameter, returns default if not found or type is invalid
        /// </summary>
        /// <typeparam name="T">Parameter Type</typeparam>
        /// <param name="parameter">Navigation Parameter</param>
        /// <param name="key">Parameter Key</param>
        /// <returns></returns>
        public static T GetParameter<T>(this INavigationParameter parameter, string key)
        {
            if (!parameter.TryGetValue(key, out var result))
            {
                return default;
            }

            if (result is T castResult)
            {
                return castResult;
            }

            try
            {
                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Gets the required parameter
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="parameter">Navigation Parameter</param>
        /// <param name="key">parameter key</param>
        /// <returns>pass parameter</returns>
        /// <exception cref="KeyNotFoundException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidCastException" />
        public static T GetRequiredParameter<T>(this INavigationParameter parameter, string key)
        {
            var value = parameter[key];
            if (value is null)
            {
                throw new ArgumentNullException(key);
            }
            return (T)value;
        }
    }
}