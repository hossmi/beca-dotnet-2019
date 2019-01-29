using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool startEngine;
        private int horsePorwer;

        public Engine(int horsePorwer)
        {
            this.startEngine = false;
            this.horsePorwer = horsePorwer;
        }

        public bool IsStarted
        {
            get
            {
                return startEngine;
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
            startEngine = true;
        }

        public int HorsePorwer
        {
            get
            {
                return horsePorwer;
            }
            set
            {
                horsePorwer = value;
            }
        }

        public void stop()
        {
            throw new NotImplementedException();
        }
    }
}