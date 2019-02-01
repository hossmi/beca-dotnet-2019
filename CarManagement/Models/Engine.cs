using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower;
        private bool mode;
        private bool isStarted;

        public Engine(int horsePower)
        {
            Asserts.isTrue(horsePower>0);
            this.horsePower = horsePower;
        }
        public int Model {
            get
            {
                return this.horsePower;
            }
            set
            {
                Asserts.isTrue(value>0);
                this.horsePower = value;
            }
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
            Asserts.isFalse(this.isStarted);
            this.isStarted  = true;
        }

        public void stop()
        {
            Asserts.isTrue(this.isStarted);
            this.isStarted = false;
        }
    }
}