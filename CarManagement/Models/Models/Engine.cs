namespace CarManagement.Models.Models
{
    public class Engine
    {
        public string model;
        private int horsePower;

        public Engine(Engine engine)
        {
            this.model = engine.model;
            this.horsePower = engine.horsePower;
        }
    }
}