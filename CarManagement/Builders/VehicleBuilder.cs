using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;

        private int wheels;
        private int doors;
        private int horsepowerValue;
        private CarColor vehicleColor;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.wheels = 0;
            this.doors = 0;
            this.horsepowerValue = 0;
            this.vehicleColor = CarColor.White;
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheels < 4);
            this.wheels++;
        }

        public void setDoors(int doorsCount)
        {

            if (doorsCount > 0 && doorsCount <= 6)
            {
                this.doors = doorsCount;
            }

        }

        public void setEngine(int horsePower)
        {
            this.horsepowerValue = horsePower;
        }

        public void setColor(CarColor color)
        {
            Asserts.isEnumDefined(color);
            this.vehicleColor = color;
        }

        public Vehicle build()
        {

            List<Wheel> wheelsList = new List<Wheel>();

            if (this.wheels > 0 && this.wheels <= 4)
            {
                for (int i = 1; i <= this.wheels; i++)
                {
                    Wheel newWheel = new Wheel();
                    wheelsList.Add(newWheel);
                }
            }
            else
            {
                throw new ArgumentException("The number maximun of wheels is 4");
            }

            List<Door> doorsList = new List<Door>();

            if (this.doors <= 6)
            {
                for (int i = 1; i <= this.doors; i++)
                {
                    Door newDoor = new Door(false);
                    doorsList.Add(newDoor);
                }
            }
            else
            {
                throw new ArgumentException("The number maximun of doors is 6");
            }

            Engine engine = new Engine(this.horsepowerValue);

            IEnrollment enrollment = this.enrollmentProvider.getNew();

            CarColor carColor = this.vehicleColor;

            Vehicle vehicle = new Vehicle(wheelsList, doorsList, engine, enrollment, carColor);
            return vehicle;
        }

        public void removeWheel()
        {
            if (this.wheels > 0)
            {
                this.wheels--;
            }
            else
            {
                throw new ArgumentException("The number maximun of wheels is 4");
            }
        }
    }
}