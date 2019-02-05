using CarManagement.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;

        public FileVehicleStorage()
        {
            this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }
        public int Count {
            get
            {
                return this.vehicles.Count;
            }
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            Vehicle returnedVehicle = default(Vehicle);
            foreach(Vehicle vehicle in this.vehicles.Values)
            {
                if (vehicle.Enrollment == defaultEnrollment)
                {
                    returnedVehicle = vehicle;
                }
            }
            return returnedVehicle;
        }

        public void set(Vehicle motoVehicle)
        {
            this.vehicles.Add(motoVehicle.Enrollment,motoVehicle);
        }
    }
}