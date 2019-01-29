using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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
            this.filePath = fileFullPath;
            this.vehicles = readFromFile(fileFullPath, this.dtoConverter);
            this.dtoConverter = dtoConverter;
        }

        public void clear()
        {
            vehicles.Clear();
            //File.Delete(filePath);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            Vehicle v;
            Boolean vehicleFound = false;

            vehicleFound = vehicles.TryGetValue(enrollment, out v);

            Asserts.isTrue(vehicleFound, "Could not find vehicle.");
            return v;
        }

        public void set(Vehicle vehicle)
        {
            vehicles.Add(vehicle.Enrollment, vehicle);
            writeToFile(this.filePath, vehicles, dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization

            VehicleDto[] vehiclesDtoArray = new VehicleDto[vehicles.Count];

            int i = 0;
            foreach (Vehicle v in vehicles.Values)
            {
                vehiclesDtoArray[i] = dtoConverter.convert(v);
                i++;
            }

            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, vehiclesDtoArray);
            writer.Close();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicleDictionary = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);
                VehicleDto[] vehiclesDtoArray = (VehicleDto[])ser.Deserialize(reader);
                reader.Close();

                foreach (VehicleDto vDto in vehiclesDtoArray)
                {
                    //vehicleDictionary.Add(v.Enrollment, v);
                    Vehicle vehicle = dtoConverter.convert(vDto);
                    vehicleDictionary.Add(vehicle.Enrollment, vehicle);

                }
            }

            return vehicleDictionary;
        }


    }
}


    