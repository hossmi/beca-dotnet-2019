using CarManagement.Models;

using System;
using System.Text;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int letter1;
        private int letter2;
        private int letter3;
        private int number;
        public DefaultEnrollmentProvider()
        {
            this.letter1 = 65;
            this.letter2 = 65;
            this.letter3 = 65;
            this.number = -1;
        }

    
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


 

        IEnrollment IEnrollmentProvider.getNewEnrollment()
        {
            if (number <= 9999)
            {
                number++;
            }
            else
            {
                number = 0;
                if (letter3 <= 91)
                {
                    letter3++;
                }
                else
                {
                    number = 0;
                    letter3 = 0;
                    if (letter2 <= 91)
                    {
                        letter2++;
                    }
                    else
                    {
                        number = 0;
                        letter3 = 0;
                        letter2 = 0;
                        if (letter1 <= 91)
                        {
                            letter1++;
                        }
                        else
                        {
                            throw new ArgumentException("Superado límite de matrículas");
                        }
                    }
                }
            }
            string serial = ((Char)(letter1)).ToString() + ((Char)(letter2)).ToString() + ((Char)(letter3)).ToString();
            /*string letter1ToUTF16 = ((Char)(letter1)).ToString();
            string letter2ToUTF16 = ((Char)(letter2)).ToString();
            string letter3ToUTF16 = ((Char)(letter3)).ToString();
            byte[] serialUTF161 = Encoding.Unicode.GetBytes(letter1ToUTF16);
            byte[] serialUTF162 = Encoding.Unicode.GetBytes(letter2ToUTF16);
            byte[] serialUTF163 = Encoding.Unicode.GetBytes(letter3ToUTF16);
            serial = serialUTF161.ToString() + serialUTF162.ToString()+ serialUTF163.ToString();*/
            return new Enrollment(serial, this.number);
            
        }

    }
}