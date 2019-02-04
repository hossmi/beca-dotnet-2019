using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool isStarted;
        private double horsepower;

        public Engine(double horsepower)
        {
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

        public void start()
        {
            this.isStarted = true;
        }

        public void setHorsePower(double nHorsePower)
        {
            this.horsepower = nHorsePower;
        }
    }
}