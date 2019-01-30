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
            this.tamLetterAccept = this.lettersacept.Length -1;
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

        IEnrollment IEnrollmentProvider.getNew()
        {
            if (this.number < 9999)
            {
                this.number++;
            }
            else
            {
                this.number = 0;
                if (this.letter3 < this.tamLetterAccept)
                {
                    this.letter3++;
                }
                else
                {
                    this.number = 0;
                    this. letter3 = 0;
                    if (this.letter2 < this.tamLetterAccept)
                    {

                        this.letter2++;
                    }
                    else
                    {
                        this.number = 0;
                        this.letter3 = 0;
                        this.letter2 = 0;
                        if (this.letter1 < this.tamLetterAccept)
                        {
                            this.letter1++;
                        }
                        else
                        {
                            throw new ArgumentException("Superado límite de matrículas");
                        }
                    }
                }
            }
            //string serial = ;
            return new Enrollment(this.lettersacept[this.letter3].ToString() + this.lettersacept[this.letter2].ToString() + this.lettersacept[this.letter1].ToString(), this.number);
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }
    }
}