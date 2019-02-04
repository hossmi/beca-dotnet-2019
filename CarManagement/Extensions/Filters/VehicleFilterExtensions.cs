using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> getVehiclesByPairEnrollments( //probar mas tarde con enumerator
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

        public static IEnumerable<IEngine> getEngines(
            this IEnumerable<IVehicle> vehicles)
        {
            List<IEngine> pairVehicles = new List<IEngine>();

            foreach (IVehicle vehicle in vehicles)
            {
                pairVehicles.Add(vehicle.Engine);
            }

            return pairVehicles;
        }
    }
}