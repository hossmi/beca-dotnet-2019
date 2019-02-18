using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int numbers;
        private readonly string[] letters = { "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z;" };
       
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
            throw new System.NotImplementedException();
        }

        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            throw new System.NotImplementedException();
        }
    }
}