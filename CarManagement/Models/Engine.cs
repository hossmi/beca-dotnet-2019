using System;

namespace CarManagement.Models
{
    public class Engine
    {
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
                else
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

        void Start()
        {
            IsStarted = true;
        }

        void Stop()
        {
            IsStarted = false;
        }


    }
}