namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        const char FIRSTCHAR = 'A';
        const char LASTCHAR = 'Z';

        private static int nextIssuedNumber = 0;
        private static char[] lastIssuedLetters = new char[] {FIRSTCHAR, FIRSTCHAR};

        static public string LastIssuedEnrollment
        { get => $"ALC-{nextIssuedNumber-1}-{lastIssuedLetters[1]}{lastIssuedLetters[0]}"; }
        string IEnrollmentProvider.getNewEnrollment()
        {
            if(nextIssuedNumber > 9999)
            {
                nextIssuedNumber = nextIssuedNumber % 10000;

                if (lastIssuedLetters[0] == LASTCHAR)
                {
                    lastIssuedLetters[0] = FIRSTCHAR;
                    if (lastIssuedLetters[1] != LASTCHAR)
                        lastIssuedLetters[1]++;
                    else
                        throw new System.NotSupportedException
                            ("Number of enrollments issued reached the limit.");
                }
                else
                    lastIssuedLetters[0]++;
            }

            return $"A{lastIssuedLetters[1]}{lastIssuedLetters[0]}-{nextIssuedNumber++}";
        }
    }
}