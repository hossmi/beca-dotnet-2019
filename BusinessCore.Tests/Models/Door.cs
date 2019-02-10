using CarManagement.Core.Models;

namespace BusinessCore.Tests.Models
{
    class Door : IDoor
    {
        public bool IsOpen { get; set; }

        public void close()
        {
            this.IsOpen = false;
        }

        public void open()
        {
            this.IsOpen = true;
        }
    }
}