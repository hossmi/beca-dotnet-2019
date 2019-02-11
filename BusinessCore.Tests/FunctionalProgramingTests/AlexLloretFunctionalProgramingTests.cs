using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    [TestCategory("Functional Programing")]
    public class AlexLloretFunctionalProgramingTests
    {
        private readonly alloretArrayVehicleStorage vehicleStorage;

        public AlexLloretFunctionalProgramingTests()
        {
            this.vehicleStorage = new alloretArrayVehicleStorage();
        }

        [TestMethod]
        public void minimal_horsePower_for_vehicles_with_wheel_that_have_three_atmospheres_of_pressure_is_85cv()
        {
            //mínimo cv de los vehículos con ruedas con 3 de presión
            var pressure = 3.0;
            var horsePower = this.vehicleStorage
                .get()
                .Where(vehicle => vehicle.Wheels.All(wheel => wheel.Pressure == pressure))
                .Min(vehicle => vehicle.Engine.HorsePower);
               
            Assert.AreEqual(3.0, pressure);
            Assert.AreEqual(85, horsePower);
        }

        [TestMethod]
        public void get_wheels_and_pressure_value_grouping_by_enrollment_serial_ordering_by_wheels_number_and_pressure_value()
        {
            var vehicles = this.vehicleStorage
                .get()
                .GroupBy(vehicle => vehicle.Enrollment.Serial)
                .Select(vehicleGroup => new //solo para que compile el test
                {
                    WheelsCount = vehicleGroup.Sum(v => v.Wheels.Length),
                    Pressure = vehicleGroup.SelectMany(v => v.Wheels).Average(w => w.Pressure),
                })
                .OrderBy(a => a.WheelsCount)
                .ThenBy(a => a.Pressure)
                .ToArray();

            Assert.AreEqual(3, vehicles.Length);

            Type itemType = vehicles[0].GetType();
            Assert.AreEqual(2, itemType.GetProperties().Length);

            Assert.AreEqual(3, vehicles[0].WheelsCount);
            Assert.AreEqual(6, vehicles[0].Pressure);
            Assert.AreEqual(5, vehicles[1].WheelsCount);
            Assert.AreEqual(3.6, vehicles[1].Pressure);
            Assert.AreEqual(14, vehicles[2].WheelsCount);
            Assert.AreEqual(3.0428571428571423, vehicles[2].Pressure);
        }


         [TestMethod]
        public void from_the_two_red_cars_with_opened_doors_get_pressure_value_and_engine_status()
        {
            var vehicles = this.vehicleStorage

                .get()
                .Where( vehicle => vehicle.Color == CarColor.Red)
                .Where( vehicle => vehicle.Doors.Count( door => door.IsOpen == true) >=1)
                .Select(vehicle => new
                {
                    pressure = vehicle.Wheels[0].Pressure,
                    engine_status = vehicle.Engine.IsStarted 
                })
                .ToArray();

            Assert.AreEqual(2, vehicles.Length);
            Type itemTime = vehicles[0].GetType();
            Assert.AreEqual(2, itemTime.GetProperties().Length);


        }
    }
}
