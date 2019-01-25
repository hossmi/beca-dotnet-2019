using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        const int MAX_WHEELS = 4;
        private int wheelsCount;
        private int doorsCount;
        private int enginePower;
        private CarColor colorCode;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.wheelsCount = 0;
            this.doorsCount = 0;
            this.enginePower = 0;
            this.colorCode = 0;
        }

        public void addWheel()
        {
            if (this.wheelsCount < MAX_WHEELS)
                this.wheelsCount++;
            else
                throw new Exception("Se ha excedido el numero maximo de ruedas");
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount > 0)
                this.doorsCount = doorsCount;
            else if (doorsCount < 0)
                throw new ArgumentException("No se puede crear un vehiculo con puertas negativas.");
        }

        public void setEngine(int horsePorwer)
        {
            this.enginePower = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            if (Enum.IsDefined(typeof(CarColor), color) == false)
                throw new ArgumentException($"Parameter {nameof(color)} has not a valid value.");
            else
                this.colorCode = color;
        }

        public Vehicle build()
        {
            if (this.wheelsCount == 0)
                throw new Exception("No se puede crear un vehiculo sin ruedas");

            List<Wheel> wheels = CreateObject<Wheel>(wheelsCount);
            List<Door> doors = CreateObject<Door>(doorsCount);
            Engine engine = CreateEngine(enginePower);
            string enrollment = enrollmentProvider.getNewEnrollment();
             
            Vehicle vehicle = new Vehicle(wheels, doors, engine, colorCode, enrollment);

            return vehicle;
        }

        private List<TItem> CreateObject<TItem>(int count) where TItem : class, new()
        {
            List<TItem> list = new List<TItem>();
            for (int i = 0; i < count; i++)
            {
                TItem obj = new TItem();
                list.Add(obj);
            }
            return list;
        }

        private Engine CreateEngine(int power)
        {
            Engine engine = new Engine(power);
            return engine;
        }
    }
}