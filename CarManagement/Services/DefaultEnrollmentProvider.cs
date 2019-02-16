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

        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            throw new System.NotImplementedException();
        }
    }
}