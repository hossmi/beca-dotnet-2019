using System;

namespace CarManagement.Core.Models.DTOs
{
    public class VehicleDto
    {
        public CarColor Color { get; set; }
        public EngineDto Engine { get; set; }
        public EnrollmentDto Enrollment { get; set; }
        public WheelDto[] Wheels { get; set; }
        public DoorDto[] Doors { get; set; }
        public VehicleDto(CarColor color, EngineDto engine, EnrollmentDto enrollment, WheelDto[] wheels, DoorDto[] doors)
        {
            this.Color = color;
            this.Engine = engine;
            this.Enrollment = enrollment;
            this.Wheels = wheels;
            this.Doors = doors;
        }
        public VehicleDto()
        {

        }
    }
}
