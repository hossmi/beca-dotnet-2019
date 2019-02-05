using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class FunctionalProgramingTests
    {
        private readonly ArrayVehicleStorage vehicleStorage;

        public FunctionalProgramingTests()
        {
            this.vehicleStorage = new ArrayVehicleStorage();
        }

        [TestMethod]
        public void there_are_six_black_vehicles()
        {
            IVehicle[] vehicles = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Color == CarColor.Black)
                .ToArray();

            Assert.AreEqual(6, vehicles.Length);
        }

        [TestMethod]
        public void there_are_four_started_engines()
        {
            IEnumerable<IEngine> engines = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Engine.IsStarted == true)
                .Select(vehicle => vehicle.Engine);

            Assert.AreEqual(4, engines.Count());
        }

        [TestMethod]
        public void average_pressure_for_white_vehicles_is_3()
        {
            double pressure = this.vehicleStorage
               .getAll()
               .Where(vehicle => vehicle.Color == CarColor.White)
               .SelectMany(vehicle => vehicle.Wheels)
               .Select(wheel => wheel.Pressure)
               .Average();

                Assert.AreEqual(3.0, pressure);
        }

        [TestMethod]
        public void minimal_horsepower_is_100cv()
        {
            int horsePower = this.vehicleStorage
                .getAll()
                .Select( vehicle => vehicle.Engine.HorsePower)
                .Min();

            Assert.AreEqual(100, horsePower);
        }

        [TestMethod]
        public void maximal_horsepower_of_red_colored_and_stopped_cars_is_666cv()
        {
            int horsePower = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Color == CarColor.Red)
                .Where(vehicle => vehicle.Engine.IsStarted == false)
                .Select( vehicle => vehicle.Engine.HorsePower)
                .Max();

            Assert.AreEqual(666, horsePower);
        }

        [TestMethod]
        public void from_the_two_white_cars_with_opened_doors_get_serial_enrollment_and_horsePower()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Color==CarColor.White)
                .Where(vehicle => vehicle.Doors.Any(door => door.IsOpen == true))
                .Select( vehicle => new {  vehicle.Enrollment.Serial,  vehicle.Engine.HorsePower})
                .ToArray();

            Assert.AreEqual(2, vehicles.Length);
            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);
        }


        [TestMethod]
        public void get_enrollment_serial_and_average_horse_power_grouping_by_enrollment_serial_ordering_by_serial_and_average_horse_power()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                .GroupBy(vehicle => vehicle.Enrollment.Serial)
                .Select( group => new
                {
                    Serial = group.Key,

                    AverageHorsePower = group
                    .ToList()
                    .Select(vehicle => vehicle.Engine.HorsePower)
                    .Average()
                })
                .OrderBy(group => group.Serial)
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);

            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);

            Assert.AreEqual("JVC", vehicles[0].Serial);
            Assert.AreEqual(622, vehicles[0].AverageHorsePower);
            Assert.AreEqual("PNG", vehicles[1].Serial);
            Assert.AreEqual(633, vehicles[1].AverageHorsePower);
            Assert.AreEqual("ZZZ", vehicles[2].Serial);
            Assert.AreEqual(539.6, vehicles[2].AverageHorsePower);
        }
    }
}
