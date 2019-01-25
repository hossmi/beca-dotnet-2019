using System;
using System.Runtime.Serialization;

namespace CarManagement.Builders
{
    class Asserts
    {
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

        public static void isTrue(bool condition)
        {
            if (condition == false)
                throw new AssertsException();
        }

        public static void isFalse(bool condition)
        {
            isTrue(condition == false);
        }

        public static void stringIsFilled(string text)
        {
            isFalse(string.IsNullOrWhiteSpace(text));
        }

        public static void isNotNull<T>(T item) where T : class
        {
            isTrue(item != null);
        }

        public static void isNull<T>(T item) where T : class
        {
            isTrue(item == null);
        }

        public static void isEnumDefined<TItem>(TItem value)
        {
            isTrue(typeof(TItem).IsEnum);
            isTrue(Enum.IsDefined(typeof(TItem), value));
        }
    }
}