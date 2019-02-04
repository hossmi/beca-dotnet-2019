using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> getVehiclesByPairEnrollments(
            this IEnumerable<IVehicle> vehicles)
        {
            List<IVehicle> pairVehicles = new List<IVehicle>();
            { 
            foreach (IVehicle vehicle in vehicles)
                if (vehicle.Enrollment.Number % 2 == 0) //repasar bien los % (resto)
                    pairVehicles.Add(vehicle);
            }

            return pairVehicles;
        }



        public static IEnumerable<IVehicle> getVehiclesByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            List<IVehicle> serialVehicles = new List<IVehicle>();

            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Serial.ToString() == enrollmentSerial)
                    serialVehicles.Add(vehicle);
            }

            return serialVehicles;
        }
    }
}