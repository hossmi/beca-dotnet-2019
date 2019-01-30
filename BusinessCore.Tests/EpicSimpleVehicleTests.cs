using System;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class EpicSimpleVehicleTests
    {
        [TestMethod]
        public void Wheels_Shall_Be_Different_Entities()
        {
            DefaultEnrollmentProvider provider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(provider);

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.setDoors(3);
            builder.setEngine(12);

            Vehicle vehicle0 = builder.build();
            Vehicle vehicle1 = builder.build();

            vehicle0.setWheelsPressure(3.2);
            vehicle1.setWheelsPressure(1.1);

            Assert.IsFalse(vehicle0.Wheels[0].Pressure == vehicle1.Wheels[0].Pressure);
            Assert.IsFalse(vehicle0.Wheels[1].Pressure == vehicle1.Wheels[0].Pressure);
            Assert.IsFalse(vehicle0.Wheels[2].Pressure == vehicle1.Wheels[0].Pressure);
        }
    }
}
