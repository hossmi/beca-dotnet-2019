using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        const int maxWheels = 4;
        private List<Wheel> wheels = new List<Wheel>();
        private List<Door> doors = new List<Door>();
        private Engine engine = new Engine();
        private CarColor color;
        

        public void addWheel()
        {
            Wheel wheel = new Wheel();
            if (wheels.Count < maxWheels)
                wheels.Add(wheel);
            else
                throw new Exception("Se ha excedido el numero maximo de ruedas");
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount > 0)
            {
                for (int i = 0; i < doorsCount; i++)
                {
                    Door door = new Door();
                    doors.Add(door);
                }
            }
            else if (doorsCount < 0)
            {
                throw new Exception("No se puede crear un vehiculo con puertas negativas.");
            }
        }

        public void setEngine(int horsePorwer)
        {
            engine = new Engine(horsePorwer);
        }

        public void setColor(CarColor red)
        {
            color = red;
        }

        public Vehicle build()
        {
            Vehicle vehicle = new Vehicle();

            vehicle.SetWheels = wheels;
            vehicle.SetDoors = doors;
            vehicle.SetEngine = engine;
            vehicle.SetCarColor = color;

            if (wheels.Count == 0)
            {
                throw new Exception("No se puede crear un vehiculo sin ruedas");
            }

            return vehicle;
        }
    }
}