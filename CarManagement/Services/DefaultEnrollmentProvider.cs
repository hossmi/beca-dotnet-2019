using CarManagement.Core.Models;
using CarManagement.Core.Services;
using System;
using System.Text;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
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
                return $"{this.Serial}-{this.Number.ToString("0000")}";
            }
        }
        IEnrollment IEnrollmentProvider.getNew()
        {
            int Number = 0;
            StringBuilder Serial = new StringBuilder(3);
            StringBuilder A = new StringBuilder("BCDFGHJKLMNPRSTUVWXYZ", 21);
            int lenght = A.Length - 1;
            int i = 0;
            int i2 = 0;
            int i3 = 0;
            int total = 9999;

            Serial.Insert(0, $"{(A[i])}{(A[i2])}{(A[i3])}");
            if (i <= lenght)
            {

                if (Number == total)
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
                        Number = 0;
                    }
                }
                else
                {
                    Number++;
                }
            }
            else
            {
                Console.WriteLine("Has alcanzado el máximo de matrículas");
            }
            Enrollment enrollment = new Enrollment(Serial.ToString(), Number);
            return enrollment;
        }

        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }
    }
}