﻿using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> filterByPairEnrollments(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial, int enrollmentNumber)
        {
            return vehicles
                .Where(condition1 => condition1.Enrollment.Serial == enrollmentSerial)
                .Where(condition2 => condition2.Enrollment.Number == enrollmentNumber);
        }

        public static IEnumerable<IVehicle> filterByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            return vehicles
                .Where(vehicle => vehicle.Enrollment.Serial == enrollmentSerial);
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

        public static IVehicle get(this IVehicleStorage vehicleStorage, IEnrollment enrollment)
        {
            return vehicleStorage   
                .get()
                .whereEnrollmentIs(enrollment)
                .Single();
        }

    }
}