﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestCategory("Functional Programing")]
    [TestClass]
    public class NecoletoFunctionalProgramingTests
    {
        private readonly NecoletoArrayVehicleStorage vehicleStorage;
        

        public NecoletoFunctionalProgramingTests()
        {
            this.vehicleStorage = new NecoletoArrayVehicleStorage();
        }
        [TestMethod]
        public void get_the_number_of_the_doors_closed_of_all_black_vehicles()
        {
            int count = 0;
            var vehicles = this.vehicleStorage
            .get()
            .Select(vehicle => new
            {
                vehicle.Engine.HorsePower
                //Para que compile
            });

            /* Insert code here for boom! */

            Assert.AreEqual(15, count);

        }


    }
}
