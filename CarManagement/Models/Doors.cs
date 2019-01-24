namespace CarManagement.Models
{
    public class Doors
    {
        private bool open;

        public void Open()
        {
            open = true;
        }
        public void close()
        {
            open = false;
        }
        public bool IsOpen
        {
            get
            {
                return open;
            }
        }
    }
}