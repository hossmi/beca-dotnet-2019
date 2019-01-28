using System;
using System.Runtime.Serialization;

namespace CarManagement
{
    class Asserts
    {
        private const string ASSERT_FAILED_MESSAGE = "Assert failed.";

        [Serializable]
        private class AssertsException : Exception
        {
            public AssertsException()
            {
            }

            public AssertsException(string message) : base(message)
            {
            }

            public AssertsException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected AssertsException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        public static void isTrue(bool condition, string message = null)
        {
            message = message ?? ASSERT_FAILED_MESSAGE;
            if (condition == false)
                throw new AssertsException(message);
        }

        public static void isFalse(bool condition, string message = null)
        {
            isTrue(condition == false, message);
        }

        public static void stringIsFilled(string text, string message = null)
        {
            isFalse(string.IsNullOrWhiteSpace(text), message);
        }

        public static void isNotNull<T>(T item, string message = null) where T : class
        {
            isTrue(item != null, message);
        }

        public static void isNull<T>(T item, string message = null) where T : class
        {
            isTrue(item == null, message);
        }

        public static void isEnumDefined<TItem>(TItem value, string message = null)
        {
            isTrue(typeof(TItem).IsEnum, "Parameter value is no an enum.");
            isTrue(Enum.IsDefined(typeof(TItem), value), message);
        }

        public static void fail(string message = null)
        {
            message = message ?? ASSERT_FAILED_MESSAGE;
            throw new AssertsException(message);
        }
    }
}