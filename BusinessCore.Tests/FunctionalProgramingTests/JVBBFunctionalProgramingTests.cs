﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Models;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    [TestCategory("JVBB Tests")]
    [TestCategory("Functional Programing")]
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
                .All(horsepower => horsepower == 666);

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void all_vehicles_with_enrollment_serial_CSM_have_their_third_door_open()
        {
            bool isTrue = this.vehicleStorage

                .getAll()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .Select(vehicle => vehicle.Doors)
                .All(doors => doors.ElementAt(2).IsOpen == true);

            Assert.IsTrue(isTrue);

        }

        [TestMethod]
        public void count_of_open_and_closed_doors_from_CSM_vehicles()
        {
            var values = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .SelectMany(vehicle => vehicle.Doors)
                .GroupBy(door => door.IsOpen);


            int open = values.ElementAt(0).Count();
            int close = values.ElementAt(1).Count();

            Assert.AreEqual(open,9);
            Assert.AreEqual(close, 7);


        
        }
    }
}
