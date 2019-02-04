using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Models.CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {

        private int wheels;
        private int doors;
        private int horsepowerValue;
        private CarColor vehicleColor;

        private int numbers = 0000;
        private readonly string[] letters = { "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "R", "S", "T", "V", "W", "X", "Y", "Z" };
        private int[] serial = { 0, 0, 0 };

        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            if (this.wheels < 4)
            {
                this.wheels++;
            }
            else
            {
                throw new ArgumentException("The number maximun of wheels is 4");
            }

        }

        public void setDoors(int doorsCount)
        {

            if (doorsCount > 0 && doorsCount <= 6)
            {
                this.doors = doorsCount;
            }

        }

        public void setEngine(int horsePower)
        {
            this.horsepowerValue = horsePower;
        }

        public void setColor(CarColor color)
        {
            this.vehicleColor = color;
        }

        public Vehicle build()
        {

            List<Wheel> wheelsList = new List<Wheel>();

            if (this.wheels > 0 && this.wheels <= 4)
            {
                for (int i = 1; i <= this.wheels; i++)
                {
                    Wheel newWheel = new Wheel();
                    wheelsList.Add(newWheel);
                }
            }
            else
            {
                throw new ArgumentException("The maximum number of wheels is 4");
            }

            List<Door> doorsList = new List<Door>();

            if (this.doors > 0 && this.doors <= 4)
            {
                for (int i = 1; i <= this.doors; i++)
                {
                    Door newDoor = new Door(false);
                    doorsList.Add(newDoor);
                }
            }
            else
            {
                throw new ArgumentException("The maximum number of doors is 6");
            }

            Engine engine = new Engine(this.horsepowerValue);

            string enrollment = generateEnrollment();

            Vehicle vehicle = new Vehicle(wheelsList, doorsList, engine, enrollment);
            return vehicle;
        }

        private string generateEnrollment()
        {
            string enrollment = "";


            if (this.numbers >= 9999)
            {
                if ((this.serial[0] & this.serial[1] & this.serial[2]) < 19)
                {
                    if ((this.serial[1] & this.serial[2]) < 19)
                    {
                        if (this.serial[2] < 19)
                        {
                            this.serial[2] += 1;
                            this.numbers = 0000;
                        }
                        else
                        {
                            this.serial[1] += 1;
                            this.serial[2] = 0;
                            this.numbers = 0000;
                        }
                    }
                    else
                    {
                        this.serial[0] += 1;
                        this.serial[1] = 0;
                        this.serial[2] = 0;
                        this.numbers = 0000;
                    }
                }
                else
                {
                    throw new SystemException("Unexpected Error in generateEnrollment");
                }
            }
            else
            {
                this.numbers += 1;
            }

            enrollment = this.numbers.ToString("D4") + "-" + this.letters[this.serial[0]] + this.letters[this.serial[1]] + this.letters[this.serial[2]];

            return enrollment;
        }
    }
}
