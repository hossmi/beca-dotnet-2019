using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly string filePath;
                
        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

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
            this.vehicles.Clear();

            writeToFile(this.filePath);
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

            writeToFile(this.filePath);
        }

        private void writeToFile(string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(DataSet));
            TextWriter writer = new StreamWriter(filePath);
            //ser.Serialize(writer, ds);
            
            writer.Close();




            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization
            throw new NotImplementedException();
        }

        private IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            throw new NotImplementedException();
        }

    }
}