using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.filePath = fileFullPath;
            this.vehicles = readFromFile(fileFullPath);
            this.dtoConverter = dtoConverter;
        }



        public void clear()
        {
            this.vehicles.Clear();

            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle listedVehicle;

            bool vehicleIsListed = vehicles.TryGetValue(enrollment, out listedVehicle);
            Asserts.isTrue(vehicleIsListed);

            return listedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            vehicles.Add(vehicle.Enrollment, vehicle);

            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }


        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {

            using (var writer = new System.IO.StreamWriter(filePath))
            {

            }
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            using (var stream = System.IO.File.OpenRead(fileFullPath))
            {
                var serializer = new XmlSerializer(typeof(VehicleDto));
                return serializer.Deserialize(stream) as IDictionary<IEnrollment, Vehicle>;
            }
        }
    }
}