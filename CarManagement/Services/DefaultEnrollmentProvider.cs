using CarManagement.Models;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        IEnrollment IEnrollmentProvider.getNewEnrollment()
        {
            throw new System.NotImplementedException();
        }
    }
}