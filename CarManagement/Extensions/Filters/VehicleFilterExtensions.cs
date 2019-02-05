using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        //-----------------------------Metodos-----------------------------

        public static IEnumerable<IVehicle> filterByPairEnrollments(
            this IEnumerable<IVehicle> vehicles)
        {


            foreach (IVehicle vehicle in vehicles)
            {
                if (filterByIsPairEnrollment(vehicle))
                {
                    yield return vehicle;
                }
            }

        }

        public static IEnumerable<IVehicle> filterByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {

            foreach (IVehicle vehicle in vehicles)
            {
                if (filterByIsEnrollmentsSerial(vehicle, enrollmentSerial))
                {
                    yield return vehicle;
                }
            }

        }

        public static IEnumerable<IEngine> selectEngines(this IEnumerable<IVehicle> vehicles)
        {

            foreach (IVehicle vehicle in vehicles)
            {

                yield return selectIsEngines(vehicle);


            }
        }


        public static IEnumerable<IEngine> filterByStarted(this IEnumerable<IEngine> engines)
        {
            foreach (IEngine engine in engines)
            {
                if (filterByIsStarted(engine))
                    yield return engine;
            }
        }


        //-----------------------------Filters-----------------------------

        public static bool filterByIsPairEnrollment(IVehicle vehicle)
        {
            return vehicle.Enrollment.Number % 2 == 0;
        }
        public static bool filterByIsEnrollmentsSerial(IVehicle vehicle, string enrollmentSerial)
        {
            return vehicle.Enrollment.Serial == enrollmentSerial;
        }
        public static IEngine selectIsEngines(IVehicle vehicle)
        {

            return vehicle.Engine;

        }
        public static bool filterByIsStarted(IEngine engine)
        {
            return engine.IsStarted;
        }

        //-----------------------------Comodines-----------------------------

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

        //---------------------------------------------------------------

    }
}