using CarManagement.Core.Models;

namespace CarManagement.Core.Services
{
    public interface IEnrollmentProvider : IEnrollmentImporter
    {
        IEnrollment getNew();
    }
}
