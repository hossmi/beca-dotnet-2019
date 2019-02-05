using CarManagement.Core.Models;

namespace BusinessCore.Tests.Models
{
    class Vehicle : IVehicle
    {
        public CarColor Color { get; set; }
        public IDoor[] Doors { get; set; }
        public IEngine Engine { get; set; }
        public IEnrollment Enrollment { get; set; }
        public IWheel[] Wheels { get; set; }
    }
}