using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private string enrollment;
        private Random generate;
        private Vehicle vehicle;
        private CarColor color;
        private int numberWheels;
        private int numberDoors;
        private int horsePorwer;

        private int numberEnrollment = 0;

        public VehicleBuilder()
        {
            this.numberWheels = 0;
            this.numberDoors = 0;
            this.horsePorwer = 0;
            
        }

        public void addWheel()
        {
            if(numberWheels < 4)
            {
                numberWheels++;
            }
            else
            {
                throw new Exception("No more wheels can be added");
            }
        }

        public string generateEnrollment()
        {
            string result = "";
            result = result + (char)this.generate.Next((int)'A', (int)'Z');
            result = result + (char)this.generate.Next((int)'A', (int)'Z');
            result = result + (char)this.generate.Next((int)'A', (int)'Z');
            result = result + (char)this.generate.Next((int)'0', (int)'9');
            result = result + (char)this.generate.Next((int)'0', (int)'9');
            result = result + (char)this.generate.Next((int)'0', (int)'9');
            result = result + (char)this.numberEnrollment++;

            return result;
        }

        public void setDoors(int doorsCount)
        {
            numberDoors = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            this.horsePorwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            if((numberDoors > 0) || (numberWheels > 0))
            {
                generate = new Random();
                enrollment = generateEnrollment();
                vehicle = new Vehicle(numberWheels, numberDoors, horsePorwer, color, enrollment);
                return vehicle;
            }
            else
            {
                throw new Exception("Missing data to build the car");
            }
            
        }
    }
}