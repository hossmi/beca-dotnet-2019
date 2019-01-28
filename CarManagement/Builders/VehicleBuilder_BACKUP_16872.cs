using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;


namespace CarManagement.Builders
{
    public class VehicleBuilder
        
    {
<<<<<<< HEAD
        private int  wheels = 0;
        private int doors = 0;
        private int engine = 0;
        private CarColor color;
        private string enrollment = "AAAAAAA";
        private int horsePower;
=======
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }
>>>>>>> develop

        public void addWheel()
        {
            this.wheels++;
        }

        public void setDoors(int doorsCount)
        {
            this.doors = doorsCount;
           
        }

        public void setEngine(int horsePorwer)
        {
            this.engine = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

     
        public Vehicle build()
        {

            //Generar los objetos

            //genero puertas
            List<Door> doors = new List<Door>();
            
            for (int i = 0; i < this.doors; i++)
            {
                doors.Add(new Door());
            }
            //genero ruedas
            List<Wheel> wheels = new List<Wheel>();
            for (int i = 0; i < this.wheels ; i++)
            {
                wheels.Add(new Wheel());
            }
            //genero motor
    

            Engine engine = new Engine(horsePower);

            
            return new Vehicle(this.color,doors,engine,wheels,enrollment);
        }
          
    }
}