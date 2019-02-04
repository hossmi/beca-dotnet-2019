using CarManagement.Extensions.Vehicles;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarManagement.Services;

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

            IVehicle vehicle0 = builder.build();
            IVehicle vehicle1 = builder.build();

            vehicle0.setWheelsPressure(3.2);
            vehicle1.setWheelsPressure(1.1);

            Assert.IsFalse(vehicle0.Wheels[0].Pressure == vehicle1.Wheels[0].Pressure);
        }
    }
}

