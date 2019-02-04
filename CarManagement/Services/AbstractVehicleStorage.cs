using System.Collections.Generic;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public abstract class AbstractVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, IVehicle> vehicles;

        public AbstractVehicleStorage(IDictionary<IEnrollment, IVehicle> initialVehicles)
        {
            Asserts.isNotNull(initialVehicles);
            this.vehicles = initialVehicles;
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
            this.save(this.vehicles.Values);
        }

        public IVehicle get(IEnrollment enrollment)
        {
            bool vehicleExists = this.vehicles.TryGetValue(enrollment, out IVehicle vehicleResult);
            Asserts.isTrue(vehicleExists);

            return vehicleResult;
        }

        public IEnumerable<IVehicle> getAll()
        {
            foreach (KeyValuePair<IEnrollment, IVehicle> entry in this.vehicles)
            {
                yield return entry.Value;
            }
        }

        public void set(IVehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            this.save(this.vehicles.Values);
        }

        protected abstract void save(IEnumerable<IVehicle> vehicles);
    }
}
