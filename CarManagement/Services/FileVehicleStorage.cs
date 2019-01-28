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

        public int Count { get; }

        public FileVehicleStorage(string fileFullPath)
        {
            this.filePath = fileFullPath;

            if (File.Exists(fileFullPath))
                this.vehicles = readFromFile(fileFullPath);
            else
                this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }

        public void clear()
        {
            throw new System.NotImplementedException();
            writeToFile(this.filePath);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            throw new System.NotImplementedException();
        }

        public void set(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
            writeToFile(this.filePath);
        }

        private void writeToFile(string filePath)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization
            throw new NotImplementedException();
        }

        private IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            throw new NotImplementedException();
        }

    }
}