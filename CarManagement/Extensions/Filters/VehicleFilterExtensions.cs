using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using System.Linq;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> getVehiclesByPairEnrollments(this IEnumerable<IVehicle> vehicles)
        {
            List<IVehicle> vehiclesEnrollmentPair = new List<IVehicle>();
            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Number % 2 == 0)
                {
                    vehiclesEnrollmentPair.Add(vehicle);
                }
            }
            return vehiclesEnrollmentPair;
        }

        public static IEnumerable<IVehicle> getVehiclesByEnrollmentsSerial(this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            List<IVehicle> vehiclesSerial = new List<IVehicle>();

            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Serial == enrollmentSerial)
                {
                    vehiclesSerial.Add(vehicle);
                }
            }
                
            return vehicles;
        }
    }
}