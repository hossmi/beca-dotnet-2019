using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace BusinessCore.Tests.Services
{
    public class SingleEnrollmentProvider : IEnrollmentProvider
    {
        //public IEnrollment getNew();
        //public IEnrollment import(string serial, int number);
        private class Enrollment : IEnrollment
        {
            private string serial;
            private int number;

            public Enrollment(string serial, int number)
            {

            }

            public string Serial { get; }
            public int Number { get; }
        }

        public override string ToString()
        {
            return $"{this.Serial}-{this.Number.ToString("0000")}";
        }

        public SingleEnrollmentProvider()
        {
            this.DefaultEnrollment = new Enrollment(serial: "XXX", number: 666);
        }

        public IEnrollment DefaultEnrollment { get; }

        IEnrollment IEnrollmentProvider.getNew()
        {
            return this.DefaultEnrollment;
        }

        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }
    }
}
