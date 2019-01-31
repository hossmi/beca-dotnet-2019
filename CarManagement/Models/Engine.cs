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

        public Engine(int horsePorwer, bool startEngine)
        {
            this.startEngine = startEngine;
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
            Asserts.isFalse(startEngine);
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
            Asserts.isTrue(startEngine);
            this.startEngine = false;
        }
    }
}