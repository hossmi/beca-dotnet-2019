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
            //IVehicle[] vehicles = this.vehicleStorage
            bool isTrue = false;
            
            //isTrue = true;
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void all_vehicles_with_enrollment_serial_CSM_have_their_third_door_open()
        {
            bool isTrue = false;

            Assert.IsTrue(isTrue);

        }

        [TestMethod]
        public void count_of_open_and_closed_doors_from_CSM_vehicles()
        {
            int open = 0;
            int close = 0;

            open = this.vehicleStorage
                .getAll()
                .Select( vehicle => new
                {
                    open = vehicle.Doors.Where(door => door.IsOpen == true).Count(),
                    close = vehicle.Doors.Where(door => door.IsOpen == false).Count()
                })
                .Count();

            Assert.AreEqual(open,9);
            Assert.AreEqual(close, 7);
        }
    }
}
