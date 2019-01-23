using System;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private 

        public int DoorsCount
        {
            get
            {
                return DoorsCount;
            }

            set
            {
                if (value < 1)
                    throw new ArgumentException("Minimum doors value is 1.");
                else
                    DoorsCount = value;
            }
        }

        public int WheelCount
        {
            get
            {
                return WheelCount;
            }
            set
            {
                WheelCount = value;
            }
        }

        public Engine Engine
        {
            get
            {
                throw new NotImplementedException();
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