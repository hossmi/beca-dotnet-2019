using System;
using System.Collections.Generic;
using System.Diagnostics;
using CarManagement.Builders;
using CarManagement.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class VehicleTests
    {
        [TestMethod]
        public void builder_default_functionality()
        {
            VehicleBuilder builder = new VehicleBuilder();

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(doorsCount: 2);
            builder.setEngine(horsePorwer: 100);
            builder.setColor(CarColor.Red);

            Vehicle vehicle = builder.build();

            Assert.IsNotNull(vehicle);
            Assert.IsFalse(string.IsNullOrWhiteSpace(vehicle.Enrollment));
            Assert.AreEqual(2, vehicle.DoorsCount);
            Assert.AreEqual(4, vehicle.WheelCount);

            string matricula = vehicle.Enrollment;

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
            vehicle.setWheelsPressure(pression: 2.4);

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
            VehicleBuilder builder = new VehicleBuilder();

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(doorsCount: 2);
            builder.setEngine(horsePorwer: 100);
            builder.setColor(CarColor.Red);

            Vehicle vehicle1 = builder.build();
            Vehicle vehicle2 = builder.build();

            Assert.AreNotEqual(vehicle1.Enrollment, vehicle2.Enrollment);
        }


        [TestMethod]
        public void cannot_add_more_than_4_wheels()
        {
            VehicleBuilder builder = new VehicleBuilder();

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
            VehicleBuilder builder = new VehicleBuilder();

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
        public void every_enrollment_must_be_unique()
        {
            VehicleBuilder builder = new VehicleBuilder();
            IDictionary<string, Vehicle> vehicles = new Dictionary<string, Vehicle>();
            TimeSpan maxTime = new TimeSpan(0, 1, 0);

            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            builder.setDoors(doorsCount: 2);
            builder.setEngine(horsePorwer: 100);
            builder.setColor(CarColor.Red);

            Stopwatch stopwatch = Stopwatch.StartNew();
            do
            {
                Vehicle vehicle = builder.build();

                Assert.IsFalse(vehicles.ContainsKey(vehicle.Enrollment));
                vehicles.Add(vehicle.Enrollment, vehicle);
            } while (stopwatch.Elapsed <= maxTime);
        }

    }
}
