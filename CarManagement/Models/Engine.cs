using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int engine;
        private bool isStarted;

        public Engine(int horsePower)
        {
            this.engine = horsePower;
            this.isStarted = false;
        }
        public bool IsStarted
        {
            get
            {
                return this.isStarted;
            }
        }

        public void start()
        {
            if (this.isStarted == false)
            {
                this.isStarted = true;
            }
        }
    }
}