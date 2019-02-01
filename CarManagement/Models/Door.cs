﻿using System;

namespace CarManagement.Models
{
    public class Door
    {
        private bool isOpen;

        public void open()
        {
            this.isOpen = true;
        }
        public void close()
        {
            this.isOpen = false;
        }
        public bool IsOpen
        {
            get
            {
                return this.isOpen;
            }
        }
    }
}