using System.Collections.Generic;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public abstract class AbstractVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, IVehicle> vehicles;
        private bool disposed;

        public AbstractVehicleStorage(IDictionary<IEnrollment, IVehicle> initialVehicles)
        {
            Asserts.isNotNull(initialVehicles);
            this.vehicles = initialVehicles;
            this.disposed = false;
        }

        ~AbstractVehicleStorage()
        {
            Dispose();
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
            this.vehicles.Values.CopyTo(vehicleArray,0);
            return vehicleArray;
       
        }

        public void set(IVehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment, vehicle);
        }

        public void Dispose()
        {
            if (this.disposed == false)
            {
                save(this.vehicles.Values);
                this.disposed = true;
            }
        }

        protected abstract void save(IEnumerable<IVehicle> vehicles);
    }
}
