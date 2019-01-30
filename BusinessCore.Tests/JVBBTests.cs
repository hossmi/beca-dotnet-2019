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
    [TestCategory("JVBB Tests")]
    public class JVBBTests
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
        public void WrongBuildRequests_01()
        {
            FakeEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            Negassert.mustFail(() => builder.removeWheel());
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            Negassert.mustFail(() => builder.setDoors(-27));
            builder.setDoors(5);
            Negassert.mustFail(() => builder.setEngine(-40));
            Negassert.mustFail(() => builder.setEngine(0));
            builder.setEngine(100);
            Negassert.mustFail(() => builder.setColor((CarColor)27));

            Vehicle vehicle = builder.build();

            Negassert.mustFail(() => vehicle.Engine.stop());

            vehicle.Engine.start();
            Negassert.mustFail(() => vehicle.Engine.start());

            Negassert.mustFail(() => vehicle.setWheelsPressure(-1));
            vehicle.setWheelsPressure(0);
            vehicle.setWheelsPressure(0.99);
            vehicle.setWheelsPressure(2);

        }

        [TestMethod]
        public void StorageComparison()
        {
            Vehicle motoVehicle;

            FakeEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IDtoConverter dtoConverter = new DefaultDtoConverter(enrollmentProvider);
            IVehicleStorage vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);
            IVehicleStorage vehicleMemoryStorageA = new InMemoryVehicleStorage();
            IVehicleStorage vehicleMemoryStorageB = new InMemoryVehicleStorage();

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            motoVehicle = vehicleBuilder.build();

            vehicleFileStorage.set(motoVehicle);
            vehicleMemoryStorageA.set(motoVehicle);
            vehicleMemoryStorageB.set(motoVehicle);
            vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);

            Vehicle memoryVehicleA = vehicleMemoryStorageA.get(enrollmentProvider.DefaultEnrollment);
            Vehicle memoryVehicleB = vehicleMemoryStorageB.get(enrollmentProvider.DefaultEnrollment);
            Vehicle fileVehicle = vehicleFileStorage.get(enrollmentProvider.DefaultEnrollment);

            Assert.IsNotNull(memoryVehicleA);
            Assert.IsNotNull(memoryVehicleB);
            Assert.IsNotNull(vehicleFileStorage);

            Assert.AreEqual(memoryVehicleA, memoryVehicleB);
            Assert.AreEqual(memoryVehicleA, fileVehicle);
            Assert.AreEqual(memoryVehicleB, fileVehicle);

        }

    }
}
