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

        public List<T> createList<T>(int numberItem) where T : class, new()
        {
            List<T> items = new List<T>();
            for (int x = 0; x < numberItem; x++)
            {
                items.Add(new T());
            }
            return items;
        }
        public Vehicle build()
        {
            //Generamos puertas
            List<Door> doors=createList<Door>(this.numberDoor);

            //Generamos motor
            Engine engine = new Engine(this.engine);

            //Generamos ruedas
            List<Wheel> wheels=createList<Wheel>(this.numberWheel);

            //Generamos matricula
            IEnrollmentProvider enrollmentget = new DefaultEnrollmentProvider();
            IEnrollment enrollment = enrollmentget.getNewEnrollment();

            //Generamos coche
            return new Vehicle(this.color, wheels, enrollment, doors, engine);
        }
    }
}