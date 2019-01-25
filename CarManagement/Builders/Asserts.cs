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
            if(condition == false)
                throw new AssertsException();
        }

        public static void isEnumDefined<TItem>(TItem value)
        {
            isTrue(typeof(TItem).IsEnum);
            isTrue(Enum.IsDefined(typeof(TItem), value));
        }
    }
}