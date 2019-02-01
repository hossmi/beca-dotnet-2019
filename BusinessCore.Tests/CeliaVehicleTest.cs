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
        public void cannot_remove_wheel_if_there_are_not_wheels()
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
        public void cannot_set_15_doors()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            Negassert.mustFail(() => builder.setDoors(15));
        }

        [TestMethod]
        public void cannot_openDoor_twice()
        {
            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
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