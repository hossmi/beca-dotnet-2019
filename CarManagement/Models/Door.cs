using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;

namespace CarManagement.Models
{
    public class Door : IDoor
    {
        private const string ALREADY_OPENED_DOOR = "You cannot open an opened door";
        private const string ALREADY_CLOSED_DOOR = "You cannot close a closed door";

        private bool isOpen;

        public Door(bool isOpen = false)
        {
            this.isOpen = isOpen;
        }

        public Door(Door door)
        {
            this.isOpen = door.IsOpen;
        }

        Door(DoorDto doorDto)
        {
            this.isOpen = doorDto.IsOpen;
        }
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