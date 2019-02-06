﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class FunctionalProgramingTestsSoden
    {
        private readonly ArrayVehicleStorageSoden vehicleStorage;

        public FunctionalProgramingTestsSoden()
        {
            this.vehicleStorage = new ArrayVehicleStorageSoden();
        }

        [TestMethod]
        public void a_there_are_3_black_vehicles_and_horsePorwer_min100_and_started()
        {
            IVehicle[] vehicles = this.vehicleStorage
                .getAll()
                /**/
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);
        }

        [TestMethod]
        public void b_there_are_two_started_engines_more_one_doors_closed()
        {
            IEnumerable<IEngine> engines = this.vehicleStorage
                .getAll()
                /**/
                .Select(vehicle => vehicle.Engine);
            Assert.AreEqual(2, engines.Count());
        }

        [TestMethod]
        public void c_adds_the_pressure_of_the_wheels_of_the_cars_with_enrollment_number_above_100_and_black_color()
        {
            double pressure = this.vehicleStorage
               .getAll()
               /**/
               .Count();

                Assert.AreEqual(6, pressure);
        }

        [TestMethod]
        public void d_from_the_two_white_cars_with_any_doors_get_serial_enrollment_and_horsePower_adove_500()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                /**/
                .ToArray();

            Assert.AreEqual(1, vehicles.Length);
        }
    }
}
