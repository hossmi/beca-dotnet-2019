using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Models;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath)
        {
            this.filePath = fileFullPath;

            if (File.Exists(fileFullPath))
                this.vehicles = readFromFile(fileFullPath);
            else
                this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }

        public int Count { get; }

        public void clear()
        {
            throw new System.NotImplementedException();
            writeToFile(this.filePath, this.vehicles);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            throw new System.NotImplementedException();
        }

        public void set(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
            writeToFile(this.filePath, this.vehicles);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization
            throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            throw new NotImplementedException();
        }

    }
}