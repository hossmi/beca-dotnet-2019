namespace CarManagement.Models
{
    public class Door
    {
        public readonly string model;
        private bool isOpen;

        public Door(string model = null)
        {
            this.model = model ?? "standart";
            this.isOpen = false;
        }

        public Door(Door door)
        {
            this.model = door.model;
            this.isOpen = door.IsOpen;
        }

        public bool IsOpen { get => isOpen; }

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