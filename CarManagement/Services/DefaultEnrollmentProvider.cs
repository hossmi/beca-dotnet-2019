using System;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private int numberEnrollment = 0;
        private readonly Random generate;

        public DefaultEnrollmentProvider()
        {
            generate = new Random();
        }

        string IEnrollmentProvider.getNewEnrollment()
        {
            string result = "";
            result = result + (char)this.generate.Next((int)'A', (int)'Z');
            result = result + (char)this.generate.Next((int)'A', (int)'Z');
            result = result + (char)this.generate.Next((int)'A', (int)'Z');
            result = result + (char)this.generate.Next((int)'0', (int)'9');
            result = result + (char)this.generate.Next((int)'0', (int)'9');
            result = result + (char)this.generate.Next((int)'0', (int)'9');
            result = result + (char)this.numberEnrollment++;

            return result;
        }
    }
}