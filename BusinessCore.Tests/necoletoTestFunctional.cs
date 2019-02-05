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
    class necoletoTestFunctional
    {
        private readonly NecoletoArrayVehicleStorage vehicleStorage;

        public necoletoTestFunctional()
        {
            this.vehicleStorage = new NecoletoArrayVehicleStorage();
        }
        [TestMethod]
        public void get_the_number_of_all_black_vehicle_with_all_the_doors_closed()
        {

            int count = 0;
            count = this.vehicleStorage
            .getAll()
            .Where(vehicle => vehicle.Color == CarColor.Black)
            .SelectMany(vehicle => vehicle.Doors)
            .Where(doors => doors.IsOpen == false)
            .Count();

            /* Insert code here for boom! */

            Assert.AreEqual(3.0, count);
            
        }
    }
}
