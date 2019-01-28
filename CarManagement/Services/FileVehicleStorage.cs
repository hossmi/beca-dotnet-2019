using CarManagement.Models;
using CarManagement.Models.DTOs;
using System.Collections.Generic;
using System.IO;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly string filePath;
        public int Count { get; }


        public FileVehicleStorage(string FileFullPath)
        {
            this.filePath = FileFullPath;
            if (File.Exists(FileFullPath))
            {

            }
        }


        public void clear()
        {
            throw new System.NotImplementedException();
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            throw new System.NotImplementedException();
        }

        public void set(Vehicle motoVehicle)
        {
            throw new System.NotImplementedException();
        }
    }
}