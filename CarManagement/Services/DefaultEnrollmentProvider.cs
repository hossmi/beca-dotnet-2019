using CarManagement.Models;
using CarManagement.Services;
using System;
using System.Text;
﻿using CarManagement.Core.Models;
using CarManagement.Core.Services;

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
        
        public IEnrollment NewEnrollment {
            get
            {
                return this.enrollment;
            }
            set
            {
                this.enrollment = value;
            }
                }

        IEnrollment IEnrollmentProvider.getNew()
        {
            this.enrollment = new Enrollment(generateLetter(), generateNumber());
            return this.enrollment;
        }

        private string generateLetter()
        {
            string lettersAux;
            
            for (int i = TAM_ARRAYS_LETTERS - 1; i > 0; i--)
            {
                if (this.letters[i] < 'Z')
                {
                    if((this.letters[i]++ == 'E') || (this.letters[i]++ == 'I') || (this.letters[i]++ == 'O') || 
                        (this.letters[i]++ == 'Q') || (this.letters[i]++ == 'U'))
                    {
                        this.letters[i]++;
                    }
                    this.letters[i]++;
                    lettersAux = new string(this.letters);
                    return lettersAux;
                }
                else if(this.letters[i] == 'Z')
                {
                    this.letters[i] = 'B';
                }
            }
            lettersAux = new string(this.letters);
            return lettersAux;
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }

        private int generateNumber()
        {
            this.finalNumber++;

            return this.finalNumber;
        }

    }
}