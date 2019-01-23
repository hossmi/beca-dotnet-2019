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

        public bool IsOpen
        {
            get
            {
                return open;
            }

        }

    }


}