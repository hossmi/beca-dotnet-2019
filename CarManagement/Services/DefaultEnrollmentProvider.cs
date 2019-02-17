using CarManagement.Core.Models;
using CarManagement.Core.Services;

using System;
namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int number1;
        private int number2;
        private int number3;
        private int number4;
        private readonly string letters;
        private readonly int sizeLetters;

        private class Enrollment : IEnrollment
        {
            public Enrollment(string serial, int number)
            {
                this.Serial = serial;
                this.Number = number;
            }
            
            public string Serial{get;}
            public int Number{get;}

            public override string ToString()
            {
                return $"{this.Serial}-{this.Number.ToString("0000")}";
            }
        }

        public DefaultEnrollmentProvider()
        {
            this.number1 = 0;
            this.number2 = 0;
            this.number3 = 0;
            this.number4 = 0;
            this.letters = "BCDFGHJKLMNPRSTVWXYZ";
            this.sizeLetters = this.letters.Length-1;
        }

        public DefaultEnrollmentProvider(IEnrollment enrollment)
        {
            this.number1 = 0;
            this.number2 = 0;
            this.number3 = 0;
            this.number4 = 0;
            this.letters = "BCDFGHJKLMNPRSTVWXYZ";
            this.sizeLetters = this.letters.Length - 1;
        }

        IEnrollment IEnrollmentProvider.getNew()
        {
            if (number4 <= 9999)
            {
                number4++;
            }
            else
            {
                number4 = 0;
                if (number3 < this.sizeLetters)
                {
                    number3++;
                }
                else
                {
                    number4 = 0;
                    number3 = 0;
                    if (number2 < this.sizeLetters)
                    {
                        number2++;
                    }
                    else
                    {
                        number4 = 0;
                        number3 = 0;
                        number2 = 0;
                        if (number1 < this.sizeLetters)
                        {
                            number1++;
                        }
                        else
                        {
                            throw new ArgumentException("Superado límite de matrículas");
                        }
                    }
                }
            }
            string lettersEnrollment = this.letters[number1].ToString() + this.letters[number2].ToString() + this.letters[number3].ToString();
            return new Enrollment(lettersEnrollment, number4);
        }

        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }
    }
}