using System;
using System.Collections.Generic;
using CarManagement.Builders;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private Engine engine;
        private List<Door> doorList;
        private List<Wheel> wheelList;

        private readonly string enrollment;

        public Vehicle(VehicleBuilder referenceVehicle)
        {
            throw new NotImplementedException();
        }

        public int DoorsCount
        {
            get
            {
                return doorList.Count;
            }
        }

        public int WheelCount
        {
            get
            {
                return wheelList.Count;
            }
        }

        public Engine Engine
        {
            get
            {
                return new Engine(this.engine);
            }
        }

        public string Enrollment
        {
            get
            {
                return this.enrollment;
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        public void SetWheelsPressure(double pressure)
        {
            foreach( Wheel wheel in this.wheelList)
            {
               wheel.pressure = pressure;
            }
        }
    }
}