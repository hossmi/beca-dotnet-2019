﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestCategory("Functional Programing")]
    [TestClass]
    public class SodenFunctionalProgramingTests
    {
        private readonly ArrayVehicleStorageSoden vehicleStorage;

        public SodenFunctionalProgramingTests()
        {
            this.vehicleStorage = new ArrayVehicleStorageSoden();
        }

        [TestMethod]
        public void a_there_are_3_black_vehicles_and_horsePorwer_min100_and_started()
        {
            IVehicle[] vehicles = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Color == CarColor.Black)
                .Where(vehicle => vehicle.Engine.IsStarted == true)
                .Where(vehicle => vehicle.Engine.HorsePower >= 100)
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);
        }

        [TestMethod]
        public void b_there_are_two_started_engines_more_one_doors_closed()
        {
            IEnumerable<IEngine> engines = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Engine.IsStarted == true)
                .Where(vehicle => vehicle.Doors.Count(door => door.IsOpen == false) >1)
                .Select(vehicle => vehicle.Engine);
            Assert.AreEqual(2, engines.Count());
        }

        [TestMethod]
        public void get_sum_number_of_wheels_of_black_vehicles_with_enrollment_number_higher_to_100_is_6()
        {
            double pressure = this.vehicleStorage
               .getAll()
               .Where(vehicle => vehicle.Color == CarColor.Black)
               .Where(vehicle => vehicle.Enrollment.Number > 100)
               .Select(vehicle=> vehicle.Wheels)
               .Count();

                Assert.AreEqual(6, pressure);
        }

        [TestMethod]
        public void get_serial_enrollment_from_white_vehicles_with_at_least_one_door_and_horsePower_adobe_500cv()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                /**/
                .Select(vehicle => new { vehicle.Enrollment.Serial})
                .ToArray();

            Assert.AreEqual("PNG", vehicles[0].Serial);
        }
    }
}
