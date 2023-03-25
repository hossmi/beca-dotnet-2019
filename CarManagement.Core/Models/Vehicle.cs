using System.Collections.Generic;
using System.Linq;

namespace CarManagement.Core.Models
{
    public class Vehicle : IVehicle
    {
        private List<IDoor> doors;
        private List<IWheel> wheels;
        public IEngine Engine { get; set; }
        public IEnrollment Enrollment { get; set; }
        public CarColor Color { get; set; }
        public IWheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
            set
            {
                this.wheels = value.ToList();
            }
        }
        public IDoor[] Doors
        {
            get
            {
                return this.doors.ToArray();
            }
            set
            {
                this.doors = value.ToList();
            }
        }
    }
}
