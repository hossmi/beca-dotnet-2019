namespace CarManagement.Models
{
    public class Wheel
    {
        private string model;
        private double pressure;

        public double Pressure
        {
            get => pressure;
            set {
                if (value >= 0) pressure = value;
                else throw new System.ArgumentException("Pressure cannot be less than 0");
            }
        }

        public string Model { get => model; }

        public Wheel()
        {
            this.model = "standart";
            this.Pressure = 2.0d;
        }

        public Wheel(Wheel wheel)
        {
            this.model = wheel.model;
            this.Pressure = wheel.Pressure;
        }
    }
}