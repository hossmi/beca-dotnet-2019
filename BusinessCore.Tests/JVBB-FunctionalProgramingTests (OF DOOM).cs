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
    [TestCategory("JVBB Tests")]
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
                /* */
                ;

            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void all_vehicles_with_enrollment_serial_CSM_have_their_third_door_open()
        {
            bool isTrue = this.vehicleStorage
                .getAll()
                /* */
                ;

            Assert.IsTrue(isTrue);

        }

        [TestMethod]
        public void count_of_open_and_closed_doors_from_CSM_vehicles()
        {
            var returnedValues = this.vehicleStorage
                .getAll()
                /* */
                ;

            int open = returnedValues.ElementAt(0).Count();
            int close = returnedValues.ElementAt(1).Count();

            Assert.AreEqual(open,9);
            Assert.AreEqual(close, 7);


        }

        [TestMethod]
        public void return_all_wheels_with_4point5_pressure()
        {
            IEnumerable<IWheel> wheels = this.vehicleStorage
                .getAll()
                /* */
                ;

            Assert.AreEqual(wheels.Count(), 6);

        }
    }
}
