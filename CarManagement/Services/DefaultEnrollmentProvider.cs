using CarManagement.Builders;
using CarManagement.Models;
using System;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {

        private const char INITIAL_LETTER = 'B';
        static int number = 0;
        static char Letter1 = INITIAL_LETTER;
        static char Letter2 = INITIAL_LETTER;
        static char Letter3 = INITIAL_LETTER;

        private class Enrollment : IEnrollment
        {
            public Enrollment(string serial, int number)
            {
                this.Serial = serial;
                this.Number = number;
            }

            public string Serial { get; }
            public int Number { get; }

            public string Print()
            {
                return $"{this.Serial}-{this.Number.ToString("0000")}";
            }
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            IEnrollment e2 = new Enrollment(serial, number);

            return e2;
        }

        IEnrollment IEnrollmentProvider.getNew()
        {
            if (number >= 10000)
            {
                number = 0;
                Letter3++;
                if (!IsValidLetter(Letter3))
                    Letter3++;
                if (Letter3 > 'Z')
                {
                    Letter3 = INITIAL_LETTER;
                    Letter2++;
                    if (!IsValidLetter(Letter2))
                        Letter2++;
                    if (Letter2 > 'Z')
                    {
                        Letter2 = INITIAL_LETTER;
                        Letter1++;
                        if (!IsValidLetter(Letter1))
                            Letter1++;
                        Asserts.isTrue(Letter1 <= 'Z', "Alcanzado el limite maximo de matriculas");
                    }
                }
            }

            IEnrollment enrollment = new Enrollment(Letter1.ToString() + Letter2.ToString() + Letter3.ToString(), number);
            number++;

            return enrollment;
        }

        private bool IsValidLetter(char letter)
        {
            string allowedLetters = "BCDFGHJKLMNPRSTVWXYZ";
            return allowedLetters.Contains(letter.ToString());
        }
    }
}
