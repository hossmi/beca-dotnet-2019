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

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            bool hasVehicle = this.vehicles.TryGetValue(defaultEnrollment, out Vehicle returnedVehicle);

            Asserts.isTrue(hasVehicle, "El vehículo no está en el diccionario");

            return returnedVehicle;
        }

        public void set(Vehicle motoVehicle)
        {
            this.vehicles.Add(motoVehicle.Enrollment, motoVehicle);
        }
    }
}