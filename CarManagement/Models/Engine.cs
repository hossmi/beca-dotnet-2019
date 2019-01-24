namespace CarManagement.Models
{
    public class Engine
    {
        int weight;
        string brand;
        public Engine() {

            this.weight = 0;
            this.brand = "";
        }
        public Engine(int weight,string brand) {

            this.weight = weight;
            this.brand = brand;
        
        }

    }
}