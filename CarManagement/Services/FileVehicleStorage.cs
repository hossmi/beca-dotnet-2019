using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CarManagement.Models;
using CarManagement.Models.DTOs;
using Newtonsoft.Json;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
        private readonly IDtoConverter dtoConverter;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IDtoConverter dtoConverter)
        {
            this.filePath = fileFullPath;
            this.dtoConverter = dtoConverter;
        }
        
        protected override IDictionary<IEnrollment, Vehicle> load()
        {
            IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(this.filePath))
            {
                string jsonText = System.IO.File.ReadAllText(this.filePath);
                List<string> jsonObjects = getIndividualJson(jsonText);

                foreach (string jsonObject in jsonObjects)
                {
                    VehicleDto vehicleDto = JsonConvert.DeserializeObject<VehicleDto>(jsonObject);
                    Vehicle vehicle = this.dtoConverter.convert(vehicleDto);
                    vehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return vehicles;
        }
        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            string jsonText = "";

            foreach (Vehicle vehicle in vehicles)
            {
                VehicleDto savedVehicle = this.dtoConverter.convert(vehicle);
                jsonText += JsonConvert.SerializeObject(savedVehicle);
            }
            System.IO.File.WriteAllText(this.filePath, jsonText);
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