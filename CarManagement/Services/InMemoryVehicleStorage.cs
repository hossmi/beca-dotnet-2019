using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;

        public InMemoryVehicleStorage()
        {
            this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }
        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public void clear()
        {
            this.vehicles.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle returnedVehicle;

            bool exists = this.vehicles.TryGetValue(enrollment, out returnedVehicle);

            Asserts.isTrue(exists);

            return returnedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle.Enrollment, vehicle);
        }
    }
}