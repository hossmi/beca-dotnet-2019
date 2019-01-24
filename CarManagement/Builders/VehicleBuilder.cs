using System;
using System.Collections.Generic;
using CarManagement.Models;
using System.IO;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private static int intEnrollment;
        const int maxWheels = 4;
        private string enrollment = "";

        private int wheelsCount;
        private int doorsCount;
        private int enginePower;
        private CarColor colorCode;

        public void addWheel()
        {
            if (wheelsCount < maxWheels)
                this.wheelsCount++;
            else
                throw new Exception("Se ha excedido el numero maximo de ruedas");
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount > 0)
            {
                for (int i = 0; i < doorsCount; i++)
                {
                    this.doorsCount++;
                }
            }
            else if (doorsCount < 0)
            {
                throw new Exception("No se puede crear un vehiculo con puertas negativas.");
            }
        }

        public void setEngine(int horsePorwer)
        {
            this.enginePower = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.colorCode = color;
        }

        public Vehicle build()
        {
            Vehicle vehicle = new Vehicle();

            generateEnrollmentB();

            vehicle.SetWheels = wheels;
            vehicle.SetDoors = doors;
            vehicle.SetEngine = engine;
            vehicle.SetCarColor = color;
            vehicle.SetEnrollment = enrollment;

            if (wheelsCount == 0)
            {
                throw new Exception("No se puede crear un vehiculo sin ruedas");
            }

            return vehicle;
        }

        private List<Wheel> createWheels(int count)
        {
            List<Wheel> wheels = new List<Wheel>();

            for()

            return wheels;
        }

        private void generateEnrollment()
        {
            string e = "";

            System.Threading.Thread.Sleep(20);
            //number = generator.Next(0, 9999);


            e = e + (char)this.generator.Next((int)'A', (int)'Z');
            e = e + (char)this.generator.Next((int)'A', (int)'Z');
            e = e + (char)this.generator.Next((int)'A', (int)'Z');
            e = e + "-";
            e = e + (char)this.generator.Next((int)'0', (int)'9');
            e = e + (char)this.generator.Next((int)'0', (int)'9');
            e = e + (char)this.generator.Next((int)'0', (int)'9');
            e = e + (char)this.generator.Next((int)'0', (int)'9');


            //if (number < 10)
            //{
            //    enB = "000" + number.ToString();
            //}
            //else if (number < 100)
            //{
            //    enB = "00" + number.ToString();
            //}
            //else if (number < 1000)
            //{
            //    enB = "0" + number.ToString();
            //}
            //else
            //{
            //    enB = number.ToString();
            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    number = generator.Next(0, 26);
            //    c = (char)('A' + number);
            //    enA = enA + c;
            //}
            //e = enA + "-" + enB;

            enrollment = e;

        }

        private void generateEnrollmentB()
        {
            enrollment = intEnrollment++.ToString();
        }
    }
}