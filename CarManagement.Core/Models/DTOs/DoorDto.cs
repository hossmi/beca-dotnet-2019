namespace CarManagement.Core.Models.DTOs
{
    public class DoorDto
    {
        public bool IsOpen { get; set; }
        public DoorDto(bool isOpen)
        {
            this.IsOpen = isOpen;
        }
        public DoorDto()
        {
            this.IsOpen = false;
        }
    }
}
