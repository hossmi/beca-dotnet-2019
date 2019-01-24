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
    }
}