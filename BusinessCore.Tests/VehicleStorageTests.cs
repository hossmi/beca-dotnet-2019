using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class VehicleStorageTests
    {
        public string VehiclesFilePath
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "Vehicles.xml");
            }
        }

        [TestInitialize]
        [TestCleanup]
        public void initialize()
        {
            if (File.Exists(this.VehiclesFilePath))
                File.Delete(this.VehiclesFilePath);
        }

        [TestMethod]
        public void fileVehicleStorage_must_persists_vehicles()
        {
            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IDtoConverter dtoConverter = new DefaultDtoConverter(enrollmentProvider);
            IVehicleStorage vehicleStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);

            vehicleStorage.clear();
            Assert.AreEqual(0, vehicleStorage.Count);

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            Vehicle motoVehicle = vehicleBuilder.build();

            vehicleStorage.set(motoVehicle);
            Assert.AreEqual(1, vehicleStorage.Count);

            vehicleStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);
            Assert.AreEqual(1, vehicleStorage.Count);

            Vehicle vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
            Assert.IsNotNull(vehicle);
            Assert.IsTrue(equalityComparer.Equals(enrollmentProvider.DefaultEnrollment, vehicle.Enrollment));
        }

        [TestMethod]
        public void inMemoryVehicleStorage_must_be_empty_on_each_instance()
        {
            Vehicle vehicle, motoVehicle;

            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
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
            Assert.IsTrue(equalityComparer.Equals(enrollmentProvider.DefaultEnrollment, vehicle.Enrollment));

            vehicleStorage = new InMemoryVehicleStorage();
            Assert.AreEqual(0, vehicleStorage.Count);

            Negassert.mustFail(() =>
            {
                vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
            });
        }

        [TestMethod]
        public void vehicleStoarge_implementations_must_return_6_items()
        {
            ArrayEnrollmentProvider enrollmentProvider = new ArrayEnrollmentProvider();
            IDtoConverter dtoConverter = new DefaultDtoConverter(enrollmentProvider);
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);

            IVehicleStorage[] vehicleStorages = new IVehicleStorage[]
            {
                new InMemoryVehicleStorage(),
                new FileVehicleStorage(this.VehiclesFilePath, dtoConverter),
            };

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setColor(CarColor.Blue);
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);

            for (int i = 0; i < enrollmentProvider.Count; i++)
            {
                Vehicle vehicle = vehicleBuilder.build();
                foreach (IVehicleStorage vehicleStorage in vehicleStorages)
                    vehicleStorage.set(vehicle);
            }

            foreach (IVehicleStorage vehicleStorage in vehicleStorages)
            {
                Vehicle[] vehicles = vehicleStorage.getAll();
                Assert.AreEqual(enrollmentProvider.Count, vehicles.Length);
            }
        }
    }
}