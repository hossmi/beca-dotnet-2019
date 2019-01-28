using CarManagement.Builders;
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
                Asserts.isTrue(isOpen, "Error en la apertura de puerta");
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
                Asserts.isFalse(isOpen, "Error en el cierre de puerta");
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