using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private string engineType;
        private int horsePower;
        private bool startedEngine;
        public Engine(int horsePower)
        {
            this.startedEngine = false;
            this.engineType = "VR";
            this.horsePower = horsePower;

        }

        public bool IsStarted
        {
            get
            {
                return this.startedEngine;
            }
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
            this.startedEngine = true;
        }

        public void stop()
        {
            throw new NotImplementedException();
        }
    }
}



