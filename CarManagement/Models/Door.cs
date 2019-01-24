using System;

namespace CarManagement.Models
{
    public class Door
    {
        

        public bool IsOpen
        {
            get
            {
                return this.IsOpen;
            }
        }

        public void open()
        {
            this.openDoor = true;
        }

        public void close()
        {
            this.openDoor = false;
        }
    }
}