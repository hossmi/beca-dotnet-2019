using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private int numberWheel;
        private int numberDoor;
        private int engine;
        private CarColor color;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.numberWheel = 0;
            this.numberDoor = 0;
            this.engine = 0;
            this.color = CarColor.Red;
            this.enrollmentProvider = enrollmentProvider;
        }

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

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            List<Door> doors= new List<Door>();
            for (int x=0; x<this.numberDoor;x++)
            {
                doors.Add(new Door());
            }


            Engine engine = new Engine();

            List<Wheel> wheels = new List<Wheel>();
            for (int x=0; x<this.numberWheel;x++)
            {
                wheels.Add(new Wheel());
            }

            String enrollment = Math.Ceiling((decimal)DateTime.Now.Month).ToString();

            return new Vehicle(this.color,doors,engine,wheels, enrollment);
        }
    }
}