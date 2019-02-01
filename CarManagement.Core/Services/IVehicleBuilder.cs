using CarManagement.Core.Models;

namespace CarManagement.Core.Services
{
    public interface IVehicleBuilder
    {
        void addWheel();
        void removeWheel();
        IVehicle build();
        void setColor(CarColor color);
        void setDoors(int doorsCount);
        void setEngine(int horsePorwer);
    }
}