namespace CarManagement.Core.Models.DTOs
{
    public class EngineDto
    {
        public bool IsStarted { get; set; }
        public int HorsePower { get; set; }
        public EngineDto(int horsePower, bool isStarted)
        {
            this.IsStarted = isStarted;
            this.HorsePower = horsePower;
        }
        public EngineDto()
        {

        }
    }
}
