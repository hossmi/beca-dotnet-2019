namespace CarManagement.Models
{
    internal class Door
    {
        private bool isOpen;

        public Door()
        {
            this.isOpen = false;
        }

        public bool IsOpen { get => isOpen; }

        void openDoor()
        {
            this.isOpen ? this.isOpen = false : this.isOpen = true;
            this.isOpen = true;
        }
    }
}