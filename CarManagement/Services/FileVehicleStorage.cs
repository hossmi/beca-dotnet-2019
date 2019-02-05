using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.filePath = fileFullPath;
            this.vehicles = readFromFile(fileFullPath, this.dtoConverter);
            this.dtoConverter = dtoConverter;
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
            throw new System.NotImplementedException();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle returnedVehicle = default(Vehicle);
            foreach(Vehicle vehicle in this.vehicles.Values)
            {
                if (vehicle.Enrollment == enrollment)
                {
                    returnedVehicle = vehicle;
                }
            }
            return returnedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
            this.vehicles.Add(vehicle.Enrollment, vehicle);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization
            throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            throw new NotImplementedException();
        }

    }
}