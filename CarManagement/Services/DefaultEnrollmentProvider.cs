using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        IEnrollment IEnrollmentProvider.getNew()
        {
            throw new System.NotImplementedException();
        }

        IEnrollment IEnrollmentProvider.import(string serial, int number)
        {
            return new PrvEnrollment()
            {
                Serial = serial,
                Number = number,
            };
        }

        private class PrvEnrollment : IEnrollment
        {
            public string Serial { get; set; }
            public int Number { get; set; }
        }
    }
}