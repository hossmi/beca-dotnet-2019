using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool isStarted;
        private double horsepower;

        public Engine(double horsepower)
        {
            Asserts.isTrue(horsepower >= 1);
            this.horsepower = horsepower;
        }

        public Engine(double horsepower, bool isStarted)
        {
            this.horsepower = horsepower;
            this.isStarted = isStarted;
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
                return (int)this.horsepower;
            }
        }

        public void start()
        {
            this.isStarted = true;
        }

        public void setHorsePower(double nHorsePower)
        {
            Asserts.isTrue(this.horsepower >= 1);
            this.horsepower = nHorsePower;
        }

        public void stop()
        {
            this.isStarted = false;
        }
    }
}