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
            this.vehicles = readFromFile(fileFullPath, this.dtoConverter);
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
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

            if (File.Exists(fileFullPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Vehicle[]));
                TextReader vehiclesReader = new StreamReader(fileFullPath);
                Vehicle[] vehiclesAux = (Vehicle[]) xmlSerializer.Deserialize(vehiclesReader);
                vehiclesReader.Close();
                
                foreach (Vehicle vehicle in vehiclesAux)
                {                    
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }
            else
            {
                return vehicles;
            }

            return vehicles;
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
            throw new System.NotImplementedException();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization

            /*XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleDto));
            VehicleDto vehicleDto = new VehicleDto();

            StreamWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, xmlSerializer);
            vehiclesWriter.Close();
        }
        */

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            Vehicle[] vehiclesAux = new Vehicle[vehicles.Count];
            int contFor = 0;
            foreach(Vehicle vehicle in vehicles.Values)
            {
                vehiclesAux[contFor] = vehicle;
                contFor++;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(vehiclesAux.GetType());
            TextWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, xmlSerializer);
            vehiclesWriter.Close();
        }

    }
}