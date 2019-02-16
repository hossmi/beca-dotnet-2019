using System;
using System.Linq;
using System.Text;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private readonly static char[] CHARACTERS = new char[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
        private readonly static int CHARACTERS_COUNT = CHARACTERS.Length;

        private readonly int[] currentSerialIndexes;
        private int currentNumber;

        public DefaultEnrollmentProvider()
        {
            this.currentSerialIndexes = new int[3];
            this.currentNumber = 0;
        }

        IEnrollment IEnrollmentProvider.getNew()
        {
            incrementEnrollment(this.currentSerialIndexes, ref this.currentNumber);

            return new PrvEnrollment
            {
                Number = this.currentNumber,
                Serial = composeSerial(this.currentSerialIndexes),
            };
        }

        private static string composeSerial(int[] serialIndexes)
        {
            return serialIndexes
                .Reverse()
                .Aggregate(new StringBuilder(), (builder, index) => builder.Append(CHARACTERS[index]))
                .ToString();
            //string serial = "";

            //for (int i = 0; i < serialIndexes.Length; i++)
            //    serial = CHARACTERS[serialIndexes[i]] + serial;

            //return serial;
        }

        private static void incrementEnrollment(int[] serialIndexes, ref int number)
        {
            number = (number + 1) % 10000;

            if (number == 0000)
                increaseSerial(0, serialIndexes);
        }

        private static void increaseSerial(int index, int[] serialIndexes)
        {
            Asserts.isTrue(index < serialIndexes.Length);

            serialIndexes[index] = (serialIndexes[index] + 1) % CHARACTERS_COUNT;

            if (serialIndexes[index] == 0)
                increaseSerial(index + 1, serialIndexes);
        }

        IEnrollment IEnrollmentImporter.import(string serial, int number)
        {
            return new PrvEnrollment()
            {
                Serial = serial,
                Number = number,
            };
        }

        private class PrvEnrollment : IEnrollment
        {
            public string Serial { get; set; }
            public int Number { get; set; }

            public override string ToString()
            {
                return $"{this.Serial}-{this.Number.ToString("0000")}";
            }
        }
    }
}