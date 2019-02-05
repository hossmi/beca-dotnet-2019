using CarManagement.Core.Models;

namespace BusinessCore.Tests.Models
{
    class Enrollment : IEnrollment
    {
        public string Serial { get; set; }
        public int Number { get; set; }
    }
}