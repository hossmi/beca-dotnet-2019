using CarManagement.Core.Models;

namespace CarManagement.Core
{
    public class Door : IDoor
    {
        public bool IsOpen { get; set; }

        public void open()
        {
            Asserts.isTrue(this.IsOpen == false);
            this.IsOpen = true;
        }
        public void close()
        {
            Asserts.isTrue(this.IsOpen == true);
            this.IsOpen = false;
        }
    }
}
