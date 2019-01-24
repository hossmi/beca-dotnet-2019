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
        private string enrollment;

        public Vehicle()
        {
            generateEnrollment();
        }

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
                return enrollment;
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        private void generateEnrollment()
        {
            string e;
            Random rnd = new Random();
            string enA = "";
            string enB = "";
            int number;
            char c;

            System.Threading.Thread.Sleep(40);
            number = rnd.Next(0, 9999);

            if (number < 10)
            {
                enB = "000" + number.ToString();
            }
            else if (number < 100)
            {
                enB = "00" + number.ToString();
            }
            else if (number < 1000)
            {
                enB = "0" + number.ToString();
            }
            else
            {
                enB = number.ToString();
            }

            for (int i = 0; i < 3; i++)
            {
                number = rnd.Next(0, 26);
                c = (char)('A' + number);
                enA = enA + c;
            }
            e = enA + "-" + enB;

            enrollment = e;

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