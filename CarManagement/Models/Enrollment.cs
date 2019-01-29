using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    class Enrollment : IEnrollment
    {
        public string Serial { get; }
        public int Number { get; }

        public Enrollment(int number, string serial)
        {
            this.Number = number;
            this.Serial = serial;
        }

        override public string ToString()
        {
            string numberPrinted = this.Number.ToString("0000");
            return $"{this.Serial}-{numberPrinted}";
        }
    }
}
