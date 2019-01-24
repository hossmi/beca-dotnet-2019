using System;

namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePower=0;

        public int Model {
            get
            {
                return this.horsePower;
            }
            set
            {
                if (value>0)
                {
                    this.horsePower = value;
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
        }
    }
}