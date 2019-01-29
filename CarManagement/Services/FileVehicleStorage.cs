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
            this.vehicles = readFromFile(fileFullPath);
        }

        public int Count { get => vehicles.Count; }

        public void clear()
        {
            vehicles.Clear();
            writeToFile(this.filePath, this.vehicles);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            bool hasVehicle = this.vehicles.TryGetValue(enrollment, out Vehicle returnedVehicle);

            Asserts.isTrue(hasVehicle, "El vehículo no está en el diccionario");

            return returnedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            vehicles.Add(vehicle.Enrollment, vehicle);
            writeToFile(this.filePath, this.vehicles);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles)
        {
            foreach (KeyValuePair<IEnrollment, Vehicle> entry in vehicles)
            {
                throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            throw new NotImplementedException();
        }

    }
}