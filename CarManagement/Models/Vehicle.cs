using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> develop

namespace CarManagement.Models
{
    public class Vehicle
    {
<<<<<<< HEAD
        private CarColor color;
        private IReadOnlyList<Wheel> wheels;
        private IEnrollment enrollment;
        private IReadOnlyList<Door> doors;
        private Engine engine;

       
        public Vehicle(CarColor color,   List<Wheel> wheels, IEnrollment enrollment, List<Door> doors, Engine engine)
        {
            this.color = color;
            this.wheels = wheels;
            this.enrollment = enrollment;
            this.doors = doors;
            this.engine = engine;
        }
=======
        private List<Wheel> wheels;
>>>>>>> develop

        public int DoorsCount
        {
            get
            {
                return this.doors.Count;
            }
        }

        public int WheelCount
        {
            get
            {
                return this.wheels.Count;
            }
        }

        public Engine Engine
        {
            get
            {
                return this.engine;
            }
        }

        public IEnrollment Enrollment
        {
            get
            {
                return this.enrollment;
            }
        }

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
<<<<<<< HEAD
        }

        public Door[] Doors
=======
        }

        public Door[] Doors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void setWheelsPressure(double pression)
>>>>>>> develop
        {
            get
            {
                return this.doors.ToArray();
            }
        }

        public void setWheelsPressure(double pression)
        {
            foreach (Wheel wheel in this.wheels )
            {
                wheel.Pressure = pression;
            }
        }
    }
}