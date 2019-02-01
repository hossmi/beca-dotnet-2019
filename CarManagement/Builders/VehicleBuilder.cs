using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;


namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
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
            Asserts.isTrue(this.numberWheel < 4);
            this.numberWheel++;
        }

        public void removeWheel()
        {
            Asserts.isTrue(this.numberWheel >0);
            this.numberWheel--;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(doorsCount >= 0);
            Asserts.isTrue(doorsCount <= 6);
            this.numberDoor = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer > 0);
            this.engine = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            Asserts.isEnumDefined<CarColor>(color); //Si el valor es numerico devolver el color correspondiente
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
            Asserts.isTrue(this.numberWheel >0);
            //Generate doors
            List<Door> doors=createList<Door>(this.numberDoor);

            //Generate engine
            Engine engine = new Engine(this.engine);

            //Generate wheels
            List<Wheel> wheels=createList<Wheel>(this.numberWheel);

            //Generate enrollment
            IEnrollment enrollment = this.enrollmentProvider.getNew();

            //Generate vehicle
            return new Vehicle(this.color, wheels, enrollment, doors, engine);
        }
    }
}