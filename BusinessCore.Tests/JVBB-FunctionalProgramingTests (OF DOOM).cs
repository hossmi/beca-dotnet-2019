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
    [TestCategory("JVBB Tests")]
    public class JVBBFunctionalProgramingTests
    {
        private readonly JVBBArrayVehicleStorage vehicleStorage;

        public JVBBFunctionalProgramingTests()
        {
            this.vehicleStorage = new JVBBArrayVehicleStorage();
        }

        [TestMethod]
        public void all_vehicles_with_enrollment_serial_CSM_have_666_horsepower()
        {
            bool isTrue = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .Select(vehicle => vehicle.Engine.HorsePower)
                .All(horsepower => horsepower == 666)
                ;

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void there_are_four_started_engines()
        {

        }

        [TestMethod]
        public void average_pressure_for_white_vehicles_is_3()
        {

        }

        [TestMethod]
        public void minimal_horsepower_is_100cv()
        {

        }

        [TestMethod]
        public void maximal_horsepower_of_red_colored_and_stopped_cars_is_666cv()
        {
            int horsePower = 0;

            Assert.AreEqual(666, horsePower);
        }

        [TestMethod]
        public void from_the_two_white_cars_with_opened_doors_get_serial_enrollment_and_horsePower()
        {

        }


        [TestMethod]
        public void get_enrollment_serial_and_average_horse_power_grouping_by_enrollment_serial_ordering_by_serial_and_average_horse_power()
        {

        }
    }
}
