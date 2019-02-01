using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            if (this.wheelsCount >= 4)
                throw new InvalidOperationException("Can not add more than 4 wheels.");
            this.wheelsCount++;
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount < 0 || doorsCount > 6)
                throw new ArgumentException("Doors number must be between 0 and 6");
            else
                this.doorsCount = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            if (horsePower <= 0)
                throw new ArgumentException("Horse power must be over 0.");
            else
                this.horsePower = horsePower;
        }

        public void setColor(CarColor color)
        {
            if (Enum.IsDefined(typeof(CarColor), color) == false)
                throw new ArgumentException("Color value is not valid.");
            else
                this.carColor = color;
        }

        public IVehicle build()
        {
            if (this.wheelsCount < 1)
                throw new ArgumentException("Can not build a vehicle without wheels.");

            List<Wheel> wheels = createElementsList<Wheel>(this.wheelsCount);
            List<Door> doors = createElementsList<Door>(this.doorsCount);
            Engine engine = new Engine(this.horsePower);
            IEnrollment enrollment = this.enrollmentProvider.getNew();

            Vehicle vehicle = new Vehicle(doors, wheels, engine, enrollment, this.carColor);
            return vehicle;
        }

        public void removeWheel()
        {
            if (wheelsCount <= 0)
                throw new InvalidOperationException("There is no wheel to remove.");
            else
                wheelsCount--;
        }
    }
}