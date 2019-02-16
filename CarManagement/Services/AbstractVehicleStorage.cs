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
            private const string WHERE_ENROLLMENT = "WHERE_ENROLLMENT";
            private const string WHERE_HORSEPOWER = "WHERE_HORSEPOWER";

            private IEnumerable<IVehicle> vehicles;
            private readonly ISet<string> filters;

            public PrvVehicleQuery(IEnumerable<IVehicle> vehicles)
            {
                this.vehicles = vehicles;
                this.filters = new HashSet<string>();
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
                return this.vehicles.GetEnumerator();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isFalse(this.filters.Contains(nameof(whereColorIs)));
                this.filters.Add(nameof(whereColorIs));
                this.vehicles = this.vehicles
                    .Where(vehicle => vehicle.Color == color);

                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isFalse(this.filters.Contains(nameof(whereEngineIsStarted)));
                this.filters.Add(nameof(whereEngineIsStarted));
                this.vehicles = this.vehicles
                    .Where(v => v.Engine.IsStarted == started);

                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isFalse(this.filters.Contains(WHERE_ENROLLMENT));
                this.filters.Add(WHERE_ENROLLMENT);

                this.vehicles = this.vehicles
                    .Where(v => v.Enrollment.Serial == enrollment.Serial 
                        && v.Enrollment.Number == enrollment.Number);

                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isFalse(this.filters.Contains(WHERE_ENROLLMENT));
                this.filters.Add(WHERE_ENROLLMENT);

                this.vehicles = this.vehicles
                    .Where(v => v.Enrollment.Serial == serial);

                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isFalse(this.filters.Contains(WHERE_HORSEPOWER));
                this.filters.Add(WHERE_HORSEPOWER);

                this.vehicles = this.vehicles
                    .Where(v => v.Engine.HorsePower == horsePower);

                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isFalse(this.filters.Contains(WHERE_HORSEPOWER));
                this.filters.Add(WHERE_HORSEPOWER);

                this.vehicles = this.vehicles
                    .Where(v => min <= v.Engine.HorsePower && v.Engine.HorsePower <= max);

                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.vehicles.GetEnumerator();
            }
        }
    }
}
