using System;
namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int number1;
        private int number2;
        private int number3;
        private int number4;
        private string number = "";
        private string serial= "";


        public DefaultEnrollmentProvider()
        {
            this.number1 = -1;
            this.number2 = -1;
            this.number3 = -1;
            this.number4 = -1;
            this.serial = "";
        }
        string IEnrollmentProvider.getNewEnrollment()
        {
            if (number4<=9999)
            {
                number4++;
            }
            else
            {
                number4 = 0;
                if (number3<=26)
                {
                    number3++;
                }
                else
                {
                    number4 = 0;
                    number3 = 0;
                    if (number2 <= 26)
                    {
                        number2++;
                    }
                    else
                    {
                        number4 = 0;
                        number3 = 0;
                        number2 = 0;
                        if (number1<=26)
                        {
                            number1++;
                        }
                        else
                        {
                            throw new ArgumentException("Superado límite de matrículas");
                        }
                    }
                }
            }
            number1 += 65;
            number2 += 65;
            number3 += 65; 
            this.serial= (((Char)(number1)) + ((Char)(number2)) + (Char)(number3)).ToString();
            this.number = ((Char)(number1)) + ((Char)(number2)) + ((Char)(number3)) + " - " + number4.ToString("0000");

            return this.number;
        }

    }
}