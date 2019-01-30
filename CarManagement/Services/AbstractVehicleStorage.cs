using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Services
{
    public abstract class AbstractVehicleStorage : IVehicleStorage
    {
        private IDictionary<IEnrollment, Vehicle> vehicles;
        private bool loaded;

        public AbstractVehicleStorage()
        {
            this.vehicles = null;
            this.loaded = false;
        }

        public int Count
        {
            get
            {
                enruseLoad();
                return this.vehicles.Count;
            }
        }

        public void clear()
        {
            enruseLoad();
            this.vehicles.Clear();
            save(this.vehicles.Values);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle vehicleResult;

            enruseLoad();

            bool vehicleExists = this.vehicles.TryGetValue(enrollment, out vehicleResult);
            Asserts.isTrue(vehicleExists);

            return vehicleResult;
        }

        public void set(Vehicle vehicle)
        {
            enruseLoad();

            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            save(this.vehicles.Values);
        }

        private void enruseLoad()
        {
            if (this.loaded == false)
            {
                this.vehicles = load();
                Asserts.isNotNull(this.vehicles);
                this.loaded = true;
            }
        }

        protected abstract IDictionary<IEnrollment, Vehicle> load();
        protected abstract void save(IEnumerable<Vehicle> vehicles);
    }
}
