using System;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private int doorsCount = 5;
        private int wheelsCount = 4;
        public int DoorsCount
        {
            get
            {
                return doorsCount;
            }
        }

        public int WheelCount
        {
            get
            {
                return wheelsCount;
            }
        }

        public Engine Engine
        {

            get
            {

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