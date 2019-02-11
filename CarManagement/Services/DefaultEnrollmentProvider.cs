using CarManagement.Models;
using System;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private char letter1 = 'A';
        private char letter2 = 'A';
        private char letter3 = 'A';
        private int number = 0;
        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            throw new NotImplementedException();
        }

        IEnrollment IEnrollmentProvider.getNew()
        {
            if (this.number <= 9999)
            {
                this.number++;
            }
            else
            {
                this.number = 0;
            }

            if (this.letter3 <= 'Z')
            {
                this.letter3++;
            }
            else
            {
                this.letter3 = 'A';
                this.number = 0;
            }

            if (this.letter2 <= 'Z')
            {
                this.letter2++;
            }
            else
            {
                this.letter3 = 'A';
                this.letter2 = 'A';
                this.number = 0;
            }

            if (this.letter1 <= 'Z')
            {
                this.letter1++;
            }
            else
            {
                this.letter3 = 'A';
                this.letter2 = 'A';
                this.letter1 = 'A';
                this.number = 0;
            }

            string serial = $"{this.letter1}{this.letter2}{this.letter3}";



            IEnrollment enrollment = new PrivateEnrollment(serial, this.number);

            return enrollment;
        }

        private class PrivateEnrollment : IEnrollment
        {

            public PrivateEnrollment(string serial, int number)
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
    }
}