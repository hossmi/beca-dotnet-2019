using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Door> doors;
        private List<Wheel> wheels;
        private Engine engine;
        private CarColor color;
        private string enrollmentA;
        private int enrollmentB;

        public Door[] Doors
        {
            get
            {
                return doors.ToArray();
            }

        }
        public Wheel[] Wheels
        {
            get
            {
                return wheels.ToArray();
            }

        }


        public int DoorsCount
        {
            get
            {
                return doors.Count;
            }
        }

        public int WheelCount
        {
            get
            {
                return wheels.Count;
            }
        }

        public Engine Engine
        {
            get
            {
                return engine;
            }
        }

        public string Enrollment
        {
            get
            {
                string enrollment;
                Random rnd = new Random();
                string enB = "";

                enrollmentB = rnd.Next(0, 9999);

                switch
                    { }



                enrollment = enrollmentA + "-" + enB;
                return enrollment; 
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        public List<Wheel> SetWheels
        {
            set
            {
                wheels = value;
            }    
        }

        public List<Door> SetDoors
        {
            set
            {
                doors = value;
            }
        }
        public Engine SetEngine
        {
            set
            {
                engine = value;
            }
        }
        public CarColor SetCarColor
        {
            set
            {
                color = value;
            }
        }

        public void SetWheelsPressure(double pression)
        {
            foreach (Wheel w in wheels)
            {
                w.FillWheel(pression);
            }
        }
    }
}