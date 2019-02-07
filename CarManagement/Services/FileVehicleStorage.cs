using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Linq;

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
            
            List<VehicleDto> vehiclesDtoList = new List<VehicleDto>();

            vehiclesDtoList = vehicles
                .Select(vehicle => this.vehicleBuilder.export(vehicle))
                .ToList();

            //foreach (IVehicle v in vehicles)
            //{
            //    vehiclesDtoList.Add(this.vehicleBuilder.export(v));
            //}
            XmlSerializer ser = new XmlSerializer(typeof(List<VehicleDto>));
            TextWriter writer = new StreamWriter(this.filePath);
            ser.Serialize(writer, vehiclesDtoList);
            writer.Close();
        }


        private static IDictionary<IEnrollment, IVehicle> readFromFile(string fileFullPath, IVehicleBuilder vehicleBuilder)
        {
            IDictionary<IEnrollment, IVehicle> vehicleDictionary = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<VehicleDto>));
                TextReader reader = new StreamReader(fileFullPath);
                List<VehicleDto> vehiclesDtoList = (List<VehicleDto>)ser.Deserialize(reader);
                reader.Close();

                vehicleDictionary = vehiclesDtoList
                    .Select(vDto => vehicleBuilder.import(vDto))
                    .ToDictionary(v => v.Enrollment, v => v,new EnrollmentEqualityComparer());

                //foreach (VehicleDto vDto in vehiclesDtoList)
                //{
                //    IVehicle vehicle = vehicleBuilder.import(vDto);
                //    vehicleDictionary.Add(vehicle.Enrollment, vehicle);
                //}
            }

            return vehicleDictionary;
        }
    }
}


