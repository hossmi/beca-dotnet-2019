using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Door> doors = new List<Door>();
        private List<Wheel> wheels = new List<Wheel>();

        public Door[] Door
        {
            get
            {
                
                
            }
        }

        public int DoorsCount
        {
            get
            {
                return DoorsCount;
            }
        }

        public int WheelCount
        {
            get
            {
                return WheelCount;
            }
        }

        public Engine Engine
        {
            get
            {
                return Engine;
            }
            set
            {
                Engine = value;
            }
        }

        public string Enrollment
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The enrollment can not be empty or have blank spaces.");
                else
                    Enrollment = value;
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