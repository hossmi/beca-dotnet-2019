using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Door
    {
        private bool openedDoor;
        public Door()
        {
            openedDoor = false;
        }
        public bool IsOpen
        {
            get
            {
                return openedDoor;
            }
        }

        public void open()
        {
            openedDoor = true;
        }

        public void close()
        {
            openedDoor = false;
        }
    }
}