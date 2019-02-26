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
        private List<VehicleDto> vehiclesDtoList;
        private XmlSerializer xmlSerializer;
        private TextWriter textWriter;

        public FileVehicleStorage(string fileFullPath, IVehicleBuilder vehicleBuilder)
            : base(readFromFile(fileFullPath, vehicleBuilder))
        {
            this.filePath += fileFullPath;
            this.vehicleBuilder = vehicleBuilder;
            this.vehiclesDtoList = new List<VehicleDto>();
            this.xmlSerializer = new XmlSerializer(typeof(List<VehicleDto>));
            this.textWriter = new StreamWriter(this.filePath);
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {

            foreach (IVehicle vehicle in vehicles)
            {
                this.vehiclesDtoList.Add(this.vehicleBuilder.export(vehicle));
            }
            this.xmlSerializer.Serialize(this.textWriter, this.vehiclesDtoList);
            this.textWriter.Close();
        }
        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder)
        {
            IDictionary<IEnrollment, IVehicle> vehicleDictionary = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<VehicleDto>));
                TextReader reader = new StreamReader(fileFullPath);
                List<VehicleDto> vehiclesDtoList = (List<VehicleDto>)xmlSerializer.Deserialize(reader);
                reader.Close();
                foreach (VehicleDto vehicleDto in vehiclesDtoList)
                {
                    IVehicle vehicle = vehicleBuilder.import(vehicleDto);
                    vehicleDictionary.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicleDictionary;
        }
    }
}