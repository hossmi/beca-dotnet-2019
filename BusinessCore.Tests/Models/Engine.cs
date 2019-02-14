using CarManagement.Core.Models;

namespace BusinessCore.Tests.Models
{
    class Engine : IEngine
    {
        public int HorsePower { get; set; }
        public bool IsStarted { get; set; }

        public void start()
        {
            this.IsStarted = true;
        }

        public void stop()
        {
            this.IsStarted = false;
        }
    }
}