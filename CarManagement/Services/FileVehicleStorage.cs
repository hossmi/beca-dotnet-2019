using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
            : base(load(fileFullPath, dtoConverter))
        {
            this.filePath = fileFullPath;
            this.dtoConverter = dtoConverter;
            this.vehicles = load(fileFullPath, this.dtoConverter);
        }

        //public int Count
        //{
        //    get
        //    {
        //        return this.vehicles.Count;
        //    }
        //}

        //public void clear()
        //{
        //    this.vehicles.Clear();
        //    writeToFile(this.filePath, this.vehicles, this.dtoConverter);

        //}

        //public Vehicle get(IEnrollment enrollment) 
        //{
        //    Vehicle vehicle; 
        //    bool hasVehicle = this.vehicles.TryGetValue(enrollment, out vehicle); 

        //    Asserts.isTrue(hasVehicle);                                       
        //    return vehicle;
        //}

        //public void set(Vehicle vehicle) 
        ////{
        ////    Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment)); 
        ////    this.vehicles.Add(vehicle.Enrollment, vehicle); 
        ////    writeToFile(this.filePath, this.vehicles, this.dtoConverter); 

        ////}


        private static IDictionary<IEnrollment, Vehicle> load(string fileFullPath, IDtoConverter dtoConverter)


        {
            EnrollmentEqualityComparer enrollmentEC = new EnrollmentEqualityComparer();
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(enrollmentEC);
            // te pasan una ruta y 
            if (File.Exists(fileFullPath) == true)
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);
                VehicleDto[] listVehicle = (VehicleDto[])ser.Deserialize(reader); 
                reader.Close();

                foreach (VehicleDto v in listVehicle)
                {

                    Vehicle vehicle = dtoConverter.convert(v);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicles;



        }

        protected override void save(IEnumerable<Vehicle> vehicles )
        {

            VehicleDto[] listVehicles = new VehicleDto[vehicles.Count<Vehicle>()];
            int aux = 0;
            foreach (Vehicle vehicle in vehicles.AsEnumerable())
            {

                listVehicles[aux] = this.dtoConverter.convert(vehicle);
                aux++;
            }
            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(this.filePath);
            ser.Serialize(writer, listVehicles);
            writer.Close();
            vehicles = (IEnumerable<Vehicle>)listVehicles.Clone();

        }
    }
}