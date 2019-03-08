namespace CarManagement.Core.Models.DTOs
{
    public class EnrollmentDto
    {
        public string Serial { get; set; }
        public int Number { get; set; }
        public EnrollmentDto(string Serial, int Number)
        {
            this.Serial = Serial;
            this.Number = Number;
        }
        public EnrollmentDto()
        {

        }
    }
}
