using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        public InMemoryVehicleStorage() : base(load())
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