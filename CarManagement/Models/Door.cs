using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Door
    {
        public bool IsOpen
        {
            get
            {
                return IsOpen;
            }
            private set
            {
                IsOpen = value;
            }
        }

        public void open()
        {
            IsOpen = true;
        }
        public void close()
        {
            IsOpen = false;
        }
    }
}
