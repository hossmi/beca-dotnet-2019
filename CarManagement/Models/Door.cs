using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Door
    {
        private bool openedDoor;
        public Door()
        {
            this.openedDoor = false;
        }
        public bool IsOpen
        {
            get
            {
                return this.openedDoor;
            }
        }

        public void open()
        {
            this.openedDoor = true;
        }

        public void close()
        {
            this.openedDoor = false;
        }
    }
}