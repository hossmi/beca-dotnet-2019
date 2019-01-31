using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
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

        /*
         * IDictionary<IEnrollment, Vehicle> vehicles;

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

        public void set(Vehicle motoVehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(motoVehicle.Enrollment));
            this.vehicles.Add(motoVehicle.Enrollment, motoVehicle);
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            Vehicle vehicle;
            bool hasVehicle = this.vehicles.TryGetValue(defaultEnrollment,out vehicle);
            Asserts.isTrue(hasVehicle);
            return vehicle;
        }
        public void clear()
        {
            this.vehicles.Clear();
        }
        */

        
    }
}
 