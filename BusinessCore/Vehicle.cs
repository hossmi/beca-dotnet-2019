using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore
{
    class Vehicle
    {
        public bool Electric
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Doors[] balance = new Doors[5]
        {
            FrontRight,
            FrontLeft,
            RearRight,
            RearLeft,
            Trunk,
        };

    }
}

}

