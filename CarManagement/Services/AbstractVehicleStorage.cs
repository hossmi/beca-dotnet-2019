using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //public IVehicle get(IEnrollment enrollment)
        //{
        //    IVehicle vehicleResult;

        //    bool vehicleExists = this.vehicles.TryGetValue(enrollment, out vehicleResult);
        //    Asserts.isTrue(vehicleExists);

        //    return vehicleResult;
        //}

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

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.vehicles.Values);
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private IEnumerable<IVehicle> vehicles;

            public PrvVehicleQuery(IEnumerable<IVehicle> vehicles)
            {
                this.vehicles = vehicles;
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return this.vehicles.GetEnumerator();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Color == color);
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Engine.IsStarted == true);
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                EnrollmentEqualityComparer enrollmentEqualityComparer = new EnrollmentEqualityComparer();
                this.vehicles = this.vehicles.Where(vehicle => enrollmentEqualityComparer.Equals(vehicle.Enrollment,enrollment));
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Enrollment.Serial == serial);
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Engine.HorsePower == horsePower);
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Engine.HorsePower >= min);
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Engine.HorsePower <= max);
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.vehicles.GetEnumerator();
            }
        }
    }
}
