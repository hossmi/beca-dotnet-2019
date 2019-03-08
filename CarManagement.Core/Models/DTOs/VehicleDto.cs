namespace CarManagement.Core.Models.DTOs
{
    public class VehicleDto
    {
        public CarColor Color { get; set; }
        public EngineDto Engine { get; set; }
        public EnrollmentDto Enrollment { get; set; }
        public WheelDto[] Wheels { get; set; }
        public DoorDto[] Doors { get; set; }
        public EnrollmentDto enrollmentDto { get; set; }
        public VehicleDto(CarColor Color, EngineDto Engine, EnrollmentDto Enrollment, WheelDto[] Wheels, DoorDto[] Doors, EnrollmentDto enrollmentDto)
        {
            this.Color = Color;
            this.Engine = Engine;
            this.Wheels = Wheels;
            this.Doors = Doors;
            this.enrollmentDto = enrollmentDto;
        }
        public VehicleDto()
        {

        }
    }
}
