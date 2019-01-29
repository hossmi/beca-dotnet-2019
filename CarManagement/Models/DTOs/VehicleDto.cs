using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    class VehicleDto
    {
        public CarColor Color { get; set; }
        public EngineDto Engine { get; set; }
        public EnrollmentDto Enrollment { get; set; }
        public WheelDto[] Wheels { get; set; }
        public DoorDto[] Doors { get; set; }

        public VehicleDto(Vehicle v)
        {
            this.Color = v.Color;
            this.Engine = new EngineDto(v.Engine);
            this.Enrollment = new EnrollmentDto(v.Enrollment);
            this.Wheels = new WheelDto[v.Wheels.Length];
            this.Doors = new DoorDto[v.Doors.Length];

            int i = 0;
            foreach (Wheel w in v.Wheels)
            {
                this.Wheels[i] = new WheelDto(w);
                i++;
            }

            int j = 0;
            foreach (Door d in v.Doors)
            {
                this.Doors[j] = new DoorDto(d);
                j++;
            }
        }

        public Vehicle ConvertToVehicle()
        {
            Vehicle v;

            List<Wheel> wheels = new List<Wheel>();
            List<Door> doors = new List<Door>();
            Engine engine;
            IEnrollment enrollment;

            foreach (WheelDto w in this.Wheels)
            {
                wheels.Add(w.ConvertToWheel());
            }

            foreach (DoorDto d in this.Doors)
            {
                doors.Add(d.ConvertToDoor());
            }
            engine = this.Engine.ConvertToEngine();


            v = new Vehicle(wheels, doors, engine, this.Color, enrollment);
                       
            return v;
        }
    }
}
