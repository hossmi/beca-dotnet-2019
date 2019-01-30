using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private const int MAXPOWER = 4000;
        private const int MINPOWER = 1;

        private int horsepower;
        private bool isStarted;

        public Engine(int h)
        {
            Asserts.isTrue(h >= MINPOWER, $"Cannot create an engine with less than {MINPOWER} Horse Power.");
            Asserts.isTrue(h <= MAXPOWER, $"Cannot create an engine above {MAXPOWER} Horse Power.");
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