using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    [TestCategory("JVBB Tests")]
    public class JVBBTests
    {
        public string VehiclesFilePath
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "Vehicles.xml");
            }
        }

        public bool SameVehicle(Vehicle v1, Vehicle v2)
        {
            bool isSameVehicle = true;

            if (v1.Color == v2.Color)
            {

                if (isSameVehicle && v1.WheelCount == v2.WheelCount)
                {
                    for (int i = 0; i < v1.WheelCount; i++)
                    {
                        if (v1.Wheels[i].Pressure == v2.Wheels[i].Pressure)
                        {
                        }
                        else
                        {
                            isSameVehicle = false;
                            break;
                        }
                    }

                    if (isSameVehicle && v1.DoorsCount == v2.DoorsCount)
                    {
                        for (int i = 0; i < v1.DoorsCount; i++)
                        {
                            if (v1.Doors[i].IsOpen == v2.Doors[i].IsOpen)
                            {
                            }
                            else
                            {
                                isSameVehicle = false;
                                break;
                            }
                        }
                        if (isSameVehicle && v1.Engine.HorsePower == v2.Engine.HorsePower && v1.Engine.IsStarted == v2.Engine.IsStarted)
                        {
                            if (isSameVehicle && v1.Enrollment.Number == v2.Enrollment.Number && v1.Enrollment.Serial == v2.Enrollment.Serial)
                            {

                            }
                            else
                            {
                                isSameVehicle = false;
                            }
                        }
                        else
                        {
                            isSameVehicle = false;
                        }
                    }
                    else
                    {
                        isSameVehicle = false;
                    }
                }
                else
                {
                    isSameVehicle = false;
                }
            }
            else
            {
                isSameVehicle = false;
            }
            return isSameVehicle;
        }

        [TestInitialize]
        [TestCleanup]
        public void initialize()
        {
            if (File.Exists(this.VehiclesFilePath))
                File.Delete(this.VehiclesFilePath);
        }

        [TestMethod]
        public void WrongBuildRequests_01()
        {
            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            Negassert.mustFail(() => builder.removeWheel());
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            Negassert.mustFail(() => builder.setDoors(-27));
            builder.setDoors(5);
            Negassert.mustFail(() => builder.setEngine(-40));
            Negassert.mustFail(() => builder.setEngine(0));
            builder.setEngine(100);
            Negassert.mustFail(() => builder.setColor((CarColor)27));

            Vehicle vehicle = builder.build();

            Negassert.mustFail(() => vehicle.Engine.stop());

            vehicle.Engine.start();
            Negassert.mustFail(() => vehicle.Engine.start());

            Negassert.mustFail(() => vehicle.setWheelsPressure(-1));
            vehicle.setWheelsPressure(0);
            vehicle.setWheelsPressure(0.99);
            vehicle.setWheelsPressure(2);

        }

        [TestMethod]
        public void StorageComparison()
        {
            Vehicle motoVehicle;

            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IDtoConverter dtoConverter = new DefaultDtoConverter(enrollmentProvider);
            IVehicleStorage vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);
            IVehicleStorage vehicleMemoryStorageA = new InMemoryVehicleStorage();
            IVehicleStorage vehicleMemoryStorageB = new InMemoryVehicleStorage();

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            motoVehicle = vehicleBuilder.build();

            vehicleFileStorage.set(motoVehicle);
            vehicleMemoryStorageA.set(motoVehicle);
            vehicleMemoryStorageB.set(motoVehicle);
            vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, dtoConverter);

            Vehicle memoryVehicleA = vehicleMemoryStorageA.get(enrollmentProvider.DefaultEnrollment);
            Vehicle memoryVehicleB = vehicleMemoryStorageB.get(enrollmentProvider.DefaultEnrollment);
            Vehicle fileVehicle = vehicleFileStorage.get(enrollmentProvider.DefaultEnrollment);

            Assert.IsNotNull(memoryVehicleA);
            Assert.IsNotNull(memoryVehicleB);
            Assert.IsNotNull(vehicleFileStorage);

            Assert.AreEqual(memoryVehicleA, memoryVehicleB);
            Assert.IsTrue(SameVehicle(memoryVehicleA, fileVehicle));
            Assert.IsTrue(SameVehicle(memoryVehicleB, fileVehicle));

        }

    }
}
