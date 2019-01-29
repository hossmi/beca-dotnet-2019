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
        private readonly string filePath;

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public FileVehicleStorage(string fileFullPath)
        {
            this.filePath = fileFullPath;
            this.vehicles = readFromFile(fileFullPath);
        }



        public void clear()
        {
            this.vehicles.Clear();

            writeToFile(this.filePath, this.vehicles);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle listedVehicle;

            bool vehicleIsListed = vehicles.TryGetValue(enrollment, out listedVehicle);
            Asserts.isTrue(vehicleIsListed);

            return listedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment));
            vehicles.Add(vehicle.Enrollment, vehicle);

            writeToFile(this.filePath, this.vehicles);
        }


        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles)
        {
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                VehicleDto serializableVehicle = new VehicleDto();


                var serializer = new XmlSerializer(serializableVehicle.GetType());
                serializer.Serialize(writer, serializableVehicle);
                writer.Flush();
            }
        }

        private static VehicleDto DtoMap(IDictionary<IEnrollment, Vehicle> vehicles)
        {
            VehicleDto vehicleDto = new VehicleDto();

            foreach (Vehicle vehicle in vehicles.Values)
            {
                vehicleDto.Color = vehicle.Color;
                vehicleDto.Engine.HorsePower = vehicle.Engine.HorsePower;
                vehicleDto.Engine.IsStarted = vehicle.Engine.IsStarted;

                foreach (Door door in vehicle.Doors)
                {

                }

            }


            return vehicleDto;
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            using (var stream = System.IO.File.OpenRead(fileFullPath))
            {
                var serializer = new XmlSerializer(typeof(VehicleDto));
                return serializer.Deserialize(stream) as IDictionary<IEnrollment, Vehicle>;
            }
        }
    }
}