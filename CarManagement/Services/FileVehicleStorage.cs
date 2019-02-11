using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Models;
using System.Xml.Serialization;
using CarManagement.Models.DTOs;
using System.Linq;

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

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            VehicleDto[] vehiclesArray = new VehicleDto[vehicles.Count()];
            int i = 0;

            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(this.filePath);

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