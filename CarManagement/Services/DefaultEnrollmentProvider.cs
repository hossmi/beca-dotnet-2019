namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        const char FIRSTCHAR = 'A';
        const char LASTCHAR = 'Z';

        private static int lastIssuedNumber = 0;
        private static char lastIssuedKeyLetter0 = FIRSTCHAR;
        private static char lastIssuedKeyLetter1 = FIRSTCHAR;

        static public string LastIssuedEnrollment
        { get => $"ALC-{lastIssuedNumber}-{lastIssuedKeyLetter1}{lastIssuedKeyLetter0}"; }
        string IEnrollmentProvider.getNewEnrollment()
        {
            if(lastIssuedNumber > 9999)
            {
                lastIssuedNumber = lastIssuedNumber % 10000;

                if (lastIssuedKeyLetter0 == LASTCHAR)
                {
                    lastIssuedKeyLetter0 = FIRSTCHAR;
                    if (lastIssuedKeyLetter1 != LASTCHAR)
                        lastIssuedKeyLetter1++;
                    else
                        throw new System.NotSupportedException
                            ("Number of enrollments issued reached the limit.");
                }
                else
                    lastIssuedKeyLetter0++;
            }

            return $"ESP-{lastIssuedNumber++}-{lastIssuedKeyLetter1}{lastIssuedKeyLetter0}";
        }
    }
}