﻿using System;
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
            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
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

            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IDtoConverter dtoConverter = new DefaultDtoConverter(enrollmentProvider);
            IVehicleStorage vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);
            IVehicleStorage vehicleMemoryStorage = new InMemoryVehicleStorage();


            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            motoVehicle = vehicleBuilder.build();

            vehicleFileStorage.set(motoVehicle);
            vehicleMemoryStorage.set(motoVehicle);

            vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);

            Vehicle memoryVehicleA = vehicleMemoryStorage.get(enrollmentProvider.DefaultEnrollment);
            Vehicle fileVehicle = vehicleFileStorage.get(enrollmentProvider.DefaultEnrollment);

            Assert.IsNotNull(memoryVehicleA);
            Assert.IsNotNull(vehicleFileStorage);

            Assert.IsTrue(SameVehicle(memoryVehicleA, fileVehicle, equalityComparer));
        }

        private static bool SameVehicle(Vehicle v1, Vehicle v2, IEqualityComparer<IEnrollment> enrollmentComparer)
        {

            if (v1.Color != v2.Color)
                return false;

            if (v1.WheelCount != v2.WheelCount)
                return false;

            for (int i = 0; i < v1.WheelCount; i++)
                if (v1.Wheels[i].Pressure != v2.Wheels[i].Pressure)
                    return false;

            if (v1.DoorsCount != v2.DoorsCount)
                return false;

            for (int i = 0; i < v1.DoorsCount; i++)
                if (v1.Doors[i].IsOpen != v2.Doors[i].IsOpen)
                    return false;

            if (v1.Engine.HorsePower != v2.Engine.HorsePower)
                return false;

            if (v1.Engine.IsStarted != v2.Engine.IsStarted)
                return false;

            if (enrollmentComparer.Equals(v1.Enrollment, v2.Enrollment) == false)
                return false;

            return true;
        }
    }
}
