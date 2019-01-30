using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        public InMemoryVehicleStorage() : base(new Dictionary<IEnrollment, Vehicle>())
        { }

        protected override void save(IEnumerable<Vehicle> vehicles)
        { }
    }
}