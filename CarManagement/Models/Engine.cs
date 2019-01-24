using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool isStart;

        public bool IsStarted
        {
            get
            {
                return isStart;
            }
        }

        public void start()
        {
            isStart = true;
        }
    }
}