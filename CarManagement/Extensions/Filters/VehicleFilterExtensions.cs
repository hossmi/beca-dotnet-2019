using System;
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
                {
                    yield return vehicle;
                }
            }
            
        }
        public static bool filterByIsPairEnrollment(IVehicle vehicle)
        {
            return vehicle.Enrollment.Number % 2 == 0;
        }

        public static IEnumerable<IVehicle> filterByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            
            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Serial  == enrollmentSerial)
                {
                    yield return vehicle;
                }
            }

        }
        public static bool filterByIsEnrollmentsSerial(IVehicle vehicle,string enrollmentSerial)
        {
            return vehicle.Enrollment.Serial == enrollmentSerial;
        }

        public static IEnumerable<IEngine> selectEngines(this IEnumerable<IVehicle> vehicles)
        {
            
            foreach (IVehicle  vehicle in vehicles)
            {
               
                    yield return vehicle.Engine;
      
            }
        }
        public static IEngine  selectIsEngines(IVehicle vehicle)
        {
          
           return vehicle.Engine;

        }

        public static IEnumerable<IEngine> filterByStarted(this IEnumerable<IEngine> engines)
        {
            foreach (IEngine engine in engines)
            {
                if (engine.IsStarted)
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
        public static IEnumerable<T2> filter2<T1,T2>(
        this IEnumerable<T1> items, Func<T1, T2,bool> filterDelegate)
        {
            foreach (T2 item in items)
            {
                yield return items;
                 
            }
        }
    }
}