using System;

namespace CarManagement.Models
{
    public class Door
    {
        private bool open;

        public Door Doors
        {
            get
            {
                return this;
            }

        }

        public void Open()
        {
            this.open = true;
        }

        public bool IsOpen()
        {

        }
    }
}
