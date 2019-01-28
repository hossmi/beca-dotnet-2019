using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private CarColor carColor;
        private int doorsCount;
        private int wheelsCount;
        private int horsePower;
        private IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            if (wheelsCount >= 4)
                throw new InvalidOperationException("Can not add more than 4 wheels.");
            this.wheelsCount++;
        }

        public void setDoors(int doorsCount)
        {
            this.doorsCount = doorsCount;
        }

        public void setEngine(int horsePower)
        {
            this.horsePower = horsePower;
        }

        public void setColor(CarColor color)
        {
            this.carColor = color;
        }


        private List<T> createElementsList<T>(int elementsQuantity) where T : class, new()
        {
            List<T> tempList = new List<T>();

            for (; elementsQuantity > 0; elementsQuantity--)
            {
                tempList.Add(new T());
            }

            return tempList;
        }

        public Vehicle build()
        {
            if (this.wheelsCount < 1)
                throw new ArgumentException("Can not build a vehicle without wheels.");

            List<Wheel> wheels = createElementsList<Wheel>(this.wheelsCount);
            List<Door> doors = createElementsList<Door>(this.doorsCount);
            Engine engine = new Engine(this.horsePower);
            IEnrollment enrollment = enrollmentProvider.getNewEnrollment();

            Vehicle vehicle = new Vehicle(doors, wheels, engine, enrollment, this.carColor);
            return vehicle;
        }

        public void removeWheel()
        {
            throw new NotImplementedException();
        }
    }
}