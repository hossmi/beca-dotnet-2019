using System;
using System.Collections.Generic;
using CarManagement.Models.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private Engine engine;
        private List<Door> doorList;
        private List<Wheel> wheelList;

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
                throw new NotImplementedException();
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        public void SetWheelsPressure(double pression)
        {
            throw new NotImplementedException();
        }
    }
}