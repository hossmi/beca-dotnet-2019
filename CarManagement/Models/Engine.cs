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
                return this.startEngine;
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
            Asserts.isFalse(this.startEngine);
            this.startEngine = true;
        }

        public int HorsePorwer
        {
            get
            {
                return this.horsePorwer;
            }
            set
            {
                this.horsePorwer = value;
            }
        }

        public void stop()
        {
            Asserts.isTrue(this.startEngine);
            this.startEngine = false;
        }
    }
}