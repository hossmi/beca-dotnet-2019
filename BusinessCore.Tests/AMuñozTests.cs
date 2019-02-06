using CarManagement.Extensions.Vehicles;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarManagement.Services;
using CarManagement.Services.CarManagement.Builders;

namespace BusinessCore.Tests
{
    [TestClass]
    public class AMuñozTests
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

            IVehicle vehicle = builder.build();
            Negassert.mustFail(() => vehicle.setWheelsPressure(-50));
        }

        [TestMethod]
        public void engine_horse_power_can_not_be_negative()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);           

            Negassert.mustFail(() => builder.setEngine(-70));
        }
    }
}
