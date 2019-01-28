using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        //private List<Vehicle> vehicles = new List<Vehicle>();
        IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

        public int Count {
            get
            {
                return this.vehicles.Count;
            }
        }

        public void clear()
        {
            vehicles.Clear();
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            /*bool exist = false;
            int position = 0;

            for (int i = 0; i < Count && !exist; i++)
            {
                if(vehicles[i].Enrollment == defaultEnrollment)
                {
                    exist = true;
                    position = i;
                }
            }

            Asserts.isTrue(exist);
            return vehicles[position];*/

            Vehicle vehicle;
            bool exists = vehicles.TryGetValue(defaultEnrollment, out vehicle);
            Asserts.isTrue(exists);
            return vehicle;
        }

        public void set(Vehicle motoVehicle)
        {
            bool exists = vehicles.ContainsKey(motoVehicle.Enrollment);
            Asserts.isFalse(exists);
            this.vehicles.Add(motoVehicle.Enrollment, motoVehicle);
        }
    }
}