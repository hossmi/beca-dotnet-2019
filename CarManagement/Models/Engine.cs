using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower;
        private bool mode;

        public Engine(int horsePower)
        {
            if (horsePower<=0)
                throw new ArgumentException("Horse power need positive.");
            this.horsePower = horsePower;
        }
        public int Model {
            get
            {
                return this.horsePower;
            }
            set
            {
                if (value>0)
                {
                    this.horsePower = value;
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
        }
        public bool IsStarted
        {
            get
            {
                return this.mode;
            }
        }

        public void start()
        {
            this.mode  = true;
        }
    }
}