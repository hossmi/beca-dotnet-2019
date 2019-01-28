using CarManagement.Builders;
using CarManagement.Models;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        const char FIRSTCHAR = 'A';
        const char LASTCHAR = 'Z';

        private static int nextIssuedNumber = 0;
        public static readonly char locale = 'A';
        private static char[] lastIssuedLetters = new char[] {FIRSTCHAR, FIRSTCHAR};

        static public string LastIssuedEnrollment
        { get => $"{locale}{lastIssuedLetters[1]}{lastIssuedLetters[0]}-{nextIssuedNumber-1}"; }

        IEnrollment IEnrollmentProvider.getNewEnrollment()
        {
            if(nextIssuedNumber > 9999)
            {
                nextIssuedNumber = nextIssuedNumber % 10000;

                if (lastIssuedLetters[0] == LASTCHAR)
                {
                    lastIssuedLetters[0] = FIRSTCHAR;
                    Asserts.isTrue(lastIssuedLetters[1] != LASTCHAR, "Number of enrollments issued reached the limit.");
                    lastIssuedLetters[1]++;
                }
                else
                    lastIssuedLetters[0]++;
            }

            return new Enrollment(nextIssuedNumber++, $"{locale}{lastIssuedLetters[1]}{lastIssuedLetters[0]}");
        }
    }
}