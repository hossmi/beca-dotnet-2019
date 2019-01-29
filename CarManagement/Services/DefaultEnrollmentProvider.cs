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

        private IEnrollment enrollment;

        private char[] letters =
        {
            'B', 'B', 'B'
        };
        private int finalNumber = 0;

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
                if (letters[i] < 'Z')
                {
                    if((letters[i]++ == 'E') || (letters[i]++ == 'I') || (letters[i]++ == 'O') || 
                        (letters[i]++ == 'Q') || (letters[i]++ == 'U'))
                    {
                        letters[i]++;
                    }
                    letters[i]++;
                    lettersAux = new string(letters);
                    return lettersAux;
                }
                else if(letters[i] == 'Z')
                {
                    letters[i] = 'B';
                }
            }
            lettersAux = new string(letters);
            return lettersAux;
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            throw new System.NotImplementedException();
        }

        private int generateNumber()
        {
            finalNumber++;

            return finalNumber;
        }

    }
}