using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Core.Services
{
    public interface IVehicleStorage : IDisposable
    {
        int Count { get; }
        void set(IVehicle vehicle);
        void clear();
        IVehicleQuery get();
    }
}