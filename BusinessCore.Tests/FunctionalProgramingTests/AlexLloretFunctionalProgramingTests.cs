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
    [TestCategory("Functional Programing")]
    public class AlexLloretFunctionalProgramingTests
    {
        private readonly alloretArrayVehicleStorage vehicleStorage;

        public AlexLloretFunctionalProgramingTests()
        {
            this.vehicleStorage = new alloretArrayVehicleStorage();
        }

        [TestMethod]
        public void minimal_horsePower_for_vehicles_with_wheel_that_have_three_atmospheres_of_pressure_is_85cv()
        {
            int horsePower = 0;
            double pressure = 0;
            var value = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Wheels.All(pre => pre.Pressure == 3))
                .Select(vehicle => new
                {
                    pressure = vehicle.Wheels.First().Pressure,
                    horsePower = vehicle.Engine.HorsePower
                }
                )
               .OrderBy(pre => pre.pressure);
            horsePower = value.First().horsePower;
            pressure = value.First().pressure;

            Assert.AreEqual(3.0, pressure = 3);
            Assert.AreEqual(85, horsePower);
        }

        [TestMethod]
        public void get_wheels_and_pressure_value_grouping_by_enrollment_serial_ordering_by_wheels_number_and_pressure_value()
        {
            var vehicles = this.vehicleStorage

                .get()
                .GroupBy(vehicle => vehicle.Enrollment.Serial)
                .Select (group => new
                {
                    Pressure = group.SelectMany(wheels => wheels.Wheels).Average(pre => pre.Pressure),
                    WheelsCount = group.Sum(count => count.Wheels.Length)
                })
                .OrderBy(group => group.WheelsCount)
                .ThenBy(group => group.Pressure)
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);

            Type itemType = vehicles[0].GetType();
            Assert.AreEqual(2, itemType.GetProperties().Length);

            Assert.AreEqual(3, vehicles[0].WheelsCount);
            Assert.AreEqual(6, vehicles[0].Pressure);
            Assert.AreEqual(5, vehicles[1].WheelsCount);
            Assert.AreEqual(3.6, vehicles[1].Pressure);
            Assert.AreEqual(14, vehicles[2].WheelsCount);
            Assert.AreEqual(3.0428571428571423, vehicles[2].Pressure);
        }


         [TestMethod]
        public void from_the_two_red_cars_with_opened_doors_get_pressure_value_and_engine_status()
        {
            var vehicles = this.vehicleStorage

                .get()
                .Where (vehicle => vehicle.Color == CarColor.Red)
                .Where (vehicle => vehicle.Doors.Where (door => door.IsOpen == true).Count() >= 0 )
                .Select(vehicle => new
                {
                    vehicle.Doors,
                    vehicle.Engine.IsStarted                   
                }
                )
                .ToArray();

            Assert.AreEqual(2, vehicles.Length);
            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);


        }
    }
}
