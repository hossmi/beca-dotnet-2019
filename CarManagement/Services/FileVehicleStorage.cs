using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
       
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly string filePath;


        public FileVehicleStorage(string fileFullPath, IVehicleBuilder vehicleBuilder)
            : base(readFromFile(fileFullPath, vehicleBuilder))
        {
            this.filePath = fileFullPath;
            this.vehicleBuilder = vehicleBuilder;
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            List<IVehicle> vehiclesToExportList = new List<IVehicle>();

            foreach (IVehicle vehicle in vehicles)
            {
                vehiclesToExportList.Add(vehicle);
            }

            IVehicle[] vehiclesToExportArray = vehiclesToExportList.ToArray();

            VehicleDto[] vehiclesDto = new VehicleDto[vehiclesToExportArray.Length];

            for (int i = 0; i < vehiclesDto.Length; i++)
            {
                vehiclesDto[i] = this.vehicleBuilder.export(vehiclesToExportArray[i]);
            }

            using (var writer = new StreamWriter(this.filePath))
            {
                var serializer = new XmlSerializer(typeof(VehicleDto[]));
                serializer.Serialize(writer, vehiclesDto);
                writer.Flush();
            }
        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder)
        {
            {
                IDictionary<IEnrollment, IVehicle> vehicles
                    = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

                if (File.Exists(fileFullPath) == false)
                    return vehicles;

                IVehicle vehicle;
                VehicleDto[] vehiclesDto;

                using (var stream = File.OpenRead(fileFullPath))
                {
                    var serializer = new XmlSerializer(typeof(VehicleDto[]));
                    vehiclesDto = serializer.Deserialize(stream) as VehicleDto[];
                }

                foreach (VehicleDto vehicleDto in vehiclesDto)
                {
                    vehicle = vehicleBuilder.import(vehicleDto);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
                return vehicles;
            }
        }
    }
}