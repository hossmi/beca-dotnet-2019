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
            this.vehicles = readFromFile(fileFullPath, this.dtoConverter);
            this.dtoConverter = dtoConverter;
        }

        public int Count => this.vehicles.Count;

        public void clear()
        {
            this.vehicles.Clear();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            bool exists = vehicles.TryGetValue(enrollment, out Vehicle vehicle);
            Asserts.isTrue(exists);

            return vehicle;
        }

        public void set(Vehicle vehicle)
        {
            bool exists = vehicles.ContainsKey(vehicle.Enrollment);
            Asserts.isFalse(exists);
            this.vehicles.Add(vehicle.Enrollment, vehicle);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization
            
            VehicleDto[] vehiclesDtoAux = new VehicleDto[vehicles.Count];
            int contFor = 0;
            foreach (Vehicle vehicle in vehicles.Values)
            {
                VehicleDto vehicleDto = dtoConverter.convert(vehicle);
                vehiclesDtoAux[contFor] = vehicleDto;
                contFor++;
            }

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleDto[]));
            XmlSerializer xmlSerializer = new XmlSerializer(vehiclesDtoAux.GetType());
            TextWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, xmlSerializer);
            vehiclesWriter.Close();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(equalityComparer);

            if (File.Exists(fileFullPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleDto[]));
                TextReader vehiclesReader = new StreamReader(fileFullPath);
                VehicleDto[] vehiclesDtoAux = (VehicleDto[]) xmlSerializer.Deserialize(vehiclesReader);
                vehiclesReader.Close();
                
                foreach (VehicleDto vehicleDto in vehiclesDtoAux)
                {
                    Vehicle vehicle = dtoConverter.convert(vehicleDto);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }
            else
            {
                return vehicles;
            }

            return vehicles;
        }
                
        

    }
}