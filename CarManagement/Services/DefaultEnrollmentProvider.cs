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
        private string lettersacept;
        private int tamLetterAccept;

        public DefaultEnrollmentProvider()
        {
            this.letter1 = 0;
            this.letter2 = 0;
            this.letter3 = 0;
            this.number = -1;
            this.lettersacept = "BCDFGHJKLMNPRSTVWXYZ"; //20
            this.tamLetterAccept = this.lettersacept.Length - 1;
        }
        IEnrollment IEnrollmentProvider.getNewEnrollment()
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
                    this.letter3 = 0;
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
            string serial = string.Format("{0}{1}{2}", this.lettersacept[this.letter3], this.lettersacept[this.letter2], this.lettersacept[this.letter1]);
            return new Enrollment(serial, this.number);
        }
    }
}