using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        Random enrollmentGenerator = new Random();

        private CarColor carColor;
        private const int ENROLLMENT_CHAR_QUANTITY = 8;
        private String enrollment;
        private List<String> usedEnrollments = new List<String>();
        private int doorsCount;
        private int wheelsCount;
        private int horsePower;

        public void addWheel()
        {
            if (wheelsCount >= 4)
                throw new InvalidOperationException("Can not add more than 4 wheels.");
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
            this.carColor = color;
        }

        private void setEnrollment()
        {
            String enrollmentPlate;

            do
            {
                enrollmentPlate = "";

                for (int enrollmentChars = 0; enrollmentChars < ENROLLMENT_CHAR_QUANTITY; enrollmentChars++)
                {
                    enrollmentPlate += enrollmentGenerator.Next((int)'A', (int)'Z').ToString();
                }

            } while (usedEnrollments.Contains(enrollmentPlate));

            this.enrollment = enrollmentPlate;
            this.usedEnrollments.Add(enrollmentPlate);
        }

        private List<T> createElementsList<T>(int elementsQuantity) where T : class, new()
        {
            List<T> tempList = new List<T>();

            for (; elementsQuantity > 0; elementsQuantity--)
            {
                tempList.Add(new T());
            }

            return tempList;
        }

        public Vehicle build()
        {
            setEnrollment();

            if (this.wheelsCount < 1)
                throw new ArgumentException("Can not build a vehicle without wheels.");

            List<Wheel> wheels = createElementsList<Wheel>(this.wheelsCount);
            List<Door> doors = createElementsList<Door>(this.doorsCount);
            Engine engine = new Engine(this.horsePower);

            return new Vehicle(doors, wheels, engine, enrollment, this.carColor);
        }
    }
}