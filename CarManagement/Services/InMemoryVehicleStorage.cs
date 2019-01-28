using CarManagement.Builders;
using CarManagement.Models;
using System.Collections;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        IDictionary<IEnrollment, Vehicle> vehicles;


        public InMemoryVehicleStorage()
        {
            
            this.vehicles = new Dictionary<IEnrollment ,Vehicle>();

        }
        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }

        }

        public void clear()
        {
            Asserts.isTrue(this.vehicles.Count  > 0);
            this.vehicles.Clear();
 
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle vehicleValor;
            bool hasVehicle = vehicles.TryGetValue(defaultEnrollment, out vehicleValor);
            Asserts.isTrue(hasVehicle);
            return vehicleValor;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(motoVehicle .Enrollment));
            this.vehicles.Add(motoVehicle.Enrollment, motoVehicle);

        }
    }
}