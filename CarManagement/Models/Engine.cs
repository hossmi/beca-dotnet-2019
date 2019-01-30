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
            startedEngine = false;
            engineType = "VR";
            this.horsePower = horsePower;

        }

        public bool IsStarted
        {
            get
            {
                return startedEngine;
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
            startedEngine = true;
        }

        public void stop()
        {
            throw new NotImplementedException();
        }
    }
}



