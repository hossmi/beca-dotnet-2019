using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    public class Door
    {
        private const string DEFAULT_MODEL = "standart";
        private const string ALREADY_OPENED_DOOR = "You cannot open an opened door";
        private const string ALREADY_CLOSED_DOOR = "You cannot close a closed door";

        private bool isOpen;

        public Door(string model = null, bool isOpen = false)
        {
            this.Model = model ?? DEFAULT_MODEL;
            this.isOpen = isOpen;
        }

        public Door(Door door)
        {
            this.Model = door.Model;
            this.isOpen = door.IsOpen;
        }

        Door(DoorDto doorDto)
        {
            this.Model = doorDto.Model;
            this.isOpen = doorDto.IsOpen;
        }

        public string Model { get; }
        public bool IsOpen { get => this.isOpen; }

        public void open()
        {
            Asserts.isFalse(this.isOpen , ALREADY_OPENED_DOOR);
            this.isOpen = true;
        }
        public void close()
        {
            Asserts.isTrue(this.isOpen , ALREADY_CLOSED_DOOR);
            this.isOpen = false;
        }

        public Door Clone()
        {
            return new Door(this);
        }
    }
}