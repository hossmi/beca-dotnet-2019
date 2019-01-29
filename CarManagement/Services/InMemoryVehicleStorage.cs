using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {

        private List<Vehicle> list_vehicle = new List<Vehicle>();

        public int Count {
            get
            {
                return list_vehicle.Count;
            }
        }

        public void clear()
        {
            list_vehicle.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle T2 = new Vehicle();
            bool vehicleFound = false;

            foreach (Vehicle T in list_vehicle)
            {
                if(enrollment == T.Enrollment);
                {
                    vehicleFound = true;
                    T2 = T;
                }

            }
            Asserts.isTrue(vehicleFound);
            return T2;
        }

        public void set(Vehicle vehicle)
        {
            list_vehicle.Add(vehicle);
        }
    }
}