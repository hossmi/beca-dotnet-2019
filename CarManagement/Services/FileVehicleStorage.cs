using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CarManagement.Builders;
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

            if (File.Exists(fileFullPath))
                this.vehicles = readFromFile(fileFullPath);
            else
                this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }

        public void clear()
        {
            //throw new System.NotImplementedException();
            vehicles.Clear();
            File.Delete(filePath);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            //throw new System.NotImplementedException();
            Vehicle v;
            Boolean vehicleFound;
            vehicleFound = this.vehicles.TryGetValue(enrollment,out v);
            Asserts.isTrue(vehicleFound, "Could not find vehicle.");
            return v;
        }

        public void set(Vehicle vehicle)
        {
            VehicleDto vDTO;


        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization

            //foreach()

            //throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            IDictionary<IEnrollment, Vehicle> vehicleDictionary = new Dictionary<IEnrollment, Vehicle>();
            XmlSerializer serializer = new XmlSerializer(typeof(Vehicle));

            FileStream fs = new FileStream(fileFullPath, FileMode.Open);

            Vehicle vehicle;

            vehicle = (Vehicle)serializer.Deserialize(fs);

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
            catch (Exception ex)
            {
                return false;
            }
        }

        public VehicleDto ConvertToVehicleDto(Vehicle v)
        {
            VehicleDto vDto = new VehicleDto(v);

            return vDto;
        }



    }
}