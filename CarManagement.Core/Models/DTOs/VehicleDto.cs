namespace CarManagement.Core.Models.DTOs
{
    public class VehicleDto
    {
        public CarColor Color { get; set; }
        public EngineDto Engine { get; set; }
        public EnrollmentDto Enrollment { get; set; }
        public WheelDto[] Wheels { get; set; }
        public DoorDto[] Doors { get; set; }
        public VehicleDto(CarColor color, EngineDto engineDto, EnrollmentDto enrollmentDto, WheelDto[] wheelsDto, DoorDto[] doorsDto)
        {
            this.Color = color;
            this.Engine = engineDto;
            this.Wheels = wheelsDto;
            this.Doors = doorsDto;
            this.Enrollment = enrollmentDto;
        }
        public VehicleDto()
        {

        }
    }
}
