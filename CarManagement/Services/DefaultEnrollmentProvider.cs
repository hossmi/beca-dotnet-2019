using CarManagement.Models;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        static int number = 0;
        static char Letter1 = 'A';
        static char Letter2 = 'A';
        static char Letter3 = 'A';

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
            
            
            string numberFill = "";

            if (number >= 10000)
            {
                number = 0;
                Letter3++;
                if (Letter3 > 'Z')
                {
                    Letter3 = 'A';
                    Letter2++;
                    if (Letter2 > 'Z')
                    {
                        Letter1++;
                        if (Letter1 > 'Z')
                            throw new System.Exception("Alcanzado el limite maximo de matriculas");
                    }
                }
            }

            if (number < 1000)
            {
                numberFill = numberFill + "0";
                if (number < 100)
                {
                    numberFill = numberFill + "0";
                    if (number < 10)
                        numberFill = numberFill + "0";
                }
            }

            //enrollment = Letter1.ToString() + Letter2.ToString() + Letter3.ToString() + "-" + numberFill + number.ToString();
            IEnrollment enrollment = new Enrollment(Letter1.ToString() + Letter2.ToString() + Letter3.ToString(), number);


            number++;



            return enrollment;
        }
    }
}