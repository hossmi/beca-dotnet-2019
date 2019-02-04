﻿using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using System.Linq;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> filterByPairEnrollments(
            this IEnumerable<IVehicle> vehicles)
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

        public static IEnumerable<IVehicle> filterByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            List<IVehicle> vehiclesSerial = new List<IVehicle>();

            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Serial == enrollmentSerial)
                {
                    vehiclesSerial.Add(vehicle);
                }
            }

            return vehicles; ;
        }

        public static IEnumerable<IEngine> selectEngines(this IEnumerable<IVehicle> vehicles)
        {
            List<IEngine> engines = new List<IEngine>();

            foreach (IVehicle vehicle in vehicles)
            {
                engines.Add(vehicle.Engine);
            }

            return engines;
            ;// throw new NotImplementedException();
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