﻿using System;

namespace CarManagement.Models
{
    public class Door
    {
        private bool openDoor;

        public Door()
        {
            this.openDoor = false;
        }

        public void open()
        {
            this.openDoor = true;
        }

        public void close()
        {
            this.openDoor = false;
        }

        public bool IsOpen
        {
            get
            {
                return this.openDoor;
            }
        }
    }
}
