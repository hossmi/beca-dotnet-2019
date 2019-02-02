using System;

namespace CarManagement.Models
{
    public class Door
    {
        bool openDoor;
        public Door()
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
