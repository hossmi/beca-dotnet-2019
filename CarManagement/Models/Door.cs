using CarManagement.Builders;
using System;

namespace CarManagement.Models
{

    public class Door

    {
        private bool isOpen = false;

        public void open()
        {
            Asserts.isFalse(this.isOpen,"Door is already open.");
            this.isOpen = true;
        }

        public void close()
        {
            Asserts.isTrue(this.isOpen,"Door is already close.");
            this.isOpen = false;
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