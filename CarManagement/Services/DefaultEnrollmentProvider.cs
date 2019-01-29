using CarManagement.Models;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        IEnrollment IEnrollmentProvider.getNew()
        {
            throw new System.NotImplementedException();
        }

        IEnrollment IEnrollmentProvider.import(string enrollment)
        {
            throw new System.NotImplementedException();
        }
    }
}