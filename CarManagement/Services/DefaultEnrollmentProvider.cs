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
                            
        private const int TAM_ARRAYS_LETTERS = 3;
        private const int TAM_ARRAYS_NUMBERS = 4;

        private IEnrollment enrollment;

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
        }

        public IEnrollment NewEnrollment {
            get
            {
                return enrollment;
            }
            set
            {
                enrollment = value;
            }
                }

        IEnrollment IEnrollmentProvider.getNewEnrollment()
        {
            this.enrollment = new Enrollment(generateLetter(), generateNumber());
            return this.enrollment;
        }

        private string generateLetter()
        {
            string lettersAux;

            for (int i = TAM_ARRAYS_LETTERS - 1; i > 0; i--)
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
            string numbersAux;
            int numberFinal;

            for (int i = TAM_ARRAYS_NUMBERS - 1; i > 0; i--)
            {
                if (numbers[i] < '9')
                {
                    numbers[i]++;
                    numbersAux = new string(numbers);
                    numberFinal = int.Parse(numbersAux);
                    return numberFinal;
                }
                else if (numbers[i] == '9')
                {
                    numbers[i] = '0';
                }
            }
            numbersAux = new string(numbers);
            numberFinal = int.Parse(numbersAux);
            return numberFinal;
        }

    }
}