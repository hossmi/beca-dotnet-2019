using System;

namespace CarManagement.Models
{

    public class Door

    {
        private bool isOpen = false;

        public void open()
        {
            try
            {
                this.isOpen = true;
            }
            catch (Exception)
            {
                this.isOpen = false;
            }
        }

        public void close()
        {
            try
            {
                this.isOpen = false;
            }
            catch (Exception)
            {
                this.isOpen = true;
            }
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