using CarManagement.Models;

namespace CarManagement.Builders
{
    public interface IVehicleBuilder
    {
        void addWheel();
        void removeWheel();
        Vehicle build();
        void setColor(CarColor color);
        void setDoors(int doorsCount);
        void setEngine(int horsePorwer);
    }
}