using CarManagement.Models;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        public InMemoryVehicleStorage() 
            : base(new Dictionary<IEnrollment, Vehicle>())
        {
            //No need
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            //Nope
        }
    }
}