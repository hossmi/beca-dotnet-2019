using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    class GeneraRandom
    {
        public String GenerateRandom()
        {
            Random randomGenerate = new Random();
            String sPassword = "";
            sPassword = System.Convert.ToString(randomGenerate.Next(00000001, 99999999));
            return sPassword.Substring(sPassword.Length - 8, 8);
        }
    }
}
