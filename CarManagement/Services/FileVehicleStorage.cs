﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Models.DTOs;
using CarManagement.Services;

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
            //throw new System.NotImplementedException();
            vehicles.Clear();
            //File.Delete(filePath);
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
            VehicleDto vDTO = DtoConverter.Convert(vehicle);


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
            catch (Exception)
            {
                return false;
            }
        }


    }
}