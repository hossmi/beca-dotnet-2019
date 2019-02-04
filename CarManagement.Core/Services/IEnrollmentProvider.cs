using CarManagement.Core.Models;

namespace CarManagement.Core.Services
{
    public interface IEnrollmentProvider
    {
        IEnrollment getNew();
        IEnrollment import(string serial, int number);
    }
}
