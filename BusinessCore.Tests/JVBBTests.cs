using System;
using CarManagement.Builders;
using CarManagement.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    [TestCategory ("JVBB Tests")]
    public class JVBBTests
    {
        [TestMethod]
        public void BrainDamagedBuilder_01()
        {
            FakeEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            Negassert.mustFail(() => builder.removeWheel());
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            Negassert.mustFail(() => builder.setDoors(-27));
            Negassert.mustFail(() => builder.setEngine(-40));
            Negassert.mustFail(() => builder.setColor((CarColor)27));

            Vehicle vehicle = builder.build();

            vehicle.Engine.stop();

            vehicle.Engine.start();
            Negassert.mustFail(() => vehicle.Engine.start());

            Negassert.mustFail(() => vehicle.setWheelsPressure(-1));
            Negassert.mustFail(() => vehicle.setWheelsPressure(0));
            Negassert.mustFail(() => vehicle.setWheelsPressure(0.99));
            vehicle.setWheelsPressure(2);

        }
    }
}
