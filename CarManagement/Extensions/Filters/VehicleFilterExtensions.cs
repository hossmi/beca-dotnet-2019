using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<T> filter<T, T2>(
            this IEnumerable<T> items, Func<T, T2, bool> filterDelegate, T2 arg)
        {
            foreach (T item in items)
            {
                if (filterDelegate(item, arg))
                    yield return item;
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
        
        public static IVehicle get(this IVehicleStorage vehicleStorage, IEnrollment enrollment)
        {
            return vehicleStorage
                .get()
                .whereEnrollmentIs(enrollment)
                .Single();
        }

        public static IEnumerable<TOut> select<TIn, TOut>(
            this IEnumerable<TIn> items, Func<TIn, TOut> selectDelegate)
        {
            foreach (TIn item in items)
                yield return selectDelegate(item);
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