using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        public InMemoryVehicleStorage() 
            : base(new Dictionary<IEnrollment, Vehicle>())
        {
            //No need
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            //Nope
        }
    }
}