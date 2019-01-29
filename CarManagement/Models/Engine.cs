using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsepower;
        private bool isStarted;

        public Engine(int h)
        {
            Asserts.isTrue(h > 0, "Cannot create an engine with 0 or less Horse Power.");
            this.horsepower = h;
            //this.isStarted = false;
        }

        public void start()
        {
            Asserts.isFalse(this.isStarted, "Engine is already started.");
            this.isStarted = true;
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
                return this.horsepower;
            }
        }

        public void stop()
        {
            Asserts.isTrue(this.isStarted, "Engine is already stopped.");
            this.isStarted = false;
        }
    }
}