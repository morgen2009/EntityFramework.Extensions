namespace EntityFramework.Extensions.Commons
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class Guard
    {
        public static void NotNull(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(message);
            }
        }

        internal static void NotNull<T>(T value, string message) where T : class 
        {
            if (value == null)
            {
                throw new ArgumentException(message);
            }
        }

        public static void IsFalse(bool value, string message)
        {
            if (value)
            {
                throw new ArgumentException(message);
            }
        }
    }
}