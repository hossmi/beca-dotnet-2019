using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private static int intEnrollment;
        const int maxWheels = 4;

        private int wheelsCount;
        private int doorsCount;
        private int enginePower;
        private int colorCode;

        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            if (wheelsCount < maxWheels)
                this.wheelsCount++;
            else
                throw new Exception("Se ha excedido el numero maximo de ruedas");
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount > 0)
            {
                for (int i = 0; i < doorsCount; i++)
                {
                    this.doorsCount++;
                }
            }
            else if (doorsCount < 0)
            {
                throw new Exception("No se puede crear un vehiculo con puertas negativas.");
            }
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
            this.colorCode = (int)color;
        }

        public Vehicle build()
        {
            Vehicle vehicle = new Vehicle();

            vehicle.SetWheels = CreateObject<Wheel>(wheelsCount);
            vehicle.SetDoors = CreateObject<Door>(doorsCount);
            vehicle.SetEngine = CreateEngine(enginePower);
            vehicle.SetCarColor = applyColor(colorCode);
            

            if (wheelsCount == 0)
                throw new Exception("No se puede crear un vehiculo sin ruedas");
            

            return vehicle;
        }

        private List<TItem> CreateObject<TItem>(int count) where TItem: class, new()
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

        private CarColor applyColor(int code)
        {
            CarColor color = (CarColor)code;

            return color;
        }
    }
}