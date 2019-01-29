using CarManagement.Models;

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
            throw new System.NotImplementedException();
        }
    }
}