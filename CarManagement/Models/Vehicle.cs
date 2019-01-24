using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Wheel> wheels;
        private CarColor color;
        private Door[] door;
        private Engine moto;
        private Wheel[] wheel;
        private string enrollment;


        public Vehicle(CarColor colorCar,Door[] door,Engine motor, Wheel[] road, string enrollment)
        {
            this.color = colorCar;
            this.door = door;
            this.moto = motor;
            this.wheel = road;
            this.enrollment = enrollment;
        }

        
        
        public int DoorsCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int WheelCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Engine Engine
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Enrollment
        {
            get
            {
                return this.enrollment;
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
        }

        public Door[] Doors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void setWheelsPressure(double pression)
        {
            throw new NotImplementedException();
        }
    }
}