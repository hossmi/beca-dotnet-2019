using System;
using System.Text;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private const int TAM_ARRAYS = 4;

        private char[] letters =
        {
            '0', 'A', 'A', 'A'
        };
        private char[] numbers =
        {
            '0', '0', '0', '0'
        };
        
        string IEnrollmentProvider.getNewEnrollment()
        {
            string result = "";

            result = generateLetter() + "-" + generateNumber();            

            return result;
        }

        private string generateLetter()
        {
            string lettersAux;

            for (int i = TAM_ARRAYS - 1; i > 0; i--)
            {
                if(letters[i] == '0')
                {
                    letters[i] = 'A';
                    lettersAux = new string(letters);
                    return lettersAux;
                }

                if (letters[i] < 'Z')
                {
                    letters[i]++;
                    lettersAux = new string(letters);
                    return lettersAux;
                }
                else if(letters[i] == 'Z')
                {
                    letters[i] = 'A';
                }
            }
            lettersAux = new string(letters);
            return lettersAux;
        }

        private string generateNumber()
        {
            string numbersAux;

            for (int i = TAM_ARRAYS - 1; i > 0; i--)
            {
                if (numbers[i] < '9')
                {
                    numbers[i]++;
                    numbersAux = new string(numbers);
                    return numbersAux;
                }
                else if (numbers[i] == '9')
                {
                    numbers[i] = '0';
                }
            }
            numbersAux = new string(numbers);
            return numbersAux;
        }

    }
}