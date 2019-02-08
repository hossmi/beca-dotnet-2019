using CarManagement.Models;
using System;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;

        public InMemoryVehicleStorage() : base (load())
        {
        }

        private static IDictionary<IEnrollment, Vehicle> load()
        {
            return new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
        }
    }
}