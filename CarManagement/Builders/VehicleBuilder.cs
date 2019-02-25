using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;
        private int wheelsCount;
        private int doorsCount;
        private int horsePower;
        private CarColor color;
        private List<Door> doors;
        private Engine engine;
        private List<Wheel> wheels;
        IEnrollment enrollment;
        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
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
            this.color = color;
        }

        public Vehicle build()
        {
            this.engine = new Engine();
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();

            for (int i = 0; i < this.doorsCount; i++)
            {
                Door door = new Door();
                doors.Add(door);
            }
            for (int i = 0; i < this.wheelsCount; i++)
            {
                Wheel wheel = new Wheel();
                wheels.Add(wheel);
            }

            enrollment = enrollmentProvider.getNewEnrollment();

            return new Vehicle(this.color,this.engine,this.wheels,this.doors,this.enrollment);
        }
        //private List<T> getList<T>(List<T> list) where T : IEnumerable<T> , new()
        //{
        //    foreach (T item in list)
        //    {
        //        T items = new T();
        //        list.Add(items);
        //    }
        //    return list;
        //}
    }
}