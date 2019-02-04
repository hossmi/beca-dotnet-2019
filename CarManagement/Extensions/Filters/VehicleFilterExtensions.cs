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
            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Number % 2 == 0)
                    yield return vehicle;
            }
        }

        public static IEnumerable<IVehicle> getVehiclesByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            IEnumerator<IVehicle> enumVehicle = vehicles.GetEnumerator();

            while (enumVehicle.MoveNext())
            {
                if (enumVehicle.Current.Enrollment.Serial == enrollmentSerial)
                    yield return enumVehicle.Current;
            }
        }

        public static IEnumerable<IEngine> getEngines(this IEnumerable<IVehicle> vehicles)
        {
            foreach (IVehicle vehicle in vehicles)
            {
                    yield return vehicle.Engine;
            }
        }
    }
}