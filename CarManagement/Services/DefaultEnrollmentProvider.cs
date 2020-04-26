using CarManagement.Core.Models;
using CarManagement.Core.Services;
using System;
using System.Text;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private StringBuilder A;
        private StringBuilder Serial;
        private int lenght;
        private int Number;
        private int i;
        private int i2;
        private int i3;
        private int total;
        public DefaultEnrollmentProvider()
        {
            this.A = new StringBuilder("BCDFGHJKLMNPRSTUVWXYZ");
            this.Serial = new StringBuilder(3);
            this.lenght = A.Length - 1;
            this.Number = 0;
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
            this.Serial.Insert(0, $"{(this.A[this.i])}{(this.A[this.i2])}{(this.A[this.i3])}");
            if (this.i <= this.lenght)
            {
                if (this.Number == this.total)
                {
                    if (this.i2 >= this.lenght && this.i3 == this.lenght)
                    {
                        this.i2 = 0;
                        this.i3 = 0;
                        this.i++;
                    }
                    if (this.i3 == lenght)
                    {
                        this.i3 = 0;
                        this.i2++;
                    }
                    else
                    {
                        this.i3++;
                        this.Number = 0;
                    }
                }
                else
                    this.Number++;
            }
            else
                Console.WriteLine("Has alcanzado el máximo de matrículas");
            Enrollment enrollment = new Enrollment(Serial.ToString(), this.Number);
            return enrollment;
        }
        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }
    }
}