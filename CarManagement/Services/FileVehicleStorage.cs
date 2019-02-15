using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
            : base(readFromFile(fileFullPath, dtoConverter))
        {
            this.filePath = fileFullPath;
            this.dtoConverter = dtoConverter;
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
            return this.vehicles[enrollment];
        }

        public void set(Vehicle vehicle)
        {
            this.vehicles[vehicle.Enrollment] = vehicle;
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            VehicleDto[] vehicleArray = new VehicleDto[vehicles.Count];

            int cont = 0;


            foreach (Vehicle vehicle in vehicles.Values)
            {
                VehicleDto vehicleDto = dtoConverter.convert(vehicle);
                vehicleArray[cont] = vehicleDto;
                cont++;
            }

            XmlSerializer x = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(filePath);
            x.Serialize(writer, vehicleArray);
            writer.Close();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Vehicle[]));
                TextReader reader = new StreamReader(fileFullPath);
                Vehicle[] vehicleArray = (Vehicle[])ser.Deserialize(reader);
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