using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CarManagement.Core;
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
            save(vehicles, this.filePath, this.vehicleBuilder);
        }

        private static void save(IEnumerable<IVehicle> vehicles, string filePath, IVehicleBuilder vehicleBuilder)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VehicleDto[]));

            using (TextWriter writer = new StreamWriter(filePath))
            {
                VehicleDto[] dtos = vehicles
                    .Select(vehicleBuilder.export)
                    .ToArray();

                serializer.Serialize(writer, dtos);
                writer.Close();
            }

        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder)
        {
            VehicleDto[] dtos;

            if (File.Exists(fileFullPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(VehicleDto[]));
                using (TextReader reader = new StreamReader(fileFullPath))
                {
                    dtos = (VehicleDto[])serializer.Deserialize(reader);
                    reader.Close();
                }
            }
            else
                dtos = new VehicleDto[] { };

            return dtos
                .Select(vehicleBuilder.import)
                .ToDictionary(v => v.Enrollment, new EnrollmentEqualityComparer());
        }
    }
}