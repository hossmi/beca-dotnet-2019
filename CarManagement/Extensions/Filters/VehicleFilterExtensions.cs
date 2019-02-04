using System;
using System.Collections.Generic;
using CarManagement.Core.Models;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> getVehiclesByPairEnrollments(
            this IEnumerable<IVehicle> vehicles)
        {

            var CurrentEnrollment = vehicles.GetEnumerator();
            foreach (IVehicle vehiclesPairNumber in vehicles)
            {
                if (CurrentEnrollment.Current.Enrollment.Number % 2 == 0)
                {
                    yield return vehiclesPairNumber;
                }
            }
            
        }

        public static IEnumerable<IVehicle> getVehiclesByEnrollmentsSerial(
            this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            throw new NotImplementedException();

        }

        public static IEnumerable<IEngine> getEngines(this IEnumerable<IVehicle> vehicles)
        {
            throw new NotImplementedException();
        }
    }
}