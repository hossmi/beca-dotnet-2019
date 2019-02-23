﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Enrollment
    {
        private int number1;
        private int number2;
        private int number3;        
        private int number4;
        private const string LETTERS = "BCDFGHJKMNPQRSTUWYZ";
        private int sizeLetters;

        public Enrollment(String serial, int number)
        {
            this.Serial = serial;
            this.Number = number;
            this.sizeLetters = LETTERS.Length-1;
        }
        public Enrollment(Enrollment enrollment)
        {
            this.Serial = enrollment.Serial;
            this.Number = enrollment.Number;
            this.sizeLetters = LETTERS.Length - 1;
        }
        public Enrollment()
        {
            this.Serial = "BBB";
            this.Number = 0000;
            this.sizeLetters = LETTERS.Length - 1;
        }

        public string Serial { get; set; }
        public int Number{ get; set; }
        public override string ToString()
        {
            return $"{this.Serial}-{this.Number}";
        }

        public Enrollment getNewEnrollment()
        {
            return new Enrollment
            {
                
            };
           


            if (this.number4 <= 9999)
            {
                number4++;
            }
            else
            {
                this.number4 = 0;
                if (this.number3 < this.sizeLetters)
                {
                    this.number3++;
                }
                else
                {
                    number4 = 0;
                    number3 = 0;
                    if (this.number2 < this.sizeLetters)
                    {
                        number2++;
                    }
                    else
                    {
                        number4 = 0;
                        number3 = 0;
                        number2 = 0;
                        if (this.number1 < this.sizeLetters)
                        {
                            this.number1++;
                        }
                        else
                        {
                            throw new ArgumentException("Superado límite de matrículas");
                        }
                    }
                }
            }
            string lettersEnrollment = LETTERS[number1].ToString() + LETTERS[number2].ToString() + LETTERS[number3].ToString();

            
        }
    }
}
