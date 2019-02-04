using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {

        public static IEnumerable<IEngine> selectEngines(this IEnumerable<IVehicle> vehicles)
        {
            foreach (IVehicle vehicle in vehicles)
            {
                yield return vehicle.Engine;
            }
        }

        public static IEnumerable<T> filter<T>(
            this IEnumerable<T> items, Func<T, bool> filterDelegate)
        {
            foreach (T item in items)
            {
                if (filterDelegate(item))
                    yield return item;
            }
        }

        #region "filters"
        public static bool filterByPairEnrollments(
            this IVehicle vehicle)
        {
            return vehicle.Enrollment.Number % 2 == 0;
        }
        public static bool filterByEnrollmentsSerial(
            this IVehicle vehicle, string enrollmentSerial)
        {
            return vehicle.Enrollment.Serial == enrollmentSerial;
        }
        public static bool filterByIsStarted(
            this IEngine engine)
        {
            return engine.IsStarted;
        }
        public static bool filterByHorsePowerGreaterOrEqual(
            this IEngine engine, int horsePower)
        {
            return engine.HorsePower >= horsePower;
        }
        #endregion
    }
}