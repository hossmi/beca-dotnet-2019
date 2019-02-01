using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CarManagement.Models;
using Newtonsoft.Json;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Core.Models.DTOs;

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
            string jsonText = "";

            foreach (Vehicle vehicle in vehicles)
            {
                VehicleDto savedVehicle = this.dtoConverter.convert(vehicle);
                jsonText += JsonConvert.SerializeObject(savedVehicle);
            }
            System.IO.File.WriteAllText(this.filePath, jsonText);
        }
        private static IDictionary<IEnrollment, IVehicle> load(String filePath, IDtoConverter dtoConverter)
        {
            Dictionary<IEnrollment, IVehicle>  initialVehicles = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(filePath))
            {
                string jsonText = System.IO.File.ReadAllText(filePath);
                List<string> jsonObjects = getIndividualJson(jsonText);

                foreach (string jsonObject in jsonObjects)
                {
                    VehicleDto vehicleDto = JsonConvert.DeserializeObject<VehicleDto>(jsonObject);
                    IVehicle vehicle = dtoConverter.convert(vehicleDto);
                    initialVehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return initialVehicles;
        }
        private static List<string> getIndividualJson(String jsonText)
        {
            int BracketCount = 0;
            List<string> JsonObjects = new List<string>();
            StringBuilder Json = new StringBuilder();

            foreach (char cCursor in jsonText)
            {
                if (cCursor == '{')
                    ++BracketCount;
                else if (cCursor == '}')
                    --BracketCount;
                Json.Append(cCursor);

                if (BracketCount == 0 && cCursor != ' ')
                {
                    JsonObjects.Add(Json.ToString());
                    Json = new StringBuilder();
                }
            }
            return JsonObjects;
        }
    }
}