using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private CarColor color;
        private int numberWheels;
        private int numberDoors;
        private int horsePorwer;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.numberDoors = 0;
            this.numberWheels = 0;
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

        private List<T> create<T>(int nItems) where T : class, new()
        {
            List<T> items = new List<T>();
            for (int i = 0; i < nItems; i++)
            {
                T aux = new T();
                items.Add(aux);
            }

            return items;
        }
        

        public Vehicle build()
        {
            Asserts.isTrue(numberWheels > 0);

            Engine engine = new Engine(this.horsePorwer);
            List<Door> doors = create<Door>(this.numberDoors);
            List<Wheel> wheels = create<Wheel>(this.numberWheels);

            Vehicle vehicle = new Vehicle(wheels, doors, engine, color, enrollmentProvider.getNewEnrollment());
            return vehicle;
        }
    }
}