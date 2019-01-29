using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Models;

namespace CarManagement.Services
{
    public abstract class AbstractVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;

        public AbstractVehicleStorage()
        {

        }

        public int Count { get; }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public void set(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
