using System;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class amunozeverisTests
    {
        [TestMethod]
        public void wheel_pressure_can_not_be_negative()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);


            builder.setColor(CarColor.Black);
            builder.setDoors(4);
            builder.setEngine(60);
            builder.addWheel();
            builder.addWheel();

            Vehicle vehicle = builder.build();
            vehicle.setWheelsPressure(-20);

            foreach (Wheel wheel in vehicle.Wheels)
            {
                Assert.IsTrue(wheel.Pressure >= 0);
            }
        }

        [TestMethod]
        public void engine_horse_power_can_not_be_negative()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);


            builder.setColor(CarColor.Black);
            builder.setDoors(2);
            builder.setEngine(-50);
            builder.addWheel();
            builder.addWheel();

            Vehicle vehicle = builder.build();

            Assert.IsTrue(vehicle.Engine.HorsePower >= 0);
        }
    }
}
