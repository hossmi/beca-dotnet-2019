using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        private IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

        public int Count
        {
            get
            {
                return vehicles.Count;
            }
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            Asserts.isTrue(vehicles.Count > 0,"La lista de vehiculos esta vacia");

            Vehicle vehicle;
            Asserts.isTrue(vehicles.TryGetValue(defaultEnrollment,out vehicle),"No se encuentra el vehiculo");

            return vehicle;
        }

        public void set(Vehicle motoVehicle)
        {
            vehicles.Add(motoVehicle.Enrollment, motoVehicle);
        }
    }
}