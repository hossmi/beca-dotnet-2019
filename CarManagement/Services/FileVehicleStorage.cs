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

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.filePath = fileFullPath;
            this.vehicles = readFromFile(fileFullPath);
            this.dtoConverter = dtoConverter;
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

            Vehicle[] vehiclesArray = new Vehicle[vehicles.Count];
            int aux=0;
            foreach (Vehicle v in  vehicles.Values)
            {
                vehiclesArray[aux] = v;
                aux++;
            }

            XmlSerializer ser = new XmlSerializer(typeof(Vehicle[]));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, vehiclesArray);
            writer.Close();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            IDictionary<IEnrollment, Vehicle> vehicles= new Dictionary<IEnrollment, Vehicle>();

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Vehicle[]));
                TextReader reader = new StreamReader(fileFullPath);
                Vehicle[] vehicleArray=(Vehicle[])ser.Deserialize(reader);
                reader.Close();

                foreach (Vehicle v in vehicleArray)
                {
                    vehicles.Add(v.Enrollment, v);
                }
            }
            return vehicles;
        }
    }
}