using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    public class Engine
    {
        private string model;
        private int horsePower;
        private bool isStarted;

        public Engine(int horsePower = 50, string model = null, bool started = false)
        {
            Asserts.isTrue(horsePower > 0, "Negative or none horse power would be counterproductive, wouldn't it?");
            this.model = model ?? "standart";
            this.isStarted = started;
            this.horsePower = horsePower;
        }

        public Engine(Engine engine)
        {
            this.model = engine.model;
            this.horsePower = engine.horsePower;
        }

        Engine(EngineDto engineDto)
        {
            this.model = engineDto.Model;
            this.isStarted = engineDto.IsStarted;
            this.horsePower = engineDto.HorsePower;
        }

        public void start()
        {
            Asserts.isFalse(this.isStarted,"Cannot start a started engine");
            this.isStarted = true;
        }
        public void stop()
        {
            Asserts.isTrue(this.isStarted, "Cannot stop a stoped engine");
            this.isStarted = false;
        }

        public string Model { get => this.model; }
        public int HorsePower { get => this.horsePower; }
        public bool IsStarted { get => this.isStarted; }

        public Engine Clone()
        {
            return new Engine(this);
        }
    }
}