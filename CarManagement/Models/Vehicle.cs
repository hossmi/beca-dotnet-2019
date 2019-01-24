using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
<<<<<<< HEAD
        private int doorsCount = 5;
        private int wheelsCount = 4;
=======
        private List<Wheel> wheels;

>>>>>>> develop
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

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
        }

        public void SetWheelsPressure(double pression)
        {
            throw new NotImplementedException();
        }
    }
}