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
        private string enrollment = "";

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

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            Vehicle vehicle = new Vehicle();

            generateEnrollment();

            vehicle.SetWheels = wheels;
            vehicle.SetDoors = doors;
            vehicle.SetEngine = engine;
            vehicle.SetCarColor = color;
            vehicle.SetEnrollment = enrollment;

            if (wheels.Count == 0)
            {
                throw new Exception("No se puede crear un vehiculo sin ruedas");
            }

            return vehicle;
        }

        private void generateEnrollment()
        {
            string e;
            Random rnd = new Random();
            string enA = "";
            string enB = "";
            int number;
            char c;

            System.Threading.Thread.Sleep(20);
            number = rnd.Next(0, 9999);

            if (number < 10)
            {
                enB = "000" + number.ToString();
            }
            else if (number < 100)
            {
                enB = "00" + number.ToString();
            }
            else if (number < 1000)
            {
                enB = "0" + number.ToString();
            }
            else
            {
                enB = number.ToString();
            }

            for (int i = 0; i < 3; i++)
            {
                number = rnd.Next(0, 26);
                c = (char)('A' + number);
                enA = enA + c;
            }
            e = enA + "-" + enB;

            enrollment = e;

        }

        private void generateEnrollmentB()
        {








        }
    }
}