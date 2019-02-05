using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> filterByPairEnrollments(//probar mas tarde con enumerator
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

        public static IEnumerable<IVehicle> filterByEnrollmentsSerial(
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

        public static IEnumerable<IEngine> selectEngines(
            this IEnumerable<IVehicle> vehicles)
        {
            List<IEngine> pairVehicles = new List<IEngine>();

            foreach (IVehicle vehicle in vehicles)
            {
                pairVehicles.Add(vehicle.Engine);
            }

            return pairVehicles;
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

        public static bool filterByIsStarted(IEngine engine)
        {
            return engine.IsStarted;
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

        public static IEnumerable<TOut> select<TIn, TOut>(
            this IEnumerable<TIn> items, Func<TIn, TOut> selectDelegate)
        {
            foreach (TIn item in items)
                yield return selectDelegate(item);
        }
    }
}