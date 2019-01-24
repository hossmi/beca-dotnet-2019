using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsepower = 0;
        private bool started = false;

        public Engine(int h)
        {
            horsepower = h;
            started = false;
        }

        public Engine()
        {
            horsepower = 0;
            started = false;
        }

        public void Start()
        {
            try
            {
                //Insert Start instructions here
                started = true;
            }
            catch (Exception)
            {
                started = false;
            }
        }

        public bool IsStarted
        {
            get
            {
                return started;
            }

        }
    }
}