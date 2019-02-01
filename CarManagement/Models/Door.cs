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

        public Door(bool openDoor)
        {
            this.openDoor = openDoor;
        }

        public void open()
        {
            Asserts.isFalse(this.openDoor);
            this.openDoor = true;
        }

        public void close()
        {
            Asserts.isTrue(this.openDoor);
            this.openDoor = false;
        }

        public bool IsOpen
        {
            get
            {
                return this.openDoor;
            }
        }
    }
}
