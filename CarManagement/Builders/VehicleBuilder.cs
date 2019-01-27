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
        int number = 0;
        string serial = "";


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
            //Generamos puertas
            List<Door> doors = new List<Door>();
            for (int x = 0; x < this.numberDoor; x++)
            {
                doors.Add(new Door());
            }

            //Generamos motor
            Engine engine = new Engine(this.engine);

            //Generamos ruedas
            List<Wheel> wheels = new List<Wheel>();
            for (int x = 0; x < this.numberWheel; x++)
            {
                wheels.Add(new Wheel());
            }

            //Generamos matricula
            IEnrollmentProvider enrollmentget = new DefaultEnrollmentProvider(serial, number);
            IEnrollment enrollment = enrollmentget.getNewEnrollment();

            //Generamos coche
            return new Vehicle(this.color, wheels, enrollment, doors, engine);
        }
    }
}