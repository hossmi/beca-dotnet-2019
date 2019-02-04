using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        public InMemoryVehicleStorage()
            : base(load())
        {
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
        }

        private static IDictionary<IEnrollment, IVehicle> load()
        {
            return new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());
        }
    }
}