﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void factorial_of_5_returns_120()
        {
            long result = NumberTools.factorial(5);

            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void factorial_of_15_returns_1307674368000()
        {
            long result = NumberTools.factorial(15);

            Assert.AreEqual(1307674368000, result);
        }

        [TestMethod]
        public void fibonacci_of_0_returns_0()
        {
            int result = NumberTools.fibonacci(0);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void fibonacci_of_1_returns_1()
        {
            int result = NumberTools.fibonacci(1);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void fibonacci_of_2_returns_1()
        {
            int result = NumberTools.fibonacci(2);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void fibonacci_of_8_returns_21()
        {
            int result = NumberTools.fibonacci(8);

            Assert.AreEqual(21, result);
        }
    }
}
