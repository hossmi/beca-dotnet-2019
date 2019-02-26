using System;
using System.Collections.Generic;
using CarManagement.Core;

namespace WebCarManager.Services
{
    public class DefaultInstanceProvider : IInstanceProvider
    {
        private readonly IDictionary<Type, object> instances;

        static DefaultInstanceProvider()
        {
            Instance = new DefaultInstanceProvider();
        }

        private DefaultInstanceProvider()
        {
            this.instances = new Dictionary<Type, object>();
        }

        public static IInstanceProvider Instance { get; }

        public T get<T>()
        {
            Type typeOfT = typeof(T);
            object instance;

            Asserts.isTrue(this.instances.TryGetValue(typeOfT, out instance));
            return (T)instance;
        }

        public void register<T>(Func<T> buildDelegate)
        {
            T instance = buildDelegate();
            this.instances.Add(typeof(T), instance);
        }
    }
}