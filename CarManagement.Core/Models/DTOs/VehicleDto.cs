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
        public VehicleDto(CarColor color, EngineDto engineDto, EnrollmentDto enrollmentDto, WheelDto[] wheelsDto, DoorDto[] doorsDto)
        {
            this.Color = color;
            this.Engine = engineDto;
            this.Wheels = wheelsDto;
            this.Doors = doorsDto;
            this.Enrollment = enrollmentDto;
        }
        public VehicleDto(string color, int horsePower, bool isStarted, string serial, int number, int wheels, int doors)
        {
            this.Color = (CarColor)Enum.Parse(typeof(CarColor), color);
            this.Engine = new EngineDto(horsePower, isStarted);
            this.Enrollment = new EnrollmentDto(serial, number);
            this.Wheels = new WheelDto[wheels];
            for (int i = 0; i < wheels; i++)
            {
                this.Wheels[i] = new WheelDto();
            }
            this.Doors = new DoorDto[doors];
            for (int i = 0; i < doors; i++)
            {
                this.Doors[i] = new DoorDto();
            }
        }
        public VehicleDto()
        {

        }
    }
}
