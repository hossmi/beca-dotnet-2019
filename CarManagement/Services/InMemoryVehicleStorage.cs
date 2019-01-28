using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        static IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            throw new System.NotImplementedException();
        }

        public void set(Vehicle motoVehicle)
        {
            vehicles.Add(motoVehicle.Enrollment, motoVehicle);
        }
    }
}