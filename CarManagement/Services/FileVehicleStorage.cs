using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

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

        protected override void save(IEnumerable<IVehicle> vehicles)
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

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
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
}