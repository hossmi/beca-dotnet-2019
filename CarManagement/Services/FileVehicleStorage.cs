using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private DefaultDtoConverter defaultDtoConverter;

        public FileVehicleStorage(string fileFullPath, IVehicleBuilder vehicleBuilder)
            : base(readFromFile(fileFullPath, vehicleBuilder, defaultDtoConverter))
        {
            this.filePath = fileFullPath;
            this.vehicleBuilder = vehicleBuilder;
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            writeToFile(this.filePath, vehicles, this.defaultDtoConverter);
        }

        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder, DefaultDtoConverter defaultDtoConverter )
        {
            EnrollmentEqualityComparer enrollmentEqualityComparer = new EnrollmentEqualityComparer();
            IDictionary<IEnrollment, IVehicle> vehicles= new Dictionary<IEnrollment, IVehicle>(enrollmentEqualityComparer);

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);
                VehicleDto[] vehicleArray=(VehicleDto[])ser.Deserialize(reader);
                reader.Close();

                foreach (VehicleDto vDto in vehicleArray)
                {
                    IVehicle vehicle = defaultDtoConverter.convert(vDto);
                    vehicles.Add(vehicle.Enrollment,vehicle);
                }
            }
            return vehicles;
        }

        private static void writeToFile(string filePath, IEnumerable<IVehicle> vehicles, DefaultDtoConverter dtoConverter)
        {
            VehicleDto[] vehiclesDto = new VehicleDto[vehicles.Count()];
            int aux = 0;
            foreach (IVehicle vehicle in vehicles)
            {
                VehicleDto vehicleDto = dtoConverter.convert(vehicle);
                vehiclesDto[aux] = vehicleDto;
                aux++;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(vehiclesDto.GetType());
            TextWriter vehiclesWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(vehiclesWriter, vehiclesDto);
            vehiclesWriter.Close();
        }
    }
}