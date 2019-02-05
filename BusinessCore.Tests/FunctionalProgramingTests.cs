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
                .Select(vehicle => vehicle.Engine)
                .Where(engine => engine.IsStarted);

            Assert.AreEqual(4, engines.Count());
        }

        [TestMethod]
        public void average_pressure_for_white_vehicles_is_3()
        {
            double avgPressure = 0.0;

            avgPressure = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Color == CarColor.White)
                .Select(vehicle => vehicle.Wheels)
                .SelectMany(wheels => wheels)
                .Average(wheel => wheel.Pressure);

            Assert.AreEqual(3.0, avgPressure);
        }

        [TestMethod]
        public void minimal_horsepower_is_100cv()
        {
            int horsePower = 0;

            horsePower = this.vehicleStorage
                .getAll()
                .Select(vehicle => vehicle.Engine)
                .Min(engine => engine.HorsePower);

            Assert.AreEqual(100, horsePower);
        }

        [TestMethod]
        public void maximal_horsepower_of_red_colored_and_stopped_cars_is_666cv()
        {
            int horsePower = 0;

            horsePower = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Color == CarColor.Red)
                .Select(vehicle => vehicle.Engine)
                .Where(engine => engine.IsStarted == false)
                .Max(engine => engine.HorsePower);

            Assert.AreEqual(666, horsePower);
        }

        [TestMethod]
        public void from_the_two_white_cars_with_opened_doors_get_serial_enrollment_and_horsePower()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                /* */
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
                /* */
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
