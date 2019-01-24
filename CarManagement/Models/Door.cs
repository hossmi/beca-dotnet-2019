using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    class Door
    {
        private bool open = false;

        public void Open()
        {
            try
            {
               
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
