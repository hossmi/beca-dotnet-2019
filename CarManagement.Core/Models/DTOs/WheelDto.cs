namespace CarManagement.Core.Models.DTOs
{
    public class WheelDto
    {
        public double Pressure { get; set; }
        public WheelDto(double pressure)
        {
            this.Pressure = pressure;
        }
        public WheelDto()
        {

        }
    }
}
