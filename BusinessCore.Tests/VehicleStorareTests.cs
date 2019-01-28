using System;
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
        public void fileVehicleStorage_must_persists_vehicles()
        {
            FakeEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new FileVehicleStorage();

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            Vehicle motoVehicle = vehicleBuilder.build();

            Assert.AreEqual(0, vehicleStorage.Count);
            vehicleStorage.set(motoVehicle);
            Assert.AreEqual(1, vehicleStorage.Count);

            vehicleStorage = new FileVehicleStorage();
            Assert.AreEqual(1, vehicleStorage.Count);

            Vehicle vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
            Assert.IsNotNull(vehicle);
            Assert.AreEqual(enrollmentProvider.DefaultEnrollment, vehicle.Enrollment);
        }

        [TestMethod]
        public void inMemoryVehicleStorage_must_be_empty_on_each_instance()
        {
            Vehicle vehicle, motoVehicle;

            FakeEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new InMemoryVehicleStorage();

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            motoVehicle = vehicleBuilder.build();

            Assert.AreEqual(0, vehicleStorage.Count);
            vehicleStorage.set(motoVehicle);
            Assert.AreEqual(1, vehicleStorage.Count);

            vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
            Assert.IsNotNull(vehicle);
            Assert.AreEqual(enrollmentProvider.DefaultEnrollment, vehicle.Enrollment);

            vehicleStorage = new InMemoryVehicleStorage();
            Assert.AreEqual(0, vehicleStorage.Count);

            try
            {
                vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
                Assert.Fail();
            }
            catch (UnitTestAssertException)
            {
                throw;
            }
            catch (NotImplementedException)
            {
                throw;
            }
            catch (Exception)
            {
                //good
            }
        }
    }
}
