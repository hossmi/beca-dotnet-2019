namespace CarManagement.Core.Models.DTOs
{
    public class EnrollmentDto
    {
        public string Serial { get; set; }
        public int Number { get; set; }
        public EnrollmentDto(string serial, int number)
        {
            this.Serial = serial;
            this.Number = number;
        }
        public EnrollmentDto()
        {

        }
    }
}
