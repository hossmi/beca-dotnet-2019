using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool isStarted;
        private int horsePower;
        public Engine()
        {
            isStarted = false;
            horsePower = 1;
        }

        public bool IsStarted
        {
            get
            {
                return isStarted;
            }
        }

        public void start()
        {
            isStarted = true;
        }
    }
}