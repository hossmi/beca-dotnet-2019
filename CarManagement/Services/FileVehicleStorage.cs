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

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.dtoConverter = dtoConverter;
            this.filePath = fileFullPath;

            if (File.Exists(fileFullPath))
                this.vehicles = readFromFile(fileFullPath, dtoConverter);
            else
                vehicles = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());
        }

        public void clear()
        {
            this.vehicles.Clear();

            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
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

            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,
            Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            VehicleDto[] vehiclesDto = new VehicleDto[vehicles.Count];
            int i = 0;

            foreach (Vehicle vehicle in vehicles.Values)
            {
                vehiclesDto[i] = dtoConverter.convert(vehicle);
                i++;
            }

            using (var writer = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(VehicleDto[]));
                serializer.Serialize(writer, vehiclesDto);
                writer.Flush();
            }
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath,
            IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());
            Vehicle vehicle;
            VehicleDto[] vehiclesDto;

            using (var stream = File.OpenRead(fileFullPath))
            {
                var serializer = new XmlSerializer(typeof(VehicleDto[]));
                vehiclesDto = serializer.Deserialize(stream) as VehicleDto[];
            }

            foreach (VehicleDto vehicleDto in vehiclesDto)
            {
                vehicle = dtoConverter.convert(vehicleDto);
                vehicles.Add(vehicle.Enrollment, vehicle);
            }
            return vehicles;
        }
    }
}