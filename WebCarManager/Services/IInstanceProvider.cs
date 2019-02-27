using System;
using CarManagement.Services;

namespace WebCarManager.Services
{
    public interface IInstanceProvider
    {
        void register<T>(Func<T> buildDelegate);
        T get<T>();
    }
}