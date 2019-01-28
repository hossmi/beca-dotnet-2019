using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        private IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public void clear()
        {
            vehicles.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle listedVehicle;

            bool vehicleIsListed = vehicles.TryGetValue(enrollment, out listedVehicle);
            Asserts.isTrue(vehicleIsListed);

            return listedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            vehicles.Add(vehicle.Enrollment, vehicle);
        }
    }
}