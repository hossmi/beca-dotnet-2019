using CarManagement.Services;

namespace BusinessCore.Tests
{
    public class FakeEnrollmentProvider : IEnrollmentProvider
    {
        private readonly string enrollment;

        public FakeEnrollmentProvider(string enrollment)
        {
            this.enrollment = enrollment;
        }

        string IEnrollmentProvider.getNewEnrollment()
        {
            return this.enrollment;
        }
    }
}