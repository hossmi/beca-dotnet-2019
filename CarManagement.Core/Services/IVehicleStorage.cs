using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Core.Services
{
    public interface IVehicleStorage
    {
        int Count { get; }

        void set(IVehicle vehicle);
        IVehicle get(IEnrollment enrollment);
        void clear();
        IEnumerable<IVehicle> getAll();
    }
}
