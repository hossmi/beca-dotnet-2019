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
            this.vehicles.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            //Asserts.isTrue(vehicles.Count > 0,"La lista de vehiculos esta vacia");
            Vehicle vehicle;
            bool vehicleFound = this.vehicles.TryGetValue(enrollment, out vehicle);
            Asserts.isTrue(vehicleFound,"Cannot find vehicle");
            return vehicle;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment), "A vehicle already exists with the same enrollment");
            vehicles.Add(vehicle.Enrollment, vehicle);
        }
    }
}