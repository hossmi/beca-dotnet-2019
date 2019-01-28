using CarManagement.Models;

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
            this.letters = "BCDFGHJKLMNPRSTVWXYZ";//20 dígitos
        }

        IEnrollment IEnrollmentProvider.getNewEnrollment()
        {
            if (number4 <= 9999)
            {
                number4++;
            }
            else
            {
                number4 = 0;
                if (number3 <= 19)
                {
                    number3++;
                }
                else
                {
                    number4 = 0;
                    number3 = 0;
                    if (number2 <= 19)
                    {
                        number2++;
                    }
                    else
                    {
                        number4 = 0;
                        number3 = 0;
                        number2 = 0;
                        if (number1 <= 19)
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

            return new Enrollment(lettersEnrollment,  number4);
        }
    }
}