using System;
using System.Collections.Generic;
using CarManagement.Models;
using System.Runtime.Serialization;

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

            //generateEnrollmentB();

            vehicle.SetWheels = createWheels(wheelsCount);
            vehicle.SetDoors = createDoors(doorsCount);
            vehicle.SetEngine = createEngine(enginePower);
            vehicle.SetCarColor = applyColor(colorCode);
            //vehicle.SetEnrollment = enrollment;

            if (wheelsCount == 0)
                throw new Exception("No se puede crear un vehiculo sin ruedas");
            

            return vehicle;
        }

        private List<TItem> createObject(int count) 
        {
            List<TItem> list = new List<TItem>();

            for (int i = 0; i < count; i++)
            {
                TItem obj = new TItem();
                list.Add(obj);
            }

            return list;
        }

        private List<Wheel> createWheels(int count)
        {
            List<Wheel> wheels = new List<Wheel>();

            for (int i = 0; i < count; i++)
            {
                Wheel wheel = new Wheel();
                wheels.Add(wheel);
            }

            return wheels;
        }
        private List<Door> createDoors(int count)
        {
            List<Door> doors = new List<Door>();

            for (int i = 0; i < count; i++)
            {
                Door door = new Door();
                doors.Add(door);
            }

            return doors;
        }

        private Engine createEngine(int power)
        {
            Engine engine = new Engine(power);

            return engine;
        }

        private CarColor applyColor(int code)
        {
            CarColor color = (CarColor)code;

            return color;
        }






        /*
        private void generateEnrollment()
        {
            string e = "";

            System.Threading.Thread.Sleep(20);
            //number = generator.Next(0, 9999);


            e = e + (char)this.generator.Next((int)'A', (int)'Z');
            e = e + (char)this.generator.Next((int)'A', (int)'Z');
            e = e + (char)this.generator.Next((int)'A', (int)'Z');
            e = e + "-";
            e = e + (char)this.generator.Next((int)'0', (int)'9');
            e = e + (char)this.generator.Next((int)'0', (int)'9');
            e = e + (char)this.generator.Next((int)'0', (int)'9');
            e = e + (char)this.generator.Next((int)'0', (int)'9');


            //if (number < 10)
            //{
            //    enB = "000" + number.ToString();
            //}
            //else if (number < 100)
            //{
            //    enB = "00" + number.ToString();
            //}
            //else if (number < 1000)
            //{
            //    enB = "0" + number.ToString();
            //}
            //else
            //{
            //    enB = number.ToString();
            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    number = generator.Next(0, 26);
            //    c = (char)('A' + number);
            //    enA = enA + c;
            //}
            //e = enA + "-" + enB;

            enrollment = e;

        }

        private void generateEnrollmentB()
        {
            enrollment = intEnrollment++.ToString();
        }
        */
    }
}