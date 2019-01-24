using System;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private int numberWheel = 0;
        private int numberDoor = 0;
        private int engine = 0;
        private CarColor color;


        public void addWheel()
        {
            this.numberWheel++;
        }

        public void setDoors(int doorsCount)
        {
            this.numberDoor = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            this.engine = horsePorwer;
        }

        public void setColor(CarColor valor)
        {
            this.color = valor;
        }

        public Vehicle build()
        {
            Door[] doors= new Door[this.numberDoor];
            for (int x=0; x<this.numberDoor;x++)
            {
                doors[x] = new Door(x);
            }


            Engine engine = new Engine();

            Wheel[] wheels = new Wheel[numberWheel];
            for (int x=0; x<this.numberWheel;x++)
            {
                wheels[x] = new Wheel();
            }

            String enrollment = Math.Ceiling((decimal)DateTime.Now.Month).ToString();

            return new Vehicle(this.color,doors,engine,wheels, enrollment);
        }
    }
}