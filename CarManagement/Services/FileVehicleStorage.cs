using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Models;
using System.Xml.Serialization;
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

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }
        public void clear()
        {
            this.vehicles.Clear();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle returnedVehicle;

            bool exists = this.vehicles.TryGetValue(enrollment, out returnedVehicle);

            Asserts.isTrue(exists);

            return returnedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
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
            EnrollmentEqualityComparer enrollmentComparer = new EnrollmentEqualityComparer();
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(enrollmentComparer);

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

    }
}