using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsepower = 0;
        private bool isStarted = false;

        public Engine(int h)
        {
            horsepower = h;
            isStarted = false;
        }

        public void start()
        {
            try
            {
                //Insert Start instructions here
                isStarted = true;
            }
            catch (Exception)
            {
                isStarted = false;
            }
        }

        public bool IsStarted
        {
            get
            {
                return isStarted;
            }

        }
    }
}