using System;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class alloret
    {
        [TestMethod]
        public void pressure_must_not_be_less_than_1() // Posible conflicto con el test de JVBB 
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.setColor(CarColor.Black);
            builder.setDoors(4);
            builder.setEngine(60);
            builder.addWheel();
            builder.addWheel();

            Vehicle vehicle = builder.build();
            Negassert.mustFail(() => vehicle.setWheelsPressure(0));
        }
        [TestMethod]
        public void pressure_must_not_be_more_than_5()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.setColor(CarColor.Black);
            builder.setDoors(4);
            builder.setEngine(60);
            builder.addWheel();
            builder.addWheel();

            Vehicle vehicle = builder.build();
            Negassert.mustFail(() => vehicle.setWheelsPressure(6));
        }
    }
}
