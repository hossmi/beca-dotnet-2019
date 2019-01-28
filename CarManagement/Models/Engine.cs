using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower;
        private bool mode;

        public Engine(int horsePower)
        {
            Asserts.isTrue(horsePower > 0);
            this.horsePower = horsePower;   
            
        }
        public int Model {
            get
            {
                return this.horsePower;
            }
            set
            {
                Asserts.isTrue(value > 0);
                this.horsePower = value;

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