<<<<<<< HEAD
﻿using CarManagement.Builders;
using System;
=======
﻿using System;
>>>>>>> develop

namespace CarManagement.Models
{
    public class Engine
    {
<<<<<<< HEAD
        private int horsePower;
        private bool mode;

        public Engine(int horsePower)
        {
            Asserts.isTrue(horsePower > 0);
            this.horsePower = horsePower;   
            
        }
        public int Model {
            get
            {
                return this.horsePower;
            }
            set
            {
                Asserts.isTrue(value > 0);
                this.horsePower = value;

            }
        }
=======
>>>>>>> develop
        public bool IsStarted
        {
            get
            {
<<<<<<< HEAD
                return this.mode;
=======
                throw new NotImplementedException();
>>>>>>> develop
            }
        }

        public void start()
        {
<<<<<<< HEAD
            this.mode  = true;
=======
            throw new NotImplementedException();
>>>>>>> develop
        }
    }
}