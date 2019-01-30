using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Models;

namespace CarManagement.Services
{
    public abstract class AbstractVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;

        public AbstractVehicleStorage()
        {
            this.vehicles = load();
            Asserts.isNotNull(this.vehicles);
        }

        public int Count { get => this.vehicles.Count; }

        public void clear()
        {
            this.vehicles.Clear();
            save(this.vehicles.Values);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle vehicleResult;

            bool vehicleExists = this.vehicles.TryGetValue(enrollment, out vehicleResult);
            Asserts.isTrue(vehicleExists);

            return vehicleResult;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            save(this.vehicles.Values);
        }

        protected abstract IDictionary<IEnrollment, Vehicle> load();
        protected abstract void save(IEnumerable<Vehicle> vehicles);
    }
}
