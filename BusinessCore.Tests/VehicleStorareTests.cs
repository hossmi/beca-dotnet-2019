using System;
using System.Collections.Generic;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class VehicleStorareTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            FakeEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new FileVehicleStorage();

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            Vehicle motoVehicle = vehicleBuilder.build();

            vehicleStorage.set(motoVehicle);

            vehicleStorage = new FileVehicleStorage();
            Assert.AreEqual(1, vehicleStorage.Count);

            Vehicle vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
            Assert.IsNotNull(vehicle);
            Assert.AreEqual(enrollmentProvider.DefaultEnrollment, vehicle.Enrollment);
        }
    }
}
