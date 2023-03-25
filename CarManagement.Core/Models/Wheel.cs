namespace CarManagement.Core.Models
{
    public class Wheel : IWheel
    {
        public double pressure;
        public double Pressure
        {
            get
            {
                return this.pressure;
            }
            set
            {
                Asserts.isTrue(value >= 1 && value <= 5);
                this.pressure = value;
            }
        }
        public Wheel()
        {
            this.pressure = 1;
        }
    }
}
