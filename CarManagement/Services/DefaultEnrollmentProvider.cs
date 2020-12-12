using CarManagement.Core.Models;
using CarManagement.Core.Services;
using System;
using System.Text;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private StringBuilder Serial;
        private icollection collection;
        public DefaultEnrollmentProvider()
        {
            this.collection = new icollection();
        }

        private class Enrollment : IEnrollment
        {
            public Enrollment(string serial, int number)
            {
                this.Serial = serial;
                this.Number = number;
            }

            public string Serial { get; }
            public int Number { get; }

            public override string ToString()
            {
                return $"{this.Serial}-{this.Number:0000}";
            }
        }
        private enum Letter
        {
            B = 0,
            C = 1,
            D = 2,
            F = 3,
            G = 4,
            H = 5,
            J = 6,
            K = 7,
            L = 8,
            M = 9,
            N = 10,
            P = 11,
            R = 12,
            S = 13,
            T = 14,
            V = 15,
            W = 16,
            X = 17,
            Y = 18,
            Z = 19
        }

        IEnrollment IEnrollmentProvider.getNew()
        {
            this.Serial = new StringBuilder(3);
            this.Serial.Append($"{(Letter)this.collection.i}{(Letter)this.collection.i2}{(Letter)this.collection.i3}");
            this.collection = this.core(this.collection);
            Enrollment enrollment = new Enrollment(Serial.ToString(), this.collection.Number);
            return enrollment;
        }
        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            return new Enrollment(serial, number);
        }
        private void sum(icollection collection, int number)
        {
            switch (number)
            {
                case 0:
                    Console.WriteLine("Has alcanzado el máximo de matrículas");
                    break;
                case 1:
                    collection.Number++;
                    break;
                case 2:
                    collection.i3 = 0;
                    collection.i2++;
                    break;
                case 3:
                    collection.i2 = 0;
                    collection.i3 = 0;
                    collection.i++;
                    break;
                case 4:
                    collection.i3++;
                    collection.Number = 0;
                    break;
            }
        }
        private icollection core(icollection collection)
        {
            if (collection.i <= collection.lenght)
                if (collection.Number.Equals(collection.total))
                    if (collection.i2.Equals(collection.lenght) && collection.i3.Equals(collection.lenght))
                        this.sum(collection, 3);
                    else if (collection.i3.Equals(collection.lenght))
                        this.sum(collection, 2);
                    else
                        this.sum(collection, 4);
                else
                    this.sum(collection, 1);
            else
                this.sum(collection, 0);
            return collection;
        }
        public class icollection
        {
            public readonly int lenght;
            public readonly int total;
            public int i;
            public int i2;
            public int i3;
            public int Number;
            
            public icollection() 
            {
                this.lenght = Enum.GetNames(typeof(Letter)).Length - 1;
                this.total = 9999;
                this.i = 0;
                this.i2 = 0;
                this.i3 = 0;
                this.Number = 0;
            }
        }
    }
}