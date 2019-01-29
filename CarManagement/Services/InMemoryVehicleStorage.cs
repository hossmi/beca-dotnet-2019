using CarManagement.Models;
using System.Collections;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        IDictionary<IEnrollment,Vehicle> vehicles;

        public int Count {
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
            Vehicle vehicle;
            bool hasVehicle = this.vehicles.TryGetValue(enrollment, out vehicle);
            Asserts.isTrue(hasVehicle);
            return vehicle;

        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment,vehicle);
        }
    }
}