using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.dtoConverter = dtoConverter;
            this.filePath = fileFullPath;
        }

        protected override IDictionary<IEnrollment, Vehicle> load()
        {
            return readFromFile(this.filePath, this.dtoConverter);
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            /*foreach (Vehicle vehicle in vehicles)
            {

            }*/

            //writeToFile(this.filePath, vehicles, this.dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            VehicleDto[] vehiclesDtoAux = new VehicleDto[vehicles.Count];
            int contFor = 0;
            foreach (Vehicle vehicle in vehicles.Values)
            {
                VehicleDto vehicleDto = dtoConverter.convert(vehicle);
                vehiclesDtoAux[contFor] = vehicleDto;
                contFor++;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(vehiclesDtoAux.GetType());
            TextWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, vehiclesDtoAux);
            vehiclesWriter.Close();
        }
                
        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleDto[]));
                TextReader vehiclesReader = new StreamReader(fileFullPath);
                VehicleDto[] vehiclesDtoAux = (VehicleDto[])xmlSerializer.Deserialize(vehiclesReader);
                vehiclesReader.Close();

                foreach (VehicleDto vehicleDto in vehiclesDtoAux)
                {
                    Vehicle vehicle = dtoConverter.convert(vehicleDto);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicles;
        }

        /*public int Count => this.vehicles.Count;

        public void clear()
        {
            this.vehicles.Clear();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            bool exists = this.vehicles.TryGetValue(enrollment, out Vehicle vehicle);
            Asserts.isTrue(exists);

            return vehicle;
        }

        public void set(Vehicle vehicle)
        {
            bool exists = this.vehicles.ContainsKey(vehicle.Enrollment);
            Asserts.isFalse(exists);
            this.vehicles.Add(vehicle.Enrollment, vehicle);
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        */


    }
}