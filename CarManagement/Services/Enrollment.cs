using CarManagement.Models;

namespace CarManagement.Services
{
    internal class Enrollment : IEnrollment
    {
        private string serial;
        private int number;

        public Enrollment(string serial, int number)
        {
            this.serial = serial;
            this.number = number;
        }
        public string Serial
        {
            get
            {
                return this.serial;
            }
        }
        public int Number
        {
            get
            {
                return this.number;
            }
        }

        
    }
}