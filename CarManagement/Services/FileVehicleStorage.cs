using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CarManagement.Models;
using CarManagement.Models.DTOs;
using Newtonsoft.Json;

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
            this.vehicles = readFromFile(fileFullPath, dtoConverter);
            this.dtoConverter = dtoConverter;
        }

        public int Count { get => vehicles.Count; }

        public void clear()
        {
            vehicles.Clear();
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        public Vehicle get(IEnrollment enrollment)
        {
            bool hasVehicle = this.vehicles.TryGetValue(enrollment, out Vehicle returnedVehicle);

            Asserts.isTrue(hasVehicle, "El vehículo no está en el diccionario");

            return returnedVehicle;
        }

        public void set(Vehicle vehicle)
        {
            vehicles.Add(vehicle.Enrollment, vehicle);
            writeToFile(this.filePath, this.vehicles, this.dtoConverter);
        }

        private static void writeToFile(string filePath, IDictionary<IEnrollment,Vehicle> vehicles, IDtoConverter converter)
        {
            string jsonText = "";

            foreach (KeyValuePair<IEnrollment, Vehicle> entry in vehicles)
            {
                VehicleDto savedVehicle = converter.convert(entry.Value);
                jsonText += JsonConvert.SerializeObject(savedVehicle);
            }
            System.IO.File.WriteAllText(filePath, jsonText);
        }

        private static IDictionary<IEnrollment, Vehicle> readFromFile(string fileFullPath, IDtoConverter converter)
        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(fileFullPath))
            {
                string jsonText = System.IO.File.ReadAllText(fileFullPath);
                List<string> jsonObjects = getIndividualJson(jsonText);

                foreach (string jsonObject in jsonObjects)
                {
                    VehicleDto vehicleDto = JsonConvert.DeserializeObject<VehicleDto>(jsonObject);
                    Vehicle vehicle = converter.convert(vehicleDto);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicles;
        }

        private static List<string> getIndividualJson(String jsonText)
        {
            int BracketCount = 0;
            List<string> JsonItems = new List<string>();
            StringBuilder Json = new StringBuilder();

            foreach (char cIndex in jsonText)
            {
                if (cIndex == '{')
                    ++BracketCount;
                else if (cIndex == '}')
                    --BracketCount;
                Json.Append(cIndex);

                if (BracketCount == 0 && cIndex != ' ')
                {
                    JsonItems.Add(Json.ToString());
                    Json = new StringBuilder();
                }
            }
            return JsonItems;
        }

    }
}