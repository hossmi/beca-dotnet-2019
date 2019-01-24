using System;

namespace CarManagement.Models
{
    public class Engine
    {
        public Engine(int horsePower)
        {
            this.HorsePower = horsePower;
        }
        public int HorsePower
        {
            get
            {
                return HorsePower;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("HorsePower value must be over 0.");
                HorsePower = value;
            }
        }

        public bool IsStarted
        {
            get
            {
                return IsStarted;
            }
            private set
            {
                IsStarted = value;
            }
        }

        public void start()
        {
            IsStarted = true;
        }

        public void stop()
        {
            IsStarted = false;
        }


    }
}