using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using System.IO;
using System.Linq;
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
            this.dtoConverter = dtoConverter;
            this.filePath = fileFullPath;
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            writeToFile(this.filePath, vehicles, this.dtoConverter);
        }

        
        private static void writeToFile(string filePath, IEnumerable<Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            VehicleDto[] vehiclesDtoAux = new VehicleDto[vehicles.Count()];
            int contFor = 0;
            foreach (Vehicle vehicle in vehicles)
            {
                VehicleDto vehicleDto = dtoConverter.convert(vehicle);
                vehiclesDtoAux[contFor] = vehicleDto;
                contFor++;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(vehiclesDtoAux.GetType());
            TextWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, vehiclesDtoAux);
            vehiclesWriter.Close();
        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleDto[]));
                TextReader vehiclesReader = new StreamReader(fileFullPath);
                VehicleDto[] vehiclesDtoAux = (VehicleDto[])xmlSerializer.Deserialize(vehiclesReader);
                vehiclesReader.Close();

                foreach (VehicleDto vehicleDto in vehiclesDtoAux)
                {
                    Vehicle vehicle = dtoConverter.convert(vehicleDto);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicles;
        }        
    }
}