using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool isStarted;
        private int horsePower;

        public Engine(int horsePower)
        {
            this.horsePower = horsePower;
        }

        public bool IsStarted
        {
            get
            {
                return this.isStarted;
            }
        }

        public int HorsePower
        {
            get
            {
                return this.horsePower;
            }
        }

        public void start()
        {
            if(this.isStarted == false)
            {
                this.isStarted = true;
            }
        }

        public void stop()
        {
            if (this.isStarted == true)
            {
                this.isStarted = false;
            }
        }
    }
}