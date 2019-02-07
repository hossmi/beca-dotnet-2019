using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CarManagement.Models;
using Newtonsoft.Json;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Core.Models.DTOs;
using System.Linq;

namespace CarManagement.Services
{
    public class FileVehicleStorage : AbstractVehicleStorage
    {
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly string filePath;

        public FileVehicleStorage(string fileFullPath, IVehicleBuilder vehicleBuilder)
            : base(load(fileFullPath, vehicleBuilder))
        {
            this.filePath = fileFullPath;
            this.vehicleBuilder = vehicleBuilder;
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
            string jsonText = "";

            foreach (Vehicle vehicle in vehicles)
            {
                VehicleDto savedVehicle = this.vehicleBuilder.export(vehicle);
                jsonText += JsonConvert.SerializeObject(savedVehicle);
            }
            System.IO.File.WriteAllText(this.filePath, jsonText);
        }
        private static IDictionary<IEnrollment, IVehicle> load(String filePath, IVehicleBuilder vehicleBuilder)
        {
            Dictionary<IEnrollment, IVehicle>  initialVehicles = new Dictionary<IEnrollment, IVehicle>(new EnrollmentEqualityComparer());

            if (File.Exists(filePath))
            {
                string jsonText = System.IO.File.ReadAllText(filePath);

                foreach (string jsonObject in getIndividualJson(jsonText))
                {
                    VehicleDto vehicleDto = JsonConvert.DeserializeObject<VehicleDto>(jsonObject);
                    IVehicle vehicle = vehicleBuilder.import(vehicleDto);
                    initialVehicles.Add(vehicle.Enrollment, vehicle);
                }
            }

            return initialVehicles;
        }
        private static IEnumerable<string> getIndividualJson(String jsonText)
        {
            int BracketCount = 0;
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
                    yield return Json.ToString();
                    Json = new StringBuilder();
                }
            }
        }
    }
}