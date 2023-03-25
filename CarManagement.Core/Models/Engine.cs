namespace CarManagement.Core.Models
{
    public class Engine : IEngine
    {
        public int HorsePower { get; set; }
        public bool IsStarted { get; set; }

        public void start()
        {
            Asserts.isTrue(this.IsStarted == false);
            this.IsStarted = true;
        }
        public void stop()
        {
            Asserts.isTrue(this.IsStarted == true);
            this.IsStarted = false;
        }
    }
}
