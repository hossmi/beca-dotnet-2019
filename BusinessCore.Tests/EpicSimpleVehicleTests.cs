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
        public void Vehicle_entities_shall_not_share_wheels()
        {
            DefaultEnrollmentProvider provider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(provider);

            builder.addWheel();
            builder.setDoors(3);
            builder.setEngine(12);

            Vehicle vehicle0 = builder.build();
            Vehicle vehicle1 = builder.build();

            vehicle0.setWheelsPressure(3.2);
            vehicle1.setWheelsPressure(1.1);

            Assert.IsFalse(vehicle0.Wheels[0].Pressure == vehicle1.Wheels[0].Pressure);
        }
    }
}
