using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Extensions.Vehicles;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    [TestCategory("Soden")]
    public class SodenFunctionalProgramingTests
    {
        private readonly ArrayVehicleStorageSoden vehicleStorage;

        public SodenFunctionalProgramingTests()
        {
            this.vehicleStorage = new ArrayVehicleStorageSoden();
        }

        [TestMethod]
        [TestCategory("Soden")]
        [TestCategory("Functional Programing")]
        public void a_there_are_3_black_vehicles_and_horsePorwer_min100_and_started()
        {
            IVehicle[] vehicles = this.vehicleStorage
                .getAll()
                /**/
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);
        }

        [TestMethod]
        [TestCategory("Soden")]
        [TestCategory("Functional Programing")]
        public void b_there_are_two_started_engines_more_one_doors_closed()
        {
            IEnumerable<IEngine> engines = this.vehicleStorage
                .getAll()
                /**/
                .Select(vehicle => vehicle.Engine);
            Assert.AreEqual(2, engines.Count());
        }

        [TestMethod]
        [TestCategory("Soden")]
        [TestCategory("Functional Programing")]
        public void get_sum_number_of_wheels_of_black_vehicles_with_enrollment_number_higher_to_100_is_6()
        {
            double pressure = this.vehicleStorage
               .getAll()
               /**/
               .Count();

                Assert.AreEqual(6, pressure);
        }

        [TestMethod]
        [TestCategory("Soden")]
        [TestCategory("Functional Programing")]
        public void get_serial_enrollment_from_white_vehicles_with_at_least_one_door_and_horsePower_adobe_500cv()
        {
            var vehicles = this.vehicleStorage
                .getAll()
                /**/
                .Select(vehicle => new { vehicle.Enrollment.Serial})
                .ToArray();

            Assert.AreEqual("PNG", vehicles[0].Serial);
        }

        [TestMethod]
        [TestCategory("Soden")]
        [TestCategory("VehicleBuilder")]
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

    }
}
