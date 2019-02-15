using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly IEnrollmentProvider enrollmentProvider;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder, IEnrollmentProvider enrollmentProvider)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            this.enrollmentProvider = enrollmentProvider;
        }

        public int Count { get; }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private CarColor color;
            private bool colorHasValue;

            public IEnumerable<IEnrollment> Keys
            {
                get
                {
                    return enumerateEnrollments();
                }
            }

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = enrollmentProvider;
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                this.color = color;
                this.colorHasValue = true;
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                throw new NotImplementedException();
            }

            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                throw new NotImplementedException();
            }

        }
    }
}
