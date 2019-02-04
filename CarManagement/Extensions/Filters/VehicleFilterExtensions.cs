using System;
using System.Collections;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> filterByPairEnrollments(
            this IEnumerable<IVehicle> vehicles)
        {
            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Number % 2 == 0)
                    yield return vehicle;
            }
        }

        public static IEnumerable<IVehicle> filterByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            IEnumerator<IVehicle> enumVehicle = vehicles.GetEnumerator();

            while (enumVehicle.MoveNext())
            {
                if (enumVehicle.Current.Enrollment.Serial == enrollmentSerial)
                    yield return enumVehicle.Current;
            }
        }

        public static IEnumerable<IEngine> selectEngines(this IEnumerable<IVehicle> vehicles)
        {
            foreach (IVehicle vehicle in vehicles)
            {
                    yield return vehicle.Engine;
            }
        }

        public static IEnumerable<IEngine> filterByStarted(this IEnumerable<IEngine> engines)
        {
            foreach (IEngine engine in engines)
            {
                if (engine.IsStarted)
                    yield return engine;
            }
        }

        public static IEnumerable<IEngine> filterByHorsePowerGreaterOrEqual(this IEnumerable<IEngine> engines, int horsePower)
        {
            foreach (IEngine engine in engines)
            {
                if (engine.HorsePower >= horsePower)
                    yield return engine;
            }
        }
    }
}