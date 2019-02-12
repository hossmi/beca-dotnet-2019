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

        //public IEnumerable<IVehicle> getAll()
        //{
        //    IVehicle[] vehiclesArray = new IVehicle[this.vehicles.Count];
        //    this.vehicles.Values.CopyTo(vehiclesArray, 0);

        //    return vehiclesArray;
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
            private bool hasStarted;
            private bool hasColor;
            private bool hasHorsePowerBetween;
            private bool hasHorsePower;
            private bool hasSerial;
            private bool hasEnrollment;

            public PrvVehicleQuery(IEnumerable<IVehicle> vehicles)
            {
                this.vehicles = vehicles;
                this.hasStarted = false;
                this.hasColor = false;
                this.hasHorsePowerBetween = false;
                this.hasHorsePower = false;
                this.hasSerial = false;
                this.hasEnrollment = false;
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return this.vehicles.GetEnumerator();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isFalse(this.hasColor);
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Color == color);
                this.hasColor = true;
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isFalse(this.hasStarted);
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Engine.IsStarted == started);
                this.hasStarted = true;
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isFalse(this.hasSerial && this.hasEnrollment);
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Enrollment == enrollment);
                this.hasEnrollment = true;
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isFalse(this.hasEnrollment && this.hasSerial);
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Enrollment.Serial == serial);
                this.hasSerial = true;
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isFalse(this.hasHorsePowerBetween && this.hasHorsePower);
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Engine.HorsePower == horsePower);
                this.hasHorsePower = true;
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isFalse(this.hasHorsePower && this.hasHorsePowerBetween);
                this.vehicles = this.vehicles.Where(vehicle => vehicle.Engine.HorsePower >= min && vehicle.Engine.HorsePower <= max);
                this.hasHorsePowerBetween = true;
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.vehicles.GetEnumerator();
            }
        }
    }
}
