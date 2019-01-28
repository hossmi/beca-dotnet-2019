<<<<<<< HEAD
﻿using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;
=======
﻿using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Models;
>>>>>>> develop

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
<<<<<<< HEAD
        const string STORAGEFILE = ".\\storage\\vehicles";
        private IDictionary<IEnrollment, Vehicle> vehicles;

        public FileVehicleStorage()
        {
            this.vehicles = new Dictionary<IEnrollment, Vehicle>();
        }

        public int Count { get => this.vehicles.Count; }
=======
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly string filePath;

        public int Count { get; }
>>>>>>> develop

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
<<<<<<< HEAD
            this.vehicles.Clear();
=======
            throw new System.NotImplementedException();
            writeToFile(this.filePath);
>>>>>>> develop
        }

        public Vehicle get(IEnrollment enrollment)
        {
            bool hasVehicle = this.vehicles.TryGetValue(defaultEnrollment, out Vehicle returnedVehicle);

            Asserts.isTrue(hasVehicle, "El vehículo no está almacenado");

            return returnedVehicle;
        }

        public void set(Vehicle vehicle)
        {
<<<<<<< HEAD
            this.vehicles.Add(motoVehicle.Enrollment, motoVehicle);
=======
            throw new System.NotImplementedException();
            writeToFile(this.filePath);
        }

        private void writeToFile(string filePath)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization
            throw new NotImplementedException();
        }

        private IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath)
        {
            throw new NotImplementedException();
>>>>>>> develop
        }

    }
}