using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

        public int Count {
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
            Vehicle vehicle;
            bool exists = vehicles.TryGetValue(enrollment, out vehicle);
            Asserts.isTrue(exists);
            return vehicle;
        }

        public void set(Vehicle vehicle)
        {
            bool exists = vehicles.ContainsKey(vehicle.Enrollment);
            Asserts.isFalse(exists);
            this.vehicles.Add(vehicle.Enrollment, vehicle);
        }
    }
}