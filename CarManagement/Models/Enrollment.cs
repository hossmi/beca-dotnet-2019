using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Enrollment
    {
        private int number4;
        private int number3;
        private int number2;
        private int number1;
        private const string LETTERS = "BCDFGHJKMNPQRSTUWYZ";
        private int sizeLetters;

        public Enrollment(String serial, int number)
        {
            this.number1 = serial.IndexOf(Convert.ToChar(serial[0]));
            this.number2 = serial.IndexOf(Convert.ToChar(serial[1]));
            this.number3 = serial.IndexOf(Convert.ToChar(serial[2]));
            this.Serial = serial;
            this.Number = number;
            this.sizeLetters = LETTERS.Length-1;
        }
        public Enrollment(Enrollment enrollment)
        {
            this.Serial = enrollment.Serial;
            this.Number = enrollment.Number;
            this.number1 = this.Serial.IndexOf(Convert.ToChar(this.Serial[0]));
            this.number2 = this.Serial.IndexOf(Convert.ToChar(this.Serial[1]));
            this.number3 = this.Serial.IndexOf(Convert.ToChar(this.Serial[2]));
            this.number4 = 0;
            this.sizeLetters = LETTERS.Length - 1;
        }

        public Enrollment()
        {
            this.number1 = 0;
            this.number2 = 0;
            this.number3 = 0;
            this.number4 = 0;
            this.sizeLetters = LETTERS.Length - 1;
        }

        public string Serial { get; }
        public int Number{ get; }

        public override string ToString()
        {
            return $"{this.Serial}-{this.Number}";
        }
        public Enrollment getNewEnrollment()
        {
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

            return new Enrollment(lettersEnrollment, number4);
        }
    }
}
