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

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.filePath = fileFullPath;

            /*if (File.Exists(fileFullPath))
                this.vehicles = readFromFile(fileFullPath);
            else
                this.vehicles = new Dictionary<IEnrollment, Vehicle>();*/
            this.vehicles = readFromFile(fileFullPath);
            this.dtoConverter = dtoConverter;
        }

        public int Count => throw new NotImplementedException();

        public void clear()
        {
            throw new NotImplementedException();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public void set(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            throw new System.NotImplementedException();
            //TextReader vehiclesReader = new StreamReader(fileFullPath);
        }

        /*
         * private static VehicleDto createVehicleDto(IDictionary<IEnrollment, Vehicle> vehicles)
        {
            throw new System.NotImplementedException();
        }

        private static Vehicle createVehicle()
        {
            throw new System.NotImplementedException();
        }

        public void clear()
        {
            vehicles.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            throw new System.NotImplementedException();
            /*XmlSerializer xmlDesSerializer = new XmlSerializer(typeof(VehicleDto));
            
            StreamReader vehiclesReader = new StreamReader(this.filePath);
            VehicleDto vehicleDto = (VehicleDto)xmlDesSerializer.Deserialize(vehiclesReader);
            Vehicle vehicle = vehicleDto;
            return vehicle;
        }

        public void set(Vehicle vehicle)
        {
            writeToFile(this.filePath, this.vehicles);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization

            /*XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleDto));
            VehicleDto vehicleDto = new VehicleDto();

            StreamWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, xmlSerializer);
            vehiclesWriter.Close();
        }
        */

    }
}