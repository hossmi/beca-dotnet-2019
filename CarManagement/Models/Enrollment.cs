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
        private string letters;
        private int sizeLetters;

        public Enrollment(String serial, int number)
        {
            this.number1 = Convert.ToInt32(Convert.ToChar(serial[0]));
            this.number2 = Convert.ToInt32(Convert.ToChar(serial[1]));
            this.number3 = Convert.ToInt32(Convert.ToChar(serial[2]));
            this.Serial = serial;
            this.Number = number;
            this.letters = "BCDFGHJKMNPQRSTUWYZ";
            this.sizeLetters = letters.Length-1;
        }
        public Enrollment(Enrollment enrollment)
        {
            this.number1 = Convert.ToInt32(Convert.ToChar(enrollment.Serial[0]));
            this.number2 = Convert.ToInt32(Convert.ToChar(enrollment.Serial[1]));
            this.number3 = Convert.ToInt32(Convert.ToChar(enrollment.Serial[2]));
            this.Serial = enrollment.Serial;
            this.Number = enrollment.Number;
            this.letters = "BCDFGHJKMNPQRSTUWYZ";
            this.sizeLetters = letters.Length - 1;
        }

        public Enrollment()
        {
            this.number1 = 0;
            this.number2 = 0;
            this.number3 = 0;
            this.letters = "BCDFGHJKMNPQRSTUWYZ";
            this.sizeLetters = letters.Length - 1;
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
            string lettersEnrollment = this.letters[number1].ToString() + this.letters[number2].ToString() + this.letters[number3].ToString();
            return new Enrollment(lettersEnrollment, number4);
        }
    }
}
