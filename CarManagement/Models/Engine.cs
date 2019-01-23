using System;

namespace CarManagement.Models
{
    public class Engine
    {

        private bool started = false;

        public void Start()
        {
            try
            {
                //Insert Start instructions here
                started = true;
            }
            catch (Exception)
            {
                started = false;
            }
        }

        public bool IsStarted
        {
            get
            {
                return started;
            }

        }
    }
}