using CarManagement.Builders;
using System;
using System.Collections.Generic;
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
        public InMemoryVehicleStorage() 
            : base(load())
        {
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
        }

        private static IDictionary<IEnrollment, Vehicle> load()
        {
            return new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());
        }
    }
}