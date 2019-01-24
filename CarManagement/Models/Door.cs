using System;

namespace CarManagement.Models
{
    public class Door
    {
        private int name;
        private Boolean openDoor=false;

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
