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
                return this.horsepower;
            }
            set
            {
                this.horsepower = value;
            }
        }
        public bool IsStarted
        {
            get
            {
                return this.isStart;
            }
            set
            {
                this.isStart = value;
            }
        }

        public void start()
        {
            this.isStart = true;
        }

        public void stop()
        {
            this.isStart = false;
        }
    }
}