using System;

namespace LysCore.Common
{
    public static class Requires
    {
        public static void NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(parameterName);
            }
        }

        public static void NotNullGuid(Guid value, string parameterName)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}