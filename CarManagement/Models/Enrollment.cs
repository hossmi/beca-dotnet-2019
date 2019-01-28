namespace CarManagement.Models
{
    public class Enrollment : IEnrollment
    {
        private readonly string serial;
        private readonly int number;

        public string Serial { get => serial; }
        public int Number { get => number; }

        public Enrollment(int number, string serial)
        {
            this.number = number;
            this.serial = serial;
        }

        override public string ToString()
        {
            string numberPrinted = number.ToString("0000");
            return $"{serial}-{numberPrinted}";
        }
    }
}
