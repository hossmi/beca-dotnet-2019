using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Core.Services
{
    public interface IVehicleQuery : IEnumerable<IVehicle>
    {
        IEnumerable<IEnrollment> Keys { get; }
        IVehicleQuery whereEnrollmentSerialIs(string serial);
        IVehicleQuery whereEnrollmentIs(IEnrollment enrollment);
        IVehicleQuery whereColorIs(CarColor color);
        IVehicleQuery whereHorsePowerEquals(int horsePower);
        IVehicleQuery whereHorsePowerIsBetween(int min, int max);
        IVehicleQuery whereEngineIsStarted(bool started);
    }
}