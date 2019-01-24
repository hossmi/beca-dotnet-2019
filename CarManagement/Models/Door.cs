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
                //Insert Open instructions here
                open = true;
            }
            catch (Exception)
            {
                open = false;
            }
        }

        public void close()
        {
            try
            {
                //Insert Open instructions here
                open = false;
            }
            catch (Exception)
            {
                open = true;
            }
        }

        public bool IsOpen
        {
            get
            {
                return open;
            }

        }

    }


}