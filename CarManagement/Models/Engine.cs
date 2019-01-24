using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower;

        public Engine (int horsePower)
        {
            if(horsePower <= 0)
            {
                throw new ArgumentException("HorsePower must be positive");

            }

            this.horsePower = horsePower;

            
        }
        public bool IsStarted
        {
            get
            {
                return this.IsStarted;
            }
        }

        public void start()
        {
            
        
        }
    }
}