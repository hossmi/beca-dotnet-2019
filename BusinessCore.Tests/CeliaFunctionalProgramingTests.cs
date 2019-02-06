﻿using System;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class CeliaFunctionalProgramingTests
    {
        private readonly CeliaArrayVehicleStorage vehicleStorage;

        public CeliaFunctionalProgramingTests()
        {
            this.vehicleStorage = new CeliaArrayVehicleStorage();
        }

        [TestMethod]
        public void get_the_enrollment_and_horsePower_of_the_vehicles_ordered_descending_by_horsePower()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                .GroupBy(vehicle => vehicle.Engine.HorsePower)
                .Select(group => new
                {
                    group.Key,
                    HorsePower= 0
                })
                //.OrderBy()
                .ToArray();

            Assert.AreEqual(666, vehicles[0].HorsePower);
            Assert.AreEqual(600, vehicles[5].HorsePower);
            Assert.AreEqual(100, vehicles[9].HorsePower);
        }

        [TestMethod]
        public void get_the_vehicles_black_with_all_their_doors_closed()
        {
            IVehicle[] vehicles = this.vehicleStorage
                .getAll()
                /**/
                //.Where(door => door.IsOpen == false)
                .ToArray();

            Assert.AreEqual(5, vehicles.Length);
        }
    }
}
