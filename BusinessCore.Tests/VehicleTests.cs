﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class VehicleTests
    {
        private const int SMALL = 10 * 1000;
        private const int MEDIUM = 10 * SMALL;
        private const int LARGE = 10 * MEDIUM;
        private const string EVIL_ENROLLMENT = "XXX-0666";

        [TestMethod]
        public void builder_default_functionality()
        {
            IEnrollmentProvider enrollmentProvider = new FakeEnrollmentProvider(EVIL_ENROLLMENT);
            VehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(2);
            builder.setEngine(100);
            builder.setColor(CarColor.Red);

            Vehicle vehicle = builder.build();

            Assert.IsNotNull(vehicle);
            Assert.IsFalse(string.IsNullOrWhiteSpace(vehicle.Enrollment));
            Assert.AreEqual(2, vehicle.DoorsCount);
            Assert.AreEqual(4, vehicle.WheelCount);

            Assert.AreEqual(EVIL_ENROLLMENT, vehicle.Enrollment);

            // propiedad de solo lectura 
            // propiedad: array Door
            // campo privado: List Door
            vehicle.Doors[0].open();
            Assert.IsTrue(vehicle.Doors[0].IsOpen);
            Assert.IsFalse(vehicle.Doors[1].IsOpen);

            vehicle.Doors[0].close();
            Assert.IsFalse(vehicle.Doors[0].IsOpen);

            vehicle.Engine.start();
            Assert.IsTrue(vehicle.Engine.IsStarted);

            //ha de establecer la presion de cada rueda
            vehicle.setWheelsPressure(2.4);

            // propiedad de solo lectura 
            // propiedad: array Wheels
            // campo privado: List Wheels
            foreach (Wheel wheel in vehicle.Wheels)
            {
                Assert.IsTrue(wheel.Pressure == 2.4);
            }
        }

        [TestMethod]
        public void cannot_create_the_same_vechicle_twice()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(2);
            builder.setEngine(100);
            builder.setColor(CarColor.Red);

            Vehicle vehicle1 = builder.build();
            Vehicle vehicle2 = builder.build();

            Assert.AreNotEqual(vehicle1.Enrollment, vehicle2.Enrollment);
        }

        [TestMethod]
        public void cannot_add_more_than_4_wheels()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            try
            {
                builder.addWheel();
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

        [TestMethod]
        public void cannot_create_vehicle_without_wheels()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            try
            {
                Vehicle vehicle = builder.build();
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

        [TestMethod]
        [TestCategory("Long execution time")]
        public void every_enrollment_must_be_unique_SMALL()
        {
            buildMassiveVehicles(SMALL, new TimeSpan(0, 1, 0));
        }

        [TestMethod]
        [TestCategory("Long execution time")]
        public void every_enrollment_must_be_unique_MEDIUM()
        {
            buildMassiveVehicles(MEDIUM, new TimeSpan(0, 2, 0));
        }

        [TestMethod]
        [TestCategory("Long execution time")]
        public void every_enrollment_must_be_unique_LARGE()
        {
            buildMassiveVehicles(LARGE, new TimeSpan(0, 3, 0));
        }

        [TestMethod]
        public void enrollment_must_be_always_the_same()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(2);
            builder.setEngine(100);
            builder.setColor(CarColor.Red);

            Vehicle vehicle = builder.build();
            string enrollment1 = vehicle.Enrollment;
            string enrollment2 = vehicle.Enrollment;
            Assert.AreEqual(enrollment1, enrollment2);
        }

        private static void buildMassiveVehicles(int numberOfVehicles, TimeSpan maxTime)
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(enrollmentProvider);
            IDictionary<string, Vehicle> vehicles = new Dictionary<string, Vehicle>();

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(2);
            builder.setEngine(100);
            builder.setColor(CarColor.Red);

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < numberOfVehicles; i++)
            {
                Vehicle vehicle = builder.build();

                Assert.IsFalse(vehicles.ContainsKey(vehicle.Enrollment));
                vehicles.Add(vehicle.Enrollment, vehicle);

                Assert.IsTrue(stopwatch.Elapsed < maxTime);
            }
        }

    }
}
