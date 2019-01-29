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
        private List<Vehicle> vehicle = new List<Vehicle>();

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
                return vehicle.Count;
            }
        }

        public void clear()
        {
            vehicle.Clear();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle T2 = new Vehicle();
            bool vehicleFound = false;

            foreach (Vehicle T in vehicle)
            {
                if (enrollment == T.Enrollment)
                {
                    vehicleFound = true;
                    T2 = T;
                }
            }
            Asserts.isTrue(vehicleFound);
            return T2;
        }

        public void set(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            /*Vehicle[] vehicle = new Vehicle[this.vehicle.Count];
            int temp = 0;
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