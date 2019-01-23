using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Door
    {
        private bool Open;
        public bool IsOpen
        {
            get
            {
                if (Open)
                    return true;
                return false;
            }
            set
            {

            }


        }
    }
}
