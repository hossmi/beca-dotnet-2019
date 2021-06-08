namespace CarManagement.Core.Models.DTOs
{
    public class EngineDto
    {
        public bool IsStarted { get; set; }
        public int HorsePower { get; set; }
        public EngineDto(bool isStarted, int horsePower)
        {
            this.HorsePower = horsePower;
            this.IsStarted = isStarted;
        }
        public EngineDto(int horsePower)
        {
            this.HorsePower = horsePower;
            this.IsStarted = false;
        }
        public EngineDto(bool isStarted)
        {
            this.IsStarted = isStarted;
            this.HorsePower = 100;
        }
        public EngineDto()
        {
            this.IsStarted = false;
            this.HorsePower = 100;
        }
    }
}
