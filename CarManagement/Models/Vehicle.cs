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
            int lenght = A.Length - 1;
            int i = 0;
            int i2 = 0;
            int i3 = 0;
            int numi = 0;
            int numo = 9999;
            while (i <= lenght)
            {
                string salida = "";
                string numi2 = numi.ToString();
                int countnumi = numi2.Length;
                if (countnumi == 1)
                {
                    salida = A[i] + A[i2] + A[i3] + " - 000" + numi;
                }
                else if (countnumi == 2)
                {
                    salida = A[i] + A[i2] + A[i3] + " - 00" + numi;
                }
                else if (countnumi == 3)
                {
                    salida = A[i] + A[i2] + A[i3] + " - 0" + numi;
                }
                else if (countnumi == 4)
                {
                    salida = A[i] + A[i2] + A[i3] + " - " + numi;
                }
                if (numi == numo)
                {
                    if (i2 >= lenght && i3 == lenght)
                    {
                        i2 = 0;
                        i3 = 0;
                        i++;
                    }
                    if (i3 == lenght)
                    {
                        i3 = 0;
                        i2++;
                    }
                    else
                    {
                        i3++;
                        numi = 0;
                    }
                }
                else
                {
                    numi++;
                }
            return salida;

            }
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