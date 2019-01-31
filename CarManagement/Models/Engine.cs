using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower;
        private bool mode;
        public Engine()
        {
            this.horsePower = 1;
        }

        public Engine(int horsePower)
        {
            Asserts.isTrue(horsePower > 0);
            this.horsePower = horsePower;

        }

        public bool IsStarted
        {
            get
            {
                return this.mode;
            }
        }

        public int HorsePower
        {
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

        public void start()
        {
            this.mode = true;
        }
        public void stop()
        {
            this.mode = false;
        }
    }
}
   
