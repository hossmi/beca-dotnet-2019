using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Extensions.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestCategory("Functional Programing")]
    [TestClass]
    public class FunctionalProgramingTests
    {
        private readonly ArrayVehicleStorage vehicleStorage;

        public FunctionalProgramingTests()
        {
            this.vehicleStorage = new ArrayVehicleStorage();
        }

        [TestMethod]
        public void there_are_six_black_vehicles()
        {
            IVehicle[] vehicles = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Color == CarColor.Black)
                .ToArray();
            Assert.AreEqual(6, vehicles.Length);
        }

        [TestMethod]
        public void there_are_four_started_engines()
        {
            IEnumerable<IEngine> engines = this.vehicleStorage
                .get()
                .Select(vehicle => vehicle.Engine)
                .Where(engine => engine.IsStarted == true);

            Assert.AreEqual(4, engines.Count());
        }

        [TestMethod]
        public void average_pressure_for_white_vehicles_is_3()
        {
            double pressure = 0.0;
            pressure = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Color == CarColor.White)
                .Select(vehicle => vehicle.Wheels)
                .Average(wheel => wheel.Average(w => w.Pressure));

            Assert.AreEqual(3.0, pressure);
        }

        [TestMethod]
        public void minimal_horsepower_is_100cv()
        {
            int horsePower = 0;
            horsePower = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Engine.HorsePower >= 100)
                .Select(v => v.Engine.HorsePower)
                .Min(horse => horse);
                
            Assert.AreEqual(100, horsePower);
        }

        [TestMethod]
        public void maximal_horsepower_of_red_colored_and_stopped_cars_is_666cv()
        {
            int horsePower = 0;
            horsePower = this.vehicleStorage
                .get()
                .Where(condition1 => condition1.Color == CarColor.Red)
                .Where(condition2 => condition2.Engine.IsStarted == false)
                .Where(condition3 => condition3.Engine.HorsePower <= 666)
                .select(selection => selection.Engine.HorsePower)
                .Max(horse => horse);

            Assert.AreEqual(666, horsePower);
        }

        [TestMethod]
        public void from_the_two_white_cars_with_opened_doors_get_serial_enrollment_and_horsePower()
        {
            var vehicles = this.vehicleStorage
                .get()
                .Where(condition1 => condition1.Color == CarColor.White)
                .Where(Condition2 => Condition2.Doors.Any(door => door.IsOpen))
                .select(selection => 
                    new
                        {
                            serial = selection.Enrollment.Serial,
                            horse = selection.Engine.HorsePower
                        }
                     )
            .ToArray();

            Assert.AreEqual(2, vehicles.Length);
            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);
        }


        [TestMethod]
        public void get_enrollment_serial_and_average_horse_power_grouping_by_enrollment_serial_ordering_by_serial_and_average_horse_power()
        {
            var vehicles = this.vehicleStorage
                .get()
                .GroupBy(group => group.Enrollment.Serial)
                .Select(groupvehicle => 
                new
                    {
                        Serial = groupvehicle.Key,
                        AverageHorsePower = groupvehicle.Average(vehicle => vehicle.Engine.HorsePower)
                    }
                )
                .OrderBy(order1 => order1.Serial)
                .ThenBy(order2 => order2.AverageHorsePower)
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);

            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);

            Assert.AreEqual("JVC", vehicles[0].Serial);
            Assert.AreEqual(622, vehicles[0].AverageHorsePower);
            Assert.AreEqual("PNG", vehicles[1].Serial);
            Assert.AreEqual(633, vehicles[1].AverageHorsePower);
            Assert.AreEqual("ZZZ", vehicles[2].Serial);
            Assert.AreEqual(539.6, vehicles[2].AverageHorsePower);
            

        }


        [TestMethod]
        public void get_horsePower_of_green_vehicles_or_get_12354645_as_default()
        {
            var horsePowers = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Color == CarColor.Green)
                .Select(vehicle => vehicle.Engine.HorsePower)
                .DefaultIfEmpty(12354645)
                .ToArray();

            Assert.AreEqual(1, horsePowers.Length);
            Assert.AreEqual(12354645, horsePowers[0]);


        }
    }
}
