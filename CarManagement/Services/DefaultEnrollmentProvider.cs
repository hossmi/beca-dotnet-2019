using CarManagement.Builders;
using CarManagement.Models;
using System.Diagnostics;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        const char firstPossibleChar = 'B';
        const char lastPossibleChar = 'Z';
        const int lastValidChar = 20;
        private readonly char[] validLetterSequence = 
            {firstPossibleChar,'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N',
            'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', lastPossibleChar};

        private static int[] lastLettersTracker = new int[3];
        private static int nextIssuedNumber = 0;

        public IEnrollment getNewEnrollment()
        {
            if(nextIssuedNumber > 9999)
            {
                nextIssuedNumber = 0;

                if(++lastLettersTracker[0] > lastValidChar)
                {
                    lastLettersTracker[0] = 0;

                    if (++lastLettersTracker[1] > lastValidChar)
                    {
                        lastLettersTracker[1] = 0;

                        Asserts.isTrue(++lastLettersTracker[2] < lastValidChar,"Limit of enrollments reached");
                    }
                }
            }

            string toIssuedSerial = $"{validLetterSequence[lastLettersTracker[2]]}{validLetterSequence[lastLettersTracker[1]]}{validLetterSequence[lastLettersTracker[0]]}";
            
            //Debug.WriteLine(toIssuedSerial);

            return new Enrollment(nextIssuedNumber++, toIssuedSerial);
        }
    }
}