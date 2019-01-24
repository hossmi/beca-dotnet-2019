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

        private string Abc()
        {
            string A = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int lenght = A.Length;
            int i = 1;
            int i2 = 1;
            int i3 = 1;
            string result = "";
            string result2 = "";
            while (i <= A.Length)
            {

                char letra = A[i];
                result = result + letra;
                i++;
                while (i2 <= A.Length)
                {
                    letra = A[i2];
                    result = result + letra;
                    i2++;
        
            };
                while (i3 <= A.Length)
                {
                    letra = A[i3];
                    result = result + letra;
                    result2 = result;
                    result = "";
                    i3++;

                    };
            };
        }

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