using System;
using CarManagement.Models;


namespace CarManagement.Builders
{
    public class VehicleBuilder
        
    {
        private double a = 5.5;
        private int  wheels = 0;
        private int doors = 0;
        private int engine = 0;
        private CarColor color;
        private string enrollment = "AAAAAAA"

        public void addWheel()
        {
            this.wheels++;
        }

        public void setDoors(int doorsCount)
        {
            this.doors = doorsCount;
           
        }

        public void setEngine(int horsePorwer)
        {
            this.engine = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

     
        public Vehicle build()
        {

            //Generar los objetos

            //genero puertas
            Door[] doors = new Door[this.doors];
            
            for (int i = 0; i < this.doors; i++)
            {
                doors[i] = new Door();
            }
            //genero ruedas
            Wheel[] wheels = new Wheel[this.wheels];
            for (int i = 0; i < this.wheels ; i++)
            {
                wheels [i] = new Wheel();
            }
            //genero motor
            Engine engine = new Engine();

            
            return new Vehicle(this.color,doors,engine,wheels,enrollment);
        }
          
    }
}