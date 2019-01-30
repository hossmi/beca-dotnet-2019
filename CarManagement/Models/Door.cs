using System;

namespace CarManagement.Models
{
    public class Door
    {
        
        private bool openDoor;

        public Door()
        {
            this.openDoor = false;
        }

        public Door(bool openClose)
        {
            this.openDoor = openClose;
        }

        public bool IsOpen
        {
            get
            {
                return this.openDoor;
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
