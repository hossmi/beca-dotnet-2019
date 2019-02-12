using System;
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
                .get()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .FirstOrDefault( vehicle => vehicle.Engine.HorsePower != 666)
                == null;

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void all_vehicles_with_enrollment_serial_CSM_have_their_third_door_open()
        {
            bool isTrue = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .FirstOrDefault(vehicle => vehicle.Doors[2].IsOpen == false)
                == null;
            
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void count_of_open_and_closed_doors_from_CSM_vehicles()
        {
            int open = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .SelectMany(vehicle => vehicle.Doors)
                .Where(door => door.IsOpen)
                .Count();
            int close = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .SelectMany(vehicle => vehicle.Doors)
                .Where(door => door.IsOpen == false)
                .Count();

            Assert.AreEqual(open,9);
            Assert.AreEqual(close, 7);


        
        }
    }
}
