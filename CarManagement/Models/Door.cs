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
            Asserts.isFalse(this.openDoor);
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
            Asserts.isFalse(this.IsOpen);
            this.openDoor = true;
        }

        public void close()
        {
            Asserts.isTrue(this.openDoor);
            this.openDoor = false;
        }
    }
}
