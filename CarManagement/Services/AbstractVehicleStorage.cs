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
            save(this.vehicles.Values);
        }

        public IVehicle get(IEnrollment enrollment)
        {
            IVehicle vehicleResult;

            bool vehicleExists = this.vehicles.TryGetValue(enrollment, out vehicleResult);
            Asserts.isTrue(vehicleExists);

            return vehicleResult;
        }

        public IEnumerable<IVehicle> getAll()
        {
            IVehicle[] vehicleArray = new IVehicle[this.vehicles.Count];

            int i = 0;
            foreach (IVehicle v in this.vehicles.Values)
            {
                vehicleArray[i] = v;
                i++;
            }

            return vehicleArray;
        }

        public void set(IVehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            save(this.vehicles.Values);
        }

        protected abstract void save(IEnumerable<IVehicle> vehicles);
    }
}