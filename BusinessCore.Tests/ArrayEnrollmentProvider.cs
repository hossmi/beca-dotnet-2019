using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace BusinessCore.Tests
{
    public class ArrayEnrollmentProvider : IEnrollmentProvider
    {
        private readonly IEnrollment[] enrollments;
        private int currentEnrollment;

        public ArrayEnrollmentProvider()
        {
            this.enrollments = new IEnrollment[]
            {
                new PrvEnrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                new PrvEnrollment
                {
                    Serial = "CNP",
                    Number = 1234,
                },
                new PrvEnrollment
                {
                    Serial = "BBC",
                    Number = 1024,
                },
                new PrvEnrollment
                {
                    Serial = "BBC",
                    Number = 3000,
                },
                new PrvEnrollment
                {
                    Serial = "ZZZ",
                    Number = 9001,
                },
                new PrvEnrollment
                {
                    Serial = "ZZZ",
                    Number = 3,
                },
            };
            this.currentEnrollment = 0;
        }

        public IEnrollment getNew()
        {
            IEnrollment enrollment = this.enrollments[this.currentEnrollment];
            this.currentEnrollment++;

            return enrollment;
        }

        public IEnrollment import(string serial, int number)
        {
            return new PrvEnrollment
            {
                Serial = serial,
                Number = number,
            };
        }

        public int Count { get => this.enrollments.Length; }

        private class PrvEnrollment : IEnrollment
        {
            public string Serial { get; set; }
            public int Number { get; set; }
        }
    }
}
