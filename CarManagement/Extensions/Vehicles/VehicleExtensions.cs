using CarManagement.Core.Models;

namespace CarManagement.Extensions.Vehicles
{
    public static class VehicleExtensions
    {
        public static void setWheelsPressure(this IVehicle vehicle, double pressure)
        {
            foreach (IWheel wheel in vehicle.Wheels)
            {
                wheel.Pressure = pressure;
            }
        }
    }
}
