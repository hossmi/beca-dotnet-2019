using System;

namespace CarManagement.Models
{
    public class Door
    {
        private bool isOpen;

        public Door()
        {
            isOpen = false;
        }
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        public void open()
        {
            isOpen = true;
        }

        public void close()
        {
            isOpen = false;
        }
    }
}