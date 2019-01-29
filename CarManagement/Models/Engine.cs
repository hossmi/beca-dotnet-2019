using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    public class Engine
    {
        private string model;
        private int horsePower;
        private bool isStarted;

        public Engine(int horsePower = 0, string model = null, bool started = false)
        {
            this.model = model ?? "standart";
            this.isStarted = started;
            this.horsePower = horsePower < 1 ? 50 : horsePower;
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

        public int HorsePower
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void start()
        {
            this.isStarted = true;
        }

        public string Model { get => model; }
        public int HorsePower { get => horsePower; }
        public bool IsStarted { get => isStarted; }

        public Engine Clone()
        {
            return new Engine(this);
        }

        public void stop()
        {
            throw new NotImplementedException();
        }
    }
}