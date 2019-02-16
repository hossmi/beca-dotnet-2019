using CarManagement.Core.Models;

namespace CarManagement.Core.Services
{
    public interface IEnrollmentImporter
    {
        IEnrollment import(string serial, int number);
    }
}
