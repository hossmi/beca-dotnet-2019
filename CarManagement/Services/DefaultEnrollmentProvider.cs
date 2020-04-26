using CarManagement.Core.Models;
using CarManagement.Core.Services;
using System;
using System.Text;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int counter;
        private int i;
        private int i2;
        private int i3;
        private int total;
        public DefaultEnrollmentProvider()
        {
            this.counter = 0;
            this.i = 0;
            this.i2 = 0;
            this.i3 = 0;
            this.total = 9999;
        }

        private class Enrollment : IEnrollment
        {
            public Enrollment(string serial, int number)
            {
                this.Serial = serial;
                this.Number = number;
            }

            public string Serial { get; }
            public int Number { get; }

            public override string ToString()
            {
                return $"{this.Serial}-{this.Number:0000}";
            }
        }

        IEnrollment IEnrollmentProvider.getNew()
        {
            
            StringBuilder Serial = new StringBuilder(3);
            StringBuilder A = new StringBuilder("BCDFGHJKLMNPRSTUVWXYZ");
            int lenght = A.Length - 1;

            Serial.Insert(0, $"{(A[this.i])}{(A[this.i2])}{(A[this.i3])}");
            if (this.i <= lenght)
            {
                if (this.counter == this.total)
                {
                    if (this.i2 >= lenght && this.i3 == lenght)
                    {
                        i2 = 0;
                        i3 = 0;
                        i++;
                    }
                    if (this.i3 == lenght)
                    {
                        i3 = 0;
                        i2++;
                    }
                    else
                    {
                        this.i3++;
                        this.counter = 0;
                    }
                }
                else
                    this.counter++;
            }
            else
                Console.WriteLine("Has alcanzado el máximo de matrículas");
            Enrollment enrollment = new Enrollment(Serial.ToString(), this.counter);
            return enrollment;
        }
        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }
    }
}