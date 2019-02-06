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
            bool isTrue = false;
            isTrue = this.vehicleStorage
                .getAll()
                .All(vehicle => vehicle.Enrollment.Serial == "CSM" && vehicle.Engine.HorsePower == 666);

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void all_vehicles_with_enrollment_serial_CSM_have_their_third_door_open()
        {
            bool isTrue = false;
            isTrue = this.vehicleStorage
                .getAll()
                .All(vehicle => vehicle.Enrollment.Serial == "CSM" && 
                    vehicle.Doors.Where(doors => doors.IsOpen == true).Count() != 3);

            Assert.IsTrue(isTrue);

        }

        [TestMethod]
        public void count_of_open_and_closed_doors_from_CSM_vehicles()
        {
            int open = 0;
            int close = 0;
            //var count = this.vehicleStorage
            //    .getAll()
            //    .SelectMany (vehicle => vehicle.Doors )
            //    .Select(doors => new
            //    {
            //        open = doors => doors.
            //    }
            //    )
                


            Assert.AreEqual(open,9);
            Assert.AreEqual(close, 7);


        
        }
    }
}
