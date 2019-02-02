using System;

namespace CarManagement.Models
{
    public class Door
    {
        private bool isOpen;

        public Door()
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

        public void open()
        {
            if (this.isOpen == false)
            {
                this.isOpen = true;
            }
        }

        public void close()
        {
            if (this.isOpen == true)
            {
                this.isOpen = false;
            }
        }
    }
}