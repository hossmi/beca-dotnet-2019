﻿using System;
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
        //int number = 0;
        //string serial = "";


        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.numberWheel = 0;
            this.numberDoor = 0;
            this.engine = 1;
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
            Asserts.isTrue(this.numberWheel > 0);
            this.numberWheel--;
        }

        public void setDoors(int doorsCount)
        {
            //Asserts.isTrue(0 < doorsCount && doorsCount <= 6);
            this.numberDoor = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {

            Asserts.isTrue(horsePorwer > 0);
            this.engine = horsePorwer;
        }

        public void setColor(CarColor color)
        {
           // Asserts.isTrue((CarColor )0 < color && color < (CarColor) 7);
            this.color = color;
        }

        public List<T> generateList<T>(int numberItem) where T : class, new()
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
            //Generamos puertas
        
            List<Door> doors = generateList<Door>(this.numberDoor);

            //Generamos motor
            
            Engine engine = new Engine(this.engine);

            //Generamos ruedas

            Asserts.isTrue(this.numberWheel > 0);
            List<Wheel> wheels = generateList<Wheel>(this.numberWheel);

            //Generamos matricula

            IEnrollment enrollment = this.enrollmentProvider .getNew();

            //Generamos coche

            return new Vehicle(this.color, wheels, enrollment, doors, engine);
        }


    }
}