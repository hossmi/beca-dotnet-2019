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
                /* */
                .ToArray();

            Assert.AreEqual(6, vehicles.Length);
        }

        [TestMethod]
        public void there_are_four_started_engines()
        {
            IEnumerable<IEngine> engines = this.vehicleStorage
                .getAll()
                /* */
                .Select(vehicle => vehicle.Engine);

            Assert.AreEqual(4, engines.Count());
        }

        [TestMethod]
        public void average_pressure_for_white_vehicles_is_3()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void minimal_horsepower_is_100cv()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void maximal_horsepower_of_red_colored_and_stopped_cars_is_666cv()
        {
            throw new NotImplementedException();
        }
    }
}
