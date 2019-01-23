using System;

namespace CarManagement.Models
{
    public class Engine
    {
        public int HorsePower
        {
            get
            {
                return HorsePower;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("HorsePower value must be over 0.");
                else
                    HorsePower = value;
            }
        }
    }
}