namespace CarManagement.Models
{
    public class Engine
    {
        private string model;
        private int horsePower;

        public Engine(Engine engine)
        {
            this.model = engine.model;
            this.horsePower = engine.horsePower;
        }

        public string Model { get => model; }
        public int HorsePower { get => horsePower; }
    }
}