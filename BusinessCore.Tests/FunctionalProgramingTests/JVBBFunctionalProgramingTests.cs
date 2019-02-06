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
            bool isTrue = false;

            IEnumerable<int> horsePowers = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .Select(vehicle => vehicle.Engine.HorsePower);

            if (horsePowers.All(power => power.Equals(666)))
                isTrue = true;

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void all_vehicles_with_enrollment_serial_CSM_have_their_third_door_open()
        {
            bool isTrue = false;

            IEnumerable<IDoor> thirdsDoors = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .Select(vehicle => vehicle.Doors[2]);

            if (thirdsDoors.All(door => door.IsOpen == true))
                isTrue = true;

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void count_of_open_and_closed_doors_from_CSM_vehicles()
        {
            int open = 0;
            int closed = 0;

            open = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .SelectMany(vehicle => vehicle.Doors)
                .Count(door => door.IsOpen == true);

            closed = this.vehicleStorage
                .getAll()
                .Where(vehicle => vehicle.Enrollment.Serial == "CSM")
                .SelectMany(vehicle => vehicle.Doors)
                .Count(door => door.IsOpen == false);

            Assert.AreEqual(open, 9);
            Assert.AreEqual(closed, 7);
        }
    }
}
