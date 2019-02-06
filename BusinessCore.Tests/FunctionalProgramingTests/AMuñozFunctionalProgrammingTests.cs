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
    public class AMuñozFunctionalProgrammingTests
    {
        private readonly ArrayVehicleStorage_amunoz vehicleStorage_amunoz;

        public AMuñozFunctionalProgrammingTests()
        {
            this.vehicleStorage_amunoz = new ArrayVehicleStorage_amunoz();
        }

        [TestMethod]
        public void get_total_horse_power_from_yellow_vehicles_with_less_than_three_wheels()
        {
            int horsePower = this.vehicleStorage_amunoz
                .getAll()
                .Where(vehicle => vehicle.Color == CarColor.Yellow)
                .Where(vehicle => vehicle.Wheels.Length < 3)
                .Sum(vehicle => vehicle.Engine.HorsePower);
            
            Assert.AreEqual(1366, horsePower);
        }

    }
}