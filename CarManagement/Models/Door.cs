namespace CarManagement.Models
{
    public class Door
    {
        private string model;
        private bool isOpen;

        public Door()
        {
            this.model = "standard";
            this.isOpen = false;
        }

        public Door(Door door)
        {
            this.model = door.Model;
            this.isOpen = door.IsOpen;
        }

        public bool IsOpen { get => isOpen; }
        public string Model { get => model; }

        public void open()
        {
            this.isOpen = true;
        }
        public void close()
        {
            this.isOpen = false;
        }
    }
}