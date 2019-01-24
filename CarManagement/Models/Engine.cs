namespace CarManagement.Models
{
    public class Engine
    {
        private int horsePorwer;
        private bool start;

        public int HorsePorwer
        {
            get
            {
                return this.horsePorwer;
            }
            set
            {
                this.horsePorwer = value;
            }
        }

        public void Start()
        {
            this.start = true;
        }

        public bool IsStarted
        {
            get
            {
                return this.start;
            }
        }

    }
}