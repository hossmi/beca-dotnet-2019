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
        private string letters;
      

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
            this.number1 = -1;
            this.number2 = -1;
            this.number3 = -1;
            this.number4 = -1;
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
                if (number3 <= 0)
                {
                    number3++;
                }
                else
                {
                    number4 = 0;
                    number3 = 0;
                    if (number2 <= 91)
                    {
                        number2++;
                    }
                    else
                    {
                        number4 = 0;
                        number3 = 0;
                        number2 = 0;
                        if (number1 <= 91)
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
            string letters = ((Char)(number1)).ToString() + ((Char)(number2)).ToString() + ((Char)(number3)).ToString();

            return new Enrollment(letters,  number4);
        }
    }
}