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
            this.vehicles = readFromFile(fileFullPath);
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
            vehicleFound = this.vehicles.TryGetValue(enrollment, out v);
            Asserts.isTrue(vehicleFound, "Could not find vehicle.");
            return v;
        }

        public void set(Vehicle vehicle)
        {
            vehicles.Add(vehicle.Enrollment, vehicle);
            writeToFile(this.filePath, vehicles);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization

            VehicleDto[] vehiclesDtoArray = new VehicleDto[vehicles.Count];

            int i = 0;
            foreach (Vehicle v in vehicles.Values)
            {
                VehicleDto vehicleDto = dtoConverter.convert(v);
                vehiclesDtoArray[i] = vehicleDto;
                i++;
            }

            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, vehiclesDtoArray);
            writer.Close();


            //XmlSerializer serializer = new XmlSerializer(typeof(Vehicle[]));
            //FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);

            //foreach (KeyValuePair<IEnrollment, Vehicle> item in vehicles)
            //{
            //    //Vehicle vehicle = item.Value;

            //    VehicleDto vehicleDto = dtoConverter.convert(item.Value);

            //    serializer.Serialize(fs, vehicleDto);

            //}

            //fs.Close();

        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            IDictionary<IEnrollment, Vehicle> vehicleDictionary = new Dictionary<IEnrollment, Vehicle>();

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
            //else
            //{
            //    //FileStream fs = new FileStream(fileFullPath, FileMode.CreateNew);
            //    ////File.Create(fileFullPath);
            //    //fs.Flush();
            //    //fs.Close();
            //    return new Dictionary<IEnrollment, Vehicle>();
            //}

            return vehicleDictionary;
        }

        private static bool Serialize<T>(T value, ref string serializeXml)
        {
            if (value == null)
            {
                return false;
            }
            try
            {
                XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
                StringWriter stringWriter = new StringWriter();
                XmlWriter writer = XmlWriter.Create(stringWriter);

                xmlserializer.Serialize(writer, value);

                serializeXml = stringWriter.ToString();

                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}