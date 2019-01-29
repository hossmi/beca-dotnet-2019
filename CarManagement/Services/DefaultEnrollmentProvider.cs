using CarManagement.Builders;
using CarManagement.Models;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        IEnrollment IEnrollmentProvider.import(string enrollment)
        {
            throw new System.NotImplementedException();
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
