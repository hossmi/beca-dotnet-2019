using CarManagement.Models;

using System;
namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int letter1;
        private int letter2;
        private int letter3;
        private int number;
        private readonly string lettersacept;
        private int tamLetterAccept;
        public DefaultEnrollmentProvider()
        {
            this.letter1 = 0;
            this.letter2 = 0;
            this.letter3 = 0;
            this.number = -1;
            this.lettersacept = "BCDFGHJKLMNPRSTVWXYZ"; //20
            this.tamLetterAccept = lettersacept.Length -1;
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
                return $"{this.Serial}-{this.Number.ToString("0000")}";
            }

        }

        IEnrollment IEnrollmentProvider.getNewEnrollment()
        {
            if (number < 9999)
            {
                number++;
            }
            else
            {
                number = 0;
                if (letter3 < this.tamLetterAccept)
                {
                    letter3++;
                }
                else
                {
                    number = 0;
                    letter3 = 0;
                    if (letter2 < this.tamLetterAccept)
                    {

                        letter2++;
                    }
                    else
                    {
                        number = 0;
                        letter3 = 0;
                        letter2 = 0;
                        if (letter1 < this.tamLetterAccept)
                        {
                            letter1++;
                        }
                        else
                        {
                            throw new ArgumentException("Superado límite de matrículas");
                        }
                    }
                }
            }
            //string serial = ;
            return new Enrollment(lettersacept[letter3].ToString() + lettersacept[letter2].ToString() + lettersacept[letter1].ToString(), number);
        }

    }
}