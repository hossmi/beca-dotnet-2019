using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Wheel> wheels = new List<Wheel>();
        private List<Door> doors = new List<Door>();
        private Engine engine = new Engine();
        private CarColor color;


        public List<Wheel> carwheel
        {
            set
            {
                wheels = value;
            }
        }
        public List<Door> cardoor
        {
            set
            {
                doors = value;
            }
        }
        public Engine carengine
        {
            set
            {
                engine = value;
            }
        }
        public CarColor carcolor
        {
            set
            {
                color = value;
            }
        }
        public int DoorsCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int WheelCount
        {
            get
            {
                throw new NotImplementedException();
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

        public Door[] Doors
        {
            get
            {
                return this.doors.ToArray();
            }
        }


        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
        }

        public void setWheelsPressure(double pression)
        {
            throw new NotImplementedException();
        }
    }
}