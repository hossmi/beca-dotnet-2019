using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
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

        
    }
}