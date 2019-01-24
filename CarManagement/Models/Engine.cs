using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool isStart;
        private int horsepower;

        public int Horsepower
        {
            get
            {
                return horsepower;
            }
            set
            {
                horsepower = value;
            }
        }
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