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

        public void remove(IEnrollment enrollment)
        {
            Asserts.isTrue(this.vehicles.ContainsKey(enrollment));
            this.vehicles.Remove(enrollment);
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private IEnumerable<IVehicle> filters;
            private IEnumerable<IVehicle> vehicles;

            public PrvVehicleQuery(IEnumerable<IVehicle> vehicles)
            {
                this.filters = vehicles;
            }

            public IEnumerable<IEnrollment> Keys
            {
                get
                {
                    return this.vehicles
                        .Select(v => v.Enrollment);
                }
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return this.filters.GetEnumerator();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                this.filters = this.filters.Where(vehicle => vehicle.Color == color);
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.filters = this.filters.Where(vehicle => vehicle.Engine.IsStarted == started);
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {

                this.filters = this.filters.Where(vehicle => vehicle.Enrollment.Serial == enrollment.Serial 
                                                          && vehicle.Enrollment.Number == enrollment.Number);
                return this;
            }
                
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                this.filters = this.filters.Where(vehicle => vehicle.Enrollment.Serial == serial);
                return this;
            }
            public IVehicleQuery whereEnrollmentNumberIs(int number)
            {
                this.filters = this.filters.Where(vehicle => vehicle.Enrollment.Number == number);
                return this;
            }


            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.filters = this.filters.Where(vehicle => vehicle.Engine.HorsePower == horsePower);
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                this.filters = this.filters.Where(vehicle => vehicle.Engine.HorsePower >= min && vehicle.Engine.HorsePower <= max);
                return this;
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.filters.GetEnumerator();
            }
        }
    }
}
