using CarManagement.Builders;
using CarManagement.Models;
using System.Collections;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        private IDictionary<IEnrollment, Vehicle> vehicles;

        public InMemoryVehicleStorage()
        {
            this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }

        public int Count { get => this.vehicles.Count; }

        public void clear()
        {
            this.vehicles.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            bool hasVehicle = this.vehicles.TryGetValue(enrollment, out Vehicle returnedVehicle);

            Asserts.isTrue(hasVehicle, "El vehículo no está en el diccionario");

            return returnedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle.Enrollment, vehicle);
        }
    }
}