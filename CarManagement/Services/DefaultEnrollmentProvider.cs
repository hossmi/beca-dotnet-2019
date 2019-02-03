using CarManagement.Models;

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
        IEnrollment IEnrollmentProvider.getNew()
        {
                int Number = 0;
                string Serial;
                string A = "BCDFGHJKLMNPRSTUVWXYZ";
                int lenght = A.Length - 1;
                int i = 0;
                int i2 = 0;
                int i3 = 0;
                int numo = 9999;

                Serial = System.Convert.ToString(A[i]) + System.Convert.ToString(A[i2]) + System.Convert.ToString(A[i3]);
                if (i <= lenght)
                {

                    if (Number == numo)
                    {
                        if (i2 >= lenght && i3 == lenght)
                        {
                            i2 = 0;
                            i3 = 0;
                            i++;
                        }
                        if (i3 == lenght)
                        {
                            i3 = 0;
                            i2++;
                        }
                        else
                        {
                            i3++;
                            Number = 0;
                        }
                    }
                    else
                    {
                        Number++;
                    }
                }
                else
                {
                    System.Console.WriteLine("Has alcanzado el máximo de matrículas");
                }
                Enrollment enrollment = new Enrollment(Serial, Number);
                return enrollment;
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            Enrollment enrollment = new Enrollment(serial, number);
            return enrollment;
        }
    }
}