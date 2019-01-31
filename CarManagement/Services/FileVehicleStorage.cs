using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
            : base(readFromFile(fileFullPath, dtoConverter))
        {
            this.filePath = fileFullPath;
            this.dtoConverter = dtoConverter;
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            //Asserts.isTrue(File.Exists(this.filePath));
            if (File.Exists(this.filePath)== false)
            {
                File.Create(this.filePath);
            }

            XmlSerializer serialiserXml = new XmlSerializer(typeof(VehicleDto[]));
            
            VehicleDto[] vehiclesDto = new VehicleDto[vehicles.Count()];
            int aux = 0;
            foreach (Vehicle v in vehicles)
            {
                vehiclesDto[aux] = dtoConverter.convert(v);
                aux++;
            }

            TextWriter fileWriter = new StreamWriter(this.filePath);
            serialiserXml.Serialize(fileWriter, vehiclesDto);

            fileWriter.Close();
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization

            VehicleDto[] vehiclesDto = new VehicleDto[vehicles.Count];
            int aux = 0;
            foreach (Vehicle v in vehicles.Values)
            {
                vehiclesDto[aux] = dtoConverter.convert(v);
                aux++;
            }

            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, vehiclesDto);
            writer.Close();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicles= new Dictionary<IEnrollment, Vehicle>();

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);
                VehicleDto[] vehicleArray=(VehicleDto[])ser.Deserialize(reader);
                reader.Close();

                foreach (VehicleDto vDto in vehicleArray)
                {
                    Vehicle vehicle = dtoConverter.convert(vDto);
                    vehicles.Add(vehicle.Enrollment,vehicle);
                }
            }
            return vehicles;
        }
    }
}