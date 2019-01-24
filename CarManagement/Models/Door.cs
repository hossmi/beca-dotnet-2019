using System;

namespace CarManagement.Models
{

    public class Door

    {
        private bool open = false;

        public void Open()
        {
            try
            {
                this.open = true;
            }
            catch (Exception)
            {
                this.open = false;
            }
        }

        public void close()
        {
            try
            {
                this.open = false;
            }
            catch (Exception)
            {
                this.open = true;
            }
        }

        public bool IsOpen
        {
            get
            {
                return this.open;
            }

        }

    }


}