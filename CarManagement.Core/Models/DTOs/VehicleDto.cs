namespace CarManagement.Core.Models.DTOs
{
    public class VehicleDto
    {
        public CarColor Color { get; set; }
        public EngineDto Engine { get; set; }
        public EnrollmentDto Enrollment { get; set; }
        public WheelDto[] Wheels { get; set; }
        public DoorDto[] Doors { get; set; }
    }
}
