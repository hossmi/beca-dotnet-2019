using CarManagement.Models;
using System;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int numbers;
        private readonly string[] letters = { "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "R", "S", "T", "V", "W", "X", "Y", "Z" };
        private readonly int[] serial = { 0, 0, 0 };

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

        public DefaultEnrollmentProvider()
        {
            this.numbers = 0;
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            throw new System.NotImplementedException();
        }

        IEnrollment IEnrollmentProvider.getNew()
        {
            string enrollmentLetters = "";

            if (this.numbers >= 9999)
            {
                if ((this.serial[0] & this.serial[1] & this.serial[2]) < 19)
                {
                    if ((this.serial[1] & this.serial[2]) < 19)
                    {
                        if (this.serial[2] < 19)
                        {
                            this.serial[2] += 1;
                            this.numbers = 0000;
                        }
                        else
                        {
                            this.serial[1] += 1;
                            this.serial[2] = 0;
                            this.numbers = 0000;
                        }
                    }
                    else
                    {
                        this.serial[0] += 1;
                        this.serial[1] = 0;
                        this.serial[2] = 0;
                        this.numbers = 0000;
                    }
                }
                else
                {
                    throw new SystemException("Unexpected Error in generateEnrollment");
                }
            }
            else
            {
                this.numbers += 1;
            }

            enrollmentLetters = this.letters[this.serial[0]] + this.letters[this.serial[1]] + this.letters[this.serial[2]];
            return new Enrollment(enrollmentLetters, this.numbers);
        }

    }

}