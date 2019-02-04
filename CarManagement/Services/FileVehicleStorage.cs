using System;
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
            this.vehicleBuilder = vehicleBuilder;
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder)
        {
            throw new NotImplementedException();
        }
    }
}