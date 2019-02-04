using CarManagement.Extensions.Vehicles;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarManagement.Services;
using CarManagement.Services.CarManagement.Builders;

namespace BusinessCore.Tests
{
    [TestClass]
    public class alloretTests
    {
        [TestMethod]
        public void Pressure_must_not_be_less_than_1() // Posible conflicto con el test de JVBB 
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.setColor(CarColor.Black);
            builder.setDoors(4);
            builder.setEngine(60);
            builder.addWheel();
            builder.addWheel();

            IVehicle vehicle = builder.build();
            Negassert.mustFail(() => vehicle.setWheelsPressure(0));
            Negassert.mustFail(() => vehicle.setWheelsPressure(0.99));
            Negassert.mustFail(() => vehicle.setWheelsPressure(-1));
            Negassert.mustFail(() => vehicle.setWheelsPressure(-10.5));
        }
        [TestMethod]
        public void Pressure_must_not_be_more_than_5()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.setColor(CarColor.Black);
            builder.setDoors(4);
            builder.setEngine(60);
            builder.addWheel();
            builder.addWheel();

            IVehicle vehicle = builder.build();
            Negassert.mustFail(() => vehicle.setWheelsPressure(6));
            Negassert.mustFail(() => vehicle.setWheelsPressure(5.01));
            Negassert.mustFail(() => vehicle.setWheelsPressure(100));
            Negassert.mustFail(() => vehicle.setWheelsPressure(120.01));
        }
    }
}
