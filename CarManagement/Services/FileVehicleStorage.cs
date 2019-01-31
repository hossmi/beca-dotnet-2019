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
             this.dtoConverter = dtoConverter;
            this.vehicles = readFromFile(fileFullPath, this.dtoConverter);
           
        }

        public int Count { get; }

        public void clear()
        {
            this.vehicles.Clear();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);

        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle vehicle;
            bool hasVehicle = this.vehicles.TryGetValue(enrollment, out vehicle);
            Asserts.isTrue(hasVehicle);
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
            return vehicle;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
            throw new System.NotImplementedException();
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization

            VehicleDto[] listVehicles = new VehicleDto[vehicles.Count];
            int aux = 0;
            foreach (Vehicle v in vehicles.Values)
            {

                listVehicles[aux] = dtoConverter.convert(v);
                aux++;
            }
            XmlSerializer ser = new XmlSerializer(typeof(Vehicle[]));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, listVehicles);
            writer.Close();

        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)


        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();
            // te pasan una ruta y 
            if (File.Exists(fileFullPath) == true)
            {
                XmlSerializer ser = new XmlSerializer(typeof(Vehicle[]));
                TextReader reader = new StreamReader(fileFullPath);
                VehicleDto[] listVehicle = (VehicleDto[])ser.Deserialize(reader); //guardo lo que he leido en un array de vehiculos.
                reader.Close();

                foreach (VehicleDto v in listVehicle)
                {

                    Vehicle vehicle = dtoConverter.convert(v);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }
           
            return vehicles;



        }
    }
}