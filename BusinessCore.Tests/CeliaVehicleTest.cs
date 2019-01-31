using System;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class CeliaVehicleTest
    {
        [TestMethod]
        public void cannot_removeWheell_if_there_is_no()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.addWheel();
            builder.addWheel();
            builder.removeWheel();
            builder.removeWheel();

            Negassert.mustFail(() => builder.removeWheel());
        }

        [TestMethod]
        public void cannot_setDoors_15()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            Negassert.mustFail(() => builder.setDoors(15));
        }

        [TestMethod]
        public void cannot_openDoor_twice()
        {
            FakeEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(2);
            builder.setEngine(100);
            builder.setColor(CarColor.Red);

            Vehicle vehicle = builder.build();
            vehicle.Doors[0].open();
            Negassert.mustFail(() => vehicle.Doors[0].open());
        }
    }
}