using System;
using System.Collections.Generic;
using CarManagement.Models;
using System.Collections;



namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        IDictionary<IEnrollment, Vehicle> vehicles;

          public InMemoryVehicleStorage() : base(load())
        {
           this.vehicles.Clear();
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {

            load(this.vehicles );

        }

        private static IDictionary<IEnrollment, Vehicle> load()
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment,vehicle);
            return new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer()); //linea añadida
        }
    }
}