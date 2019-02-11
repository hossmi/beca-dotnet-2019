﻿using CarManagement.Extensions.Vehicles;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarManagement.Services;
using BusinessCore.Tests.Services;
using BusinessCore.Tests.Models;
using System.Linq;
using System;

namespace BusinessCore.Tests
{
    [TestClass]
    public class EpicSimpleVehicleTests
    {
        private readonly EpicVehicleStorage vehicleStorage;

        public EpicSimpleVehicleTests()
        {
            this.vehicleStorage = new EpicVehicleStorage();
        }

        [TestMethod]
        public void Vehicle_entities_shall_not_share_wheels()
        {
            DefaultEnrollmentProvider provider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(provider);

            builder.addWheel();
            builder.setDoors(3);
            builder.setEngine(12);

            IVehicle vehicle0 = builder.build();
            IVehicle vehicle1 = builder.build();

            vehicle0.setWheelsPressure(3.2);
            vehicle1.setWheelsPressure(1.1);

            Assert.IsFalse(vehicle0.Wheels[0].Pressure == vehicle1.Wheels[0].Pressure);
        }
        [TestCategory("Functional Programing")]
        [TestMethod]
        public void Find_vehicle_enrollmment_with_most_powerful_engine_and_2_wheels()
        {
            IEnrollment querriedEnrollment = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Wheels.Count() == 2)
                .Select( Vehicle => new
                {
                    order = Vehicle.Engine.HorsePower,
                    Enrollment = Vehicle.Enrollment,
                })
                .OrderByDescending( aninimo =>  aninimo.order)
                .First().Enrollment;
            Assert.AreEqual("ABC", querriedEnrollment.Serial);
            Assert.AreEqual(1, querriedEnrollment.Number);
        }

        [TestCategory("Functional Programing")]
        [TestMethod]
        public void Find_vehicle_enrollment_with_one_wheel_with_more_pressure_than_the_others()
        { // Vehicle has to have more than 1 wheel, and the greatest pressure is only present in one wheel
            
            IEnrollment[] querriedEnrollment = this.vehicleStorage
                .get()
                .Where(vehicle => (vehicle.Wheels
                        .GroupBy( wheel => wheel.Pressure )
                        .OrderByDescending( group => group.Key)
                        .First()
                        .Count() == 1)
                        &&
                        (vehicle.Wheels.Count() >1))
                .Select( vehicle => vehicle.Enrollment)
                .ToArray();
            try
            {
                int a = querriedEnrollment[1].Number;
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
                //good
            }
            Assert.AreEqual("AAA", querriedEnrollment[0].Serial);
            Assert.AreEqual(1000, querriedEnrollment[0].Number);
        }
    }
}
