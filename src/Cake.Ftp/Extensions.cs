using System;

namespace Cake.Ftp {
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c></exception>
        public static void NotNull<T>([ValidatedNotNull] this T value, string parameterName) where T: class {
            if(value == null) {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null, empty or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is empty or consists only of white-space characters</exception>       
        public static void NotNullOrWhiteSpace([ValidatedNotNull]this string value, string parameterName) {
            if (value == null) {
                throw new ArgumentNullException(parameterName);
            }

            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }
    }
}
