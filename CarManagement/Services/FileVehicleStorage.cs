using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public void clear()
        {
            vehicles.Clear();
            //File clear pending implementation
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle listedVehicle;

            bool isVehicleListed = this.vehicles.TryGetValue(enrollment, out listedVehicle);
            Asserts.isTrue(isVehicleListed);

            return listedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            //File update pending implementation
        }
    }
}