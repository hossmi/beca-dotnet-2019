using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool start;
        private int horsePorwer;

        public Engine(int horsePorwer)
        {
            this.start = false;
            this.horsePorwer = horsePorwer;
        }

        public bool IsStarted
        {
            get
            {
                return start;
            }
        }

        public void Start()
        {
            start = true;
        }

        public int HorsePorwer
        {
            get
            {
                return horsePorwer;
            }
            set
            {
                horsePorwer = value;
            }
        }
    }
}