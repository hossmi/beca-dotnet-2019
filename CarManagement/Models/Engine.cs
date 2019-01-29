using CarManagement.Builders;
using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower;
        private bool mode;

        public Engine(int horsePower)
        {
            Asserts.isTrue(horsePower>0);
            this.horsePower = horsePower;
        }

        public Engine(int horsePower,bool mode)
        {
            Asserts.isTrue(horsePower>0);
            this.horsePower = horsePower;
            this.mode = mode;
        }

        public int Model {
            get
            {
                return this.horsePower;
            }
            set
            {
                Asserts.isTrue(value>0);
                this.horsePower = value;
            }
        }
        public bool IsStarted
        {
            get
            {
                return this.mode;
            }
        }

        public int HorsePower
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void start()
        {
            this.mode  = true;
        }

        public void stop()
        {
            this.mode=false;
        }
    }
}