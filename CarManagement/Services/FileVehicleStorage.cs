using System;
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

        private IDtoConverter  dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath ,IDtoConverter dtoConverter )
            : base(load(fileFullPath , dtoConverter ))
        {
            this.filePath = fileFullPath;
            this.dtoConverter  = dtoConverter;
        }
        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            VehicleDto[] listVehicles = new VehicleDto[vehicles.Count<IVehicle>()];
            int aux = 0;
            foreach (IVehicle vehicle in vehicles.AsEnumerable())
            {

                listVehicles[aux] = this.dtoConverter.convert(vehicle);
                aux++;
            }
            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(this.filePath);
            ser.Serialize(writer, listVehicles);
            writer.Close();
        }

        private static IDictionary<IEnrollment, IVehicle> load(string fileFullPath,IVehicleBuilder vehicleBuilder)

        {
            EnrollmentEqualityComparer enrollmentEC = new EnrollmentEqualityComparer();
            IDictionary<IEnrollment, IVehicle> vehicles = new Dictionary<IEnrollment, IVehicle>(enrollmentEC);
            if (File.Exists(fileFullPath) == true)
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);
                VehicleDto[] listVehicle = (VehicleDto[])ser.Deserialize(reader);
                reader.Close();

                foreach (VehicleDto ToIvehcile in listVehicle)
                {

                    IVehicle vehicle = dtoConverter.convert(ToIvehcile);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }
            return vehicles;

        }

    }
}



