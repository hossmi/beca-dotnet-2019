using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class alloretTestsFunctional
    {
        private readonly alloretArrayVehicleStorage vehicleStorage;

        public alloretTestsFunctional()
        {
            this.vehicleStorage = new alloretArrayVehicleStorage();
        }

        [TestMethod]
        public void minimal_horsePower_for_vehicles_with_wheel_that_have_three_atmospheres_of_pressure_is_85cv()
        {
            int horsePower = this.vehicleStorage
                .getAll()
                .Where
                (vehicle =>
                    vehicle.Wheels.Where(wheel => wheel.Pressure == 3.0).Count()

                    ==

                    vehicle.Wheels.Count()
                )
                .Select(vehicle => vehicle.Engine.HorsePower)
                .Min();
            double pressure = 3.0;
            
              /* Insert code here for boom! */
               
            Assert.AreEqual(3.0, pressure);
            Assert.AreEqual(85, horsePower);
        }

        [TestMethod]
        public void get_wheels_and_pressure_value_grouping_by_enrollment_serial_ordering_by_wheels_number_and_pressure_value()
        {
            var vehicles = this.vehicleStorage

                .getAll()
                .GroupBy(vehicle => vehicle.Enrollment.Serial)
                .Select(group =>
                    new {
                        WheelsCount = group.Select(vehicle => vehicle.Wheels.Count()).First(),
                        Pressure = group.Select(vehicle => vehicle.Wheels.Select(wheel => wheel.Pressure).Average())
                        }
                )
                .OrderBy(anonym => anonym.Pressure)
                .OrderBy(anonym => anonym.WheelsCount)
                /* Insert code here for boom! */
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);

            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);

            Assert.AreEqual(2, vehicles[0].WheelsCount);
            Assert.AreEqual(2.5, vehicles[0].Pressure);
            Assert.AreEqual(4, vehicles[1].WheelsCount);
            Assert.AreEqual(2.8, vehicles[1].Pressure);
            Assert.AreEqual(2, vehicles[2].WheelsCount);
            Assert.AreEqual(2.1, vehicles[2].Pressure);
        }


         [TestMethod]
        public void from_the_two_red_cars_with_opened_doors_get_pressure_value_and_engine_status()
        {
            var vehicles = this.vehicleStorage

                .getAll()
                .Where(vehicle => vehicle.Color == CarColor.Red)
                .Where(vehicle=>
                    vehicle.Doors.Where(door => door.IsOpen).Count()

                    >

                    0
                )
                .Select(vehicle => new {vehicle.Wheels, vehicle.Engine.IsStarted})
                /* Insert code here for boom! */
                .ToArray();

            Assert.AreEqual(2, vehicles.Length);
            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);


        }
    }
}
