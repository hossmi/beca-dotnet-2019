﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class alloretTestsFunctional
    {
        private readonly alloretArrayVehicleStorage vehicleStorage;

        public alloretTestsFunctional()
        {
            this.vehicleStorage = new alloretArrayVehicleStorage();
        }

        [TestMethod]
        public void minimal_horsepower_of_pressure_value_wheel_3_and_started_engine_cars_is_85cv()
        {
            int horsePower = 0;
            double pressure = 0;
            
              /* Insert code here for boom! */
               
            Assert.AreEqual(3.0, pressure);
            Assert.AreEqual(85, horsePower);
        }

        [TestMethod]
        public void get_wheels_and_pressure_value_grouping_by_enrollment_serial_ordering_by_wheels_number_and_pressure_value()
        {
            var vehicles = this.vehicleStorage

                .getAll()
                .Select(vehicle => new //solo para que compile el test
                {
                    vehicle.Wheels,
                    Pressure = 0
                })
                /* Insert code here for boom! */
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);

            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);

            Assert.AreEqual(2, vehicles[0].Wheels);
            Assert.AreEqual(2.5, vehicles[0].Pressure);
            Assert.AreEqual(4, vehicles[1].Wheels);
            Assert.AreEqual(2.8, vehicles[1].Pressure);
            Assert.AreEqual(2, vehicles[2].Wheels);
            Assert.AreEqual(2.1, vehicles[2].Pressure);
        }


         [TestMethod]
        public void from_the_two_red_cars_with_opened_doors_get_pressure_value_and_engine_status()
        {
            var vehicles = this.vehicleStorage

                .getAll()
                /* Insert code here for boom! */
                .ToArray();

            Assert.AreEqual(2, vehicles.Length);
            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);


        }
    }
}