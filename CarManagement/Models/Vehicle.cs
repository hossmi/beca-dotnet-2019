﻿using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private Wheel[] wheels;
        private Door door;
        private Engine engine;
        private CarColor color;

        public Vehicle()
        {

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

        public IEnrollment Enrollment
        {
            get
            {
                throw new NotImplementedException();
            }
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