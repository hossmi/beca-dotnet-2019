using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

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

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            throw new NotImplementedException();
        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            throw new NotImplementedException();
        }
    }
}