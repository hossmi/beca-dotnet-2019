using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Models;
using System.Xml.Serialization;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter) : base(load(fileFullPath, dtoConverter))
        {
            this.filePath = fileFullPath;
            this.dtoConverter = dtoConverter;
            this.vehicles = readFromFile(fileFullPath, this.dtoConverter);
        }

        private static IDictionary<IEnrollment, Vehicle> load(string fileFullPath, IDtoConverter dtoConverter)
        {
            return readFromFile(fileFullPath, dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            VehicleDto[] vehiclesArray = new VehicleDto[vehicles.Count];
            int i = 0;

            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(filePath);

            foreach (Vehicle vehicle in vehicles.Values)
            {
                vehiclesArray[i] = dtoConverter.convert(vehicle);
                i++;
            }

            ser.Serialize(writer, vehiclesArray);
            writer.Close();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);
                
                VehicleDto[] vehiclesArray = (VehicleDto[])ser.Deserialize(reader);
                reader.Close();

                for (int i = 0; i < vehiclesArray.Length; i++)
                {
                    Vehicle vehicle = dtoConverter.convert(vehiclesArray[i]);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }

            }

            return vehicles;
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            int counter;
            int i;

            counter = 0;
            foreach (Vehicle vehicle in vehicles)
            {
                counter++;
            }

            VehicleDto[] vehiclesArray = new VehicleDto[counter];
            
            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(this.filePath);

            i = 0;
            foreach (Vehicle vehicle in vehicles)
            {
                vehiclesArray[i] = this.dtoConverter.convert(vehicle);
                i++;
            }

            ser.Serialize(writer, vehiclesArray);
            writer.Close();
        }
    }
}