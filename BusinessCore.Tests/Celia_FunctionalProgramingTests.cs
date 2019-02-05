using System;
using System.Linq;
using BusinessCore.Tests.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class Celia_FunctionalProgramingTests
    {
        private readonly ArrayVehicleStorage vehicleStorage;

        public Celia_FunctionalProgramingTests()
        {
            this.vehicleStorage = new ArrayVehicleStorage();
        }

        [TestMethod]
        public void get_the_enrollment_and_horsePower_of_the_vehicles_ordered_descending_by_horsePower()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                .Select(vehicle => new
                {
                    vehicle.Enrollment,
                    vehicle.Engine.HorsePower
                })
                .OrderByDescending(vehicle => vehicle.HorsePower)
                .ToArray();

            Assert.AreEqual(100, vehicles[0].HorsePower);
            //Assert.AreEqual(600, vehicles[5].HorsePower);
            //Assert.AreEqual(100, vehicles[9].HorsePower);
        }
    }
}
