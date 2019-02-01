using System;
using System.Collections.Generic;
using CarManagement.Models;
using System.Collections;
using System.Collections.Generic;


namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        IDictionary<IEnrollment, Vehicle> vehicles;

        public InMemoryVehicleStorage()
        {
            this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }

        public int Count {
            get
            {
                return this.vehicles.Count;
            }

        }

        public void clear()
            //public InMemoryVehicleStorage() 
            //: base(load())
        {
           this.vehicles.Clear();
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            Vehicle vehicle;
            bool hasVehicle = this.vehicles.TryGetValue(enrollment, out vehicle);
            Asserts.isTrue(hasVehicle);
            return vehicle;

        }

        private static IDictionary<IEnrollment, Vehicle> load()
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment,vehicle);
            return new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer()); //linea añadida
        }
    }
}