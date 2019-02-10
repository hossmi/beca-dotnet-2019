using System;
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

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
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

        public IVehicle get(IEnrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicle> getAll()
        {
            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
