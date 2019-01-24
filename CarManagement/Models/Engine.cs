using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool start;

        public bool IsStarted
        {
            get
            {
                return start;
            }
        }

        public void start()
        {
            start = true;
        }
    }
}