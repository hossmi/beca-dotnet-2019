using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Models;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;
        private List<Vehicle> listvehicle = new List<Vehicle>();


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
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
            this.vehicles.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle vehicle;
            bool vehicleExists = this.vehicles.TryGetValue(enrollment, out vehicle);
            Asserts.isTrue(vehicleExists);
            return vehicle;
        }

        public void set(Vehicle vehicle)
        {
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            /*int temp = 0;
            foreach (Vehicle vehicleArr in vehicle)
            {
                vehicle[temp] = vehicleArr;
                temp++;
            }
            XmlSerializer ser = new XmlSerializer(typeof(Vehicle[]));*/
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization
            throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            
            throw new NotImplementedException();
        }

    }
}