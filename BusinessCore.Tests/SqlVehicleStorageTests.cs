using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class SqlVehicleStorageTests
    {
        public SqlVehicleStorageTests()
        {
            
        }

        [TestInitialize]
        public void drop_and_create()
        {
            drop();

        }

        [TestCleanup]
        public void drop()
        {

        }
    }
}
