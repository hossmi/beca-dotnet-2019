using CarManagement.Models;

using System;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {

        private char[] validSerialLetters = new char[]
        { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N',
            'P', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };

        private int FirstLetterIndexCount = 0;
        private int SecondLetterIndexCount = 0;
        private int ThirdLetterIndexCount = 0;
        private int numberCount = 0;

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
        IEnrollment IEnrollmentProvider.getNew()
        {
            throw new System.NotImplementedException();
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            IEnrollment enrollment;
            string serial = "";
            int number = 0;

            if (numberCount == 10000)
            {
                numberCount = 0;
                ThirdLetterIndexCount++;
            }
            if (ThirdLetterIndexCount == validSerialLetters.Length)
            {
                ThirdLetterIndexCount = 0;
                SecondLetterIndexCount++;
            }
            if (SecondLetterIndexCount == validSerialLetters.Length)
            {
                SecondLetterIndexCount = 0;
                FirstLetterIndexCount++;
            }
            if (FirstLetterIndexCount == validSerialLetters.Length)
            {
                throw new Exception("Se ha alcanzado el número máximo de matrículas generadas.");
            }

            serial += validSerialLetters[FirstLetterIndexCount];
            serial += validSerialLetters[SecondLetterIndexCount];
            serial += validSerialLetters[ThirdLetterIndexCount];
            number = numberCount;

            enrollment = new Enrollment(serial, number);
            return enrollment;
        }
    }
}