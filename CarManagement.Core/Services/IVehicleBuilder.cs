using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;

namespace CarManagement.Core.Services
{
    public interface IVehicleBuilder : IEnrollmentImporter
    {
        void addWheel();
        void removeWheel();
        IVehicle build();
        void setColor(CarColor color);
        void setDoors(int doorsCount);
        void setEngine(int horsePorwer);
        IVehicle import(VehicleDto vehicleDto);
        VehicleDto export(IVehicle vehicleDto);
    }
}