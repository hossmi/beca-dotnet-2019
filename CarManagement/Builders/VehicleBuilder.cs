using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private List<Door> doors = new List<Door>();
        private List<Wheel> wheels = new List<Wheel>();
        private Engine engine;
        private CarColor carColor;
        private int enrollmentCharQuantity = 4;
        private String enrollment;
        private List<String> usedEnrollments = new List<String>();

        public void addWheel()
        {
            if (wheels.Count >= 4)
                throw new InvalidOperationException("Can not add more than 4 wheels.");
            wheels.Add(new Wheel());
        }

        public void setDoors(int doorsCount)
        {
            for (; doorsCount > 0; doorsCount--)
            {
                doors.Add(new Door());
            }
        }

        public void setEngine(int horsePower)
        {
            engine = new Engine(horsePower);
        }

        public void setColor(CarColor carColor)
        {
            this.carColor = carColor;
        }

        private void setEnrollment()
        {
            Random enrollmentGenerator = new Random();
            String enrollmentPlate;

            do
            {
                enrollmentPlate = "";

                for (int enrollmentChars = 0; enrollmentChars < enrollmentCharQuantity; enrollmentChars++)
                {
                    enrollmentPlate += enrollmentGenerator.Next(0, 9).ToString();
                }

            } while (usedEnrollments.Contains(enrollmentPlate));

            enrollment = enrollmentPlate;
            usedEnrollments.Add(enrollment);
        }

        public Vehicle build()
        {
            setEnrollment();

            if (wheels.Count < 1)
                throw new InvalidOperationException("Can not build a vehicle without wheels.");

            return new Vehicle(doors, wheels, engine, enrollment, carColor);
        }
    }
}