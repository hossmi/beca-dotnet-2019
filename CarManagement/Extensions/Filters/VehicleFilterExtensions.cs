using System;
using System.Collections;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        //public static IEnumerable<IVehicle> filterByPairEnrollments(
        //    this IEnumerable<IVehicle> vehicles)
        //{
        //    foreach (IVehicle vehicle in vehicles)
        //    {
        //        if (vehicle.Enrollment.Number % 2 == 0)
        //            yield return vehicle;
        //    }
        //}

        //public static IEnumerable<IVehicle> filterByEnrollmentsSerial(
        //    this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        //{
        //    IEnumerator<IVehicle> enumVehicle = vehicles.GetEnumerator();

        //    while (enumVehicle.MoveNext())
        //    {
        //        if (enumVehicle.Current.Enrollment.Serial == enrollmentSerial)
        //            yield return enumVehicle.Current;
        //    }
        //}

        public static IEnumerable<IEngine> selectEngines(this IEnumerable<IVehicle> vehicles)
        {
            foreach (IVehicle vehicle in vehicles)
            {
                yield return vehicle.Engine;
            }
        }

        //public static IEnumerable<IEngine> filterByStarted(this IEnumerable<IEngine> engines)
        //{
        //    foreach (IEngine engine in engines)
        //    {
        //        if (engine.IsStarted)
        //            yield return engine;
        //    }
        //}

        //public static IEnumerable<IEngine> filterByHorsePowerGreaterOrEqual(this IEnumerable<IEngine> engines, int horsePower)
        //{
        //    foreach (IEngine engine in engines)
        //    {
        //        if (engine.HorsePower >= horsePower)
        //            yield return engine;
        //    }
        //}

        public static bool filterByPairEnrollments(IVehicle vehicle)
        {
            return vehicle.Enrollment.Number % 2 == 0;
        }

        public static bool filterByEnrollmentsSerial(IVehicle vehicle, string enrollmentSerial)
        {
            return vehicle.Enrollment.Serial == enrollmentSerial;
        }

        public static IEngine selectEngine(IVehicle vehicle)
        {
            return vehicle.Engine;
        }

        public static bool filterByIsStarted(IEngine engine)
        {
            return engine.IsStarted;
        }

        public static bool filterByHorsePowerGreaterOrEqual(IEngine engine, int horsePower)
        {
            return engine.HorsePower >= horsePower;
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

        public static IEnumerable<T> filter<T,T2>(
            this IEnumerable<T> items, Func<T,T2, bool> filterDelegate,T2 value)
        {
            foreach (T item in items)
            {
                if (filterDelegate(item,value))
                    yield return item;
            }
        }

    }
}