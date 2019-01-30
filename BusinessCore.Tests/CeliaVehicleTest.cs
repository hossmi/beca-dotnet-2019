using System;
using CarManagement.Builders;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class CeliaVehicleTest
    {
        [TestMethod]
        public void cannot_removeWheell_if_there_is_no()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            builder.addWheel();
            builder.addWheel();
            builder.removeWheel();
            builder.removeWheel();

            Negassert.mustFail(() => builder.removeWheel());
        }
    }
}