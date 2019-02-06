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
    public class FunctionalProgrammingTests_amunoz
    {

        private readonly ArrayVehicleStorage_amunoz vehicleStorage_amunoz;

        public FunctionalProgrammingTests_amunoz()
        {
            this.vehicleStorage_amunoz = new ArrayVehicleStorage_amunoz();
        }

        [TestMethod]
        public void get_total_horse_power_from_yellow_vehicles_with_less_than_three_wheels()
        {
            int horsePower = 0;
            /**/
            Assert.AreEqual(1366, horsePower);

        }
    }
}