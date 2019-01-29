﻿using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    public class Door
    {
        private bool isOpen;

        public Door(string model = null, bool isOpen = false)
        {
            this.Model = model ?? "standart";
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
        public bool IsOpen { get => isOpen; }

        public void open()
        {
            this.isOpen = true;
        }
        public void close()
        {
            this.isOpen = false;
        }

        public Door Clone()
        {
            return new Door(this);
        }
    }
}