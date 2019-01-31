using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private readonly IDictionary<IEnrollment, Vehicle> vehicles;
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.filePath = fileFullPath;
            this.dtoConverter = dtoConverter;
            this.vehicles = readFromFile(fileFullPath, this.dtoConverter);
         
        }

        public int Count
        {
            get
            {
                return this.vehicles.Count;
            }
        }

        public void clear()
        {
            this.vehicles.Clear();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);

        }

        public Vehicle get(IEnrollment enrollment) //me pasan matricula
        {
            Vehicle vehicle; // declaro un objeto veiculo
            bool hasVehicle = this.vehicles.TryGetValue(enrollment, out vehicle); // miro si con esa matricula tengo 
                                                                                  // datos del vehiculo(En teoria true)
            Asserts.isTrue(hasVehicle);  // BOOOOM peta porque hasvehiculo es false entonces me dice que no hay vehiclo.                                        
            return vehicle;
        }

        public void set(Vehicle vehicle) // me pasan un vehicle
        {
            Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment)); //pregunto si tengo una matricula registrada igual
            this.vehicles.Add(vehicle.Enrollment, vehicle); // guardo vehiculo con matricula en diccionario
            writeToFile(this.filePath, this.vehicles, this.dtoConverter); // escribo el xml con los datos del vehiclo
            
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment, Vehicle> vehicles, IDtoConverter dtoConverter)
        {
            //https://docs.microsoft.com/es-es/dotnet/standard/serialization/examples-of-xml-serialization



            VehicleDto[] listVehicles = new VehicleDto[vehicles.Count];
            int aux = 0;
            foreach (Vehicle vehicle in vehicles.Values)
            {

                listVehicles[aux] = dtoConverter.convert(vehicle);
                aux++;
            }
            XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, listVehicles);
            writer.Close();

        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter dtoConverter)

        
        {
            EnrollmentEqualityComparer enrollmentEC = new EnrollmentEqualityComparer();
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(enrollmentEC);
            // te pasan una ruta y 
            if (File.Exists(fileFullPath) == true)
            {
                XmlSerializer ser = new XmlSerializer(typeof(VehicleDto[]));
                TextReader reader = new StreamReader(fileFullPath);
                VehicleDto[] listVehicle = (VehicleDto[])ser.Deserialize(reader); //guardo lo que he leido en un array de vehiculos.
                reader.Close();

                foreach (VehicleDto v in listVehicle)
                {

                    Vehicle vehicle = dtoConverter.convert(v);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }
           
            return vehicles;



        }
    }
}