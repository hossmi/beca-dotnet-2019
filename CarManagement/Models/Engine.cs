﻿using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower;
        public Engine(int horsePower)
        {
            this.horsePower = horsePower;
        }
        public int HorsePower
        {
            get
            {
                return horsePower;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("HorsePower value must be over 0.");
                horsePower = value;
            }
        }

        public bool IsStarted { get; private set; }

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