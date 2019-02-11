﻿using System;
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
            int counter;
            int i;

            counter = 0;
            foreach (IVehicle vehicle in vehicles)
            {
                counter++;
            }

            VehicleDto[] vehiclesArray = new VehicleDto[counter];

            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(this.filePath);

            i = 0;
            foreach (IVehicle vehicle in vehicles)
            {
                vehiclesArray[i] = this.vehicleBuilder.export(vehicle);
                i++;
            }

            ser.Serialize(writer, vehiclesArray);
            writer.Close();
        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder)
        {
            IDictionary<IEnrollment, IVehicle> vehicles = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);

                VehicleDto[] vehiclesArray = (VehicleDto[])ser.Deserialize(reader);
                reader.Close();

                for (int i = 0; i < vehiclesArray.Length; i++)
                {
                    IVehicle vehicle = vehicleBuilder.import(vehiclesArray[i]);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }

            }

            return vehicles;
        }
    }
}