﻿using System;
using System.Collections.Generic;
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
            this.dtoConverter = dtoConverter;

        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            List<VehicleDto> vehiclesDtoList = new List<VehicleDto>();

            foreach (Vehicle v in vehicles)
            {
                vehiclesDtoList.Add(this.dtoConverter.convert(v));
            }
            XmlSerializer ser = new XmlSerializer(typeof(List<VehicleDto>));
            TextWriter writer = new StreamWriter(this.filePath);
            ser.Serialize(writer, vehiclesDtoList);
            writer.Close();
        }

        private static IDictionary<IEnrollment, Vehicle> load(string filePath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicleDictionary = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(filePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<VehicleDto>));
                TextReader reader = new StreamReader(filePath);
                List<VehicleDto> vehiclesDtoList = (List<VehicleDto>)ser.Deserialize(reader);
                reader.Close();
                foreach (VehicleDto vDto in vehiclesDtoList)
                {
                    Vehicle vehicle = dtoConverter.convert(vDto);
                    vehicleDictionary.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicleDictionary;
        }
    }
}