using System;
using System.Collections;
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

            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Number % 2 == 0)
                    pairVehicles.Add(vehicle);
            }

            return pairVehicles;
        }

        public static IEnumerable<IVehicle> getVehiclesByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            List<IVehicle> serialVehicles = new List<IVehicle>();
            IEnumerator<IVehicle> enumVehicle = vehicles.GetEnumerator();

            while (enumVehicle.MoveNext())
            {
                if (enumVehicle.Current.Enrollment.Serial == enrollmentSerial)
                    serialVehicles.Add(enumVehicle.Current);
            }

            return serialVehicles;
        }

        public static IEnumerable<IEngine> getEngines(this IEnumerable<IVehicle> vehicles)
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