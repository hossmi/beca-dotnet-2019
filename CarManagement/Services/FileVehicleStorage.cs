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
            this.vehicles = readFromFile(fileFullPath); //Lee y si exite deserealiza y lo mete al dicccionario(en memoria)
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

            Vehicle[] listVehicles = new Vehicle[vehicles.Count];
            int aux = 0;
            foreach(Vehicle v  in vehicles.Values)
            {
                listVehicles[aux] = v;
                aux++;
            }
            XmlSerializer ser = new XmlSerializer(typeof (Vehicle[]));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, listVehicles);
            writer.Close();

        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)


        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();
            // te pasan una ruta y 
            if (File.Exists(fileFullPath) == true)
            {
                XmlSerializer ser = new XmlSerializer(typeof(Vehicle[]));
                TextReader reader = new StreamReader(fileFullPath );
                Vehicle[] listVehicle=(Vehicle[])ser.Deserialize(reader); //guardo lo que he leido en un array de vehiculos.
                reader.Close();
                
                foreach (Vehicle v in listVehicle )
                {
                    vehicles.Add(v.Enrollment, v);
                }
            }
            return vehicles;
                
        }

    }
}