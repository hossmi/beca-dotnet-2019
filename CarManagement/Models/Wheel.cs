<<<<<<< HEAD
﻿using CarManagement.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{

    public class Wheel
    {
        double pressure = 0;
        public double Pressure
        {
            set
            {
                Asserts.isTrue(value >= 0);
                this.pressure = value;
            }    
            get
            {
                return this.pressure ;
            }
        }

        public Wheel()
        {
            Pressure = 0;
        }
    }
}
=======
﻿using System;

namespace CarManagement.Models
{
    public class Wheel
    {
        public double Pressure
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
>>>>>>> develop
