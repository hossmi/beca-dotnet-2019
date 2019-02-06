﻿using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private bool isStarted;
        public bool IsStarted
        {
            get
            {
                return this.isStarted;
            }
        }

        public void start()
        {
            if(this.isStarted == false)
            {
                this.isStarted = true;
            }
        }
    }
}