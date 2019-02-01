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
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter) 
            : base(load(fileFullPath, dtoConverter))
        {
            this.filePath = fileFullPath;
            this.dtoConverter = dtoConverter;
            
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            List<VehicleDto> vehiclesDtoList = new List<VehicleDto>();

            foreach (IVehicle v in vehicles)
            {
                vehiclesDtoList.Add(this.dtoConverter.convert(v));
            }
            XmlSerializer ser = new XmlSerializer(typeof(List<VehicleDto>));
            TextWriter writer = new StreamWriter(this.filePath);
            ser.Serialize(writer, vehiclesDtoList);
            writer.Close();
        }

        private static IDictionary<IEnrollment, IVehicle> load(string filePath, IDtoConverter dtoConverter)
        {
            IDictionary<IEnrollment, IVehicle> vehicleDictionary = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(filePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<VehicleDto>));
                TextReader reader = new StreamReader(filePath);
                List<VehicleDto> vehiclesDtoList = (List<VehicleDto>)ser.Deserialize(reader);
                reader.Close();
                foreach (VehicleDto vDto in vehiclesDtoList)
                {
                    IVehicle vehicle = dtoConverter.convert(vDto);
                    vehicleDictionary.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicleDictionary;
        }
    }
}


    