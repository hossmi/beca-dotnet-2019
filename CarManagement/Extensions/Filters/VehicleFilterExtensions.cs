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
            List<IVehicle> vehiclesPairs = new List<IVehicle>();
            
            foreach(IVehicle vehicle in vehicles)
            {
                if(vehicle.Enrollment.Number % 2 == 0)
                {
                    vehiclesPairs.Add(vehicle);
                }
            }

            return vehiclesPairs;
        }

        public static IEnumerable<IVehicle> getVehiclesByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {         
            
            List<IVehicle> vehiclesBySerial = new List<IVehicle>();
            IEnumerator<IVehicle> enumerator = vehicles.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if(enumerator.Current.Enrollment.Serial == enrollmentSerial)
                {
                    vehiclesBySerial.Add(enumerator.Current);
                }                
            }

            return vehiclesBySerial;

            /*
            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Serial  == enrollmentSerial)
                {
                    vehiclesBySerial.Add(vehicle);
                }
            }

            return vehiclesBySerial;
            */
        }
    }
}