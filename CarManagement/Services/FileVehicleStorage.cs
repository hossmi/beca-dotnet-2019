using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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
            writeToFile(this.filePath, vehicles, this.vehicleBuilder);
        }

        
        private static void writeToFile(string filePath, IEnumerable<IVehicle> vehicles, IVehicleBuilder vehicleBuilder)
        {
            VehicleDto[] vehiclesDtoAux = new VehicleDto[vehicles.Count()];
            int contFor = 0;
            foreach (IVehicle vehicle in vehicles)
            {
                VehicleDto vehicleDto = vehicleBuilder.export(vehicle);
                vehiclesDtoAux[contFor] = vehicleDto;
                contFor++;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(vehiclesDtoAux.GetType());
            TextWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, vehiclesDtoAux);
            vehiclesWriter.Close();
        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder)
        {
            IDictionary<IEnrollment, IVehicle> vehicles = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleDto[]));
                TextReader vehiclesReader = new StreamReader(fileFullPath);
                VehicleDto[] vehiclesDtoAux = (VehicleDto[])xmlSerializer.Deserialize(vehiclesReader);
                vehiclesReader.Close();

                foreach (VehicleDto vehicleDto in vehiclesDtoAux)
                {
                    IVehicle vehicle = vehicleBuilder.import(vehicleDto);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicles;
        }        
    }
}