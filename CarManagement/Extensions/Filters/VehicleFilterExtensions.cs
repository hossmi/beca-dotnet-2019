using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using System.Linq;

namespace CarManagement.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IEnumerable<IVehicle> getVehiclesByPairEnrollments(this IEnumerable<IVehicle> vehicles)
        {
            IEnumerable<T> items = new T[] { new T("msg") };
            items.ToList().Add(new T("msg2"));

            IEnumerable<IVehicle> vehiclesEnrollmentPair = new Enumerable<IVehicle>();
            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.Enrollment.Number % 2 == 0)
                {
                    vehiclesEnrollmentPair.ToList().Add(vehicle);
                }
            }
            return vehiclesEnrollmentPair;
        }

        public static IEnumerable<IVehicle> getVehiclesByEnrollmentsSerial(this IEnumerable<IVehicle> vehicles, string enrollmentSerial)
        {
            throw new NotImplementedException();
        }
    }
}