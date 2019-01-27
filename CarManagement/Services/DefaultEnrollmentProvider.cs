using CarManagement.Models;
using CarManagement.Services;
using System;
using System.Text;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
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
                            
        private const int TAM_ARRAYS = 4;

        private char[] letters =
        {
            '0', 'A', 'A', 'A'
        };
        private char[] numbers =
        {
            '0', '0', '0', '0'
        };

        public DefaultEnrollmentProvider()
        {
            this.NewEnrollment = new Enrollment(serial: generateLetter(), number: generateNumber());
        }

        public IEnrollment NewEnrollment { get; }

        IEnrollment IEnrollmentProvider.getNewEnrollment()
        {
            /*string result = "";

            result = generateLetter() + "-" + generateNumber();            

            return result;*/

            return this.NewEnrollment;
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

        private int generateNumber()
        {
            /*string numbersAux;

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
            return numbersAux;*/
            return 0000;
        }

    }
}