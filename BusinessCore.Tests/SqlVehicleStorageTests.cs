using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestCategory("Requires SQL Server connection")]
    [TestClass]
    public class SqlVehicleStorageTests
    {
        private const string CONNECTION_STRING_KEY = "CarManagerConnectionString";
        private const string CREATION_SCRIPT_FILE_KEY = "CreationScriptFile";
        private const string DESTRUCTION_SCRIPT_FILE_KEY = "DestructionScriptFile";
        private const string SCRIPTS_FOLDER_KEY = "scriptsFolder";

        private const string INSERT_ENROLLMENT = @"INSERT INTO [enrollment] ([serial],[number]) VALUES (@serial,@number)";
        private const string INSERT_VEHICLE = @"INSERT INTO [vehicle] ([enrollmentId],[color],[engineHorsePower],[engineIsStarted]) VALUES (@enrollmentId,@color,@engineHorsePower,@engineIsStarted)";
        private const string INSERT_DOOR = @"INSERT INTO [door]  ([vehicleId],[isOpen]) VALUES (@vehicleId,isOpen)";
        private const string INSERT_WHEEL = @"INSERT INTO [wheel] ([pressure],[vehicleId]) VALUES (@pressure, @vehicleId)";

        private readonly string connectionString;
        private readonly string creationScript;
        private readonly string destructionScript;
        private readonly IVehicleStorage fakeStorage;

        public SqlVehicleStorageTests()
        {
            this.connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY].ToString();

            string baseFolder = ConfigurationManager.AppSettings[SCRIPTS_FOLDER_KEY].ToString();
            baseFolder = Path.Combine(Environment.CurrentDirectory, baseFolder);

            this.creationScript = ConfigurationManager.AppSettings[CREATION_SCRIPT_FILE_KEY].ToString();
            this.creationScript = Path.Combine(baseFolder, this.creationScript);

            this.destructionScript = ConfigurationManager.AppSettings[DESTRUCTION_SCRIPT_FILE_KEY].ToString();
            this.destructionScript = Path.Combine(baseFolder, this.destructionScript);

            this.fakeStorage = new ArrayVehicleStorage();
        }

        [TestInitialize]
        public void recreate()
        {
            drop(this.connectionString, this.destructionScript);
            create(this.connectionString, this.creationScript);
            fullfillWithSampleData(this.connectionString, this.fakeStorage.getAll());
        }

        [TestCleanup]
        public void cleanUp()
        {
            drop(this.connectionString, this.destructionScript);
        }

        [TestMethod]
        public void create_and_drop_works()
        {

        }

        private static void fullfillWithSampleData(string connectionString, IEnumerable<IVehicle> vehicles)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            foreach (IVehicle vehicle in vehicles)
            {
                String enrollmentId;
                using (SqlCommand command = new SqlCommand(INSERT_ENROLLMENT, conn)) {

                    command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                    command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                    command.ExecuteNonQuery();
                }


                string selectEnrollmentId = @"SELECT [id] FROM [enrollment] WHERE [serial] = @serial AND [number] = @number";
                using (SqlCommand command = new SqlCommand(selectEnrollmentId, conn))
                {
                    command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                    command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                    enrollmentId = command.ExecuteScalar().ToString();
                }


                using (SqlCommand command = new SqlCommand(INSERT_VEHICLE, conn)) {

                    command.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                    command.Parameters.AddWithValue("@color", vehicle.Color);
                    command.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                    command.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                }

                foreach (IDoor door in vehicle.Doors)
                {
                    using (SqlCommand command = new SqlCommand(INSERT_VEHICLE, conn)) {

                        command.Parameters.AddWithValue("@vehicleId", enrollmentId);
                        command.Parameters.AddWithValue("@isOpen", door.IsOpen);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (IWheel wheel in vehicle.Wheels)
                {
                    using (SqlCommand command = new SqlCommand(INSERT_WHEEL, conn)) {

                        command.Parameters.AddWithValue("@pressure", wheel.Pressure);
                        command.Parameters.AddWithValue("@vehicleId", enrollmentId);
                    }
                }
            }
        }

        private static void create(string connectionString, string creationScript)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            String query = @"
                        USE[CarManagement]
        
                        CREATE TABLE [enrollment](
                            [serial][varchar](3) NOT NULL,
                            [number] [smallint] NOT NULL,
                            [id] [int] IDENTITY(1,1) NOT NULL,
                        CONSTRAINT[PK_enrollment] PRIMARY KEY NONCLUSTERED ([id] ASC)
                            )

                        CREATE CLUSTERED INDEX[IX_enrollment] ON [enrollment](
                            [serial] ASC,
                            [number] ASC
                            )";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.ExecuteNonQuery();
            }

            query = @"
                USE [CarManagement]

                CREATE TABLE [vehicle](
	                [enrollmentId] [int] NOT NULL,
	                [color] [smallint] NULL,
	                [engineHorsePower] [smallint] NULL,
	                [engineIsStarted] [bit] NULL,
                CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED (
	                [enrollmentId] ASC)
                    )

                ALTER TABLE [vehicle]  WITH CHECK ADD  CONSTRAINT [FK_vehicle_enrollment] FOREIGN KEY([enrollmentId])
                REFERENCES [enrollment] ([id])";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.ExecuteNonQuery();
            }

            query = @"
                    USE [CarManagement]

                    CREATE TABLE [door](
	                    [id] [int] NOT NULL,
	                    [vehicleId] [float] NOT NULL,
	                    [isOpen] [bit] NOT NULL,
                    CONSTRAINT [PK_door] PRIMARY KEY CLUSTERED ([id] ASC)
                        )
                        
                    ALTER TABLE [vehicle]  WITH CHECK ADD  CONSTRAINT [FK_vehicle_enrollment1] FOREIGN KEY([enrollmentId])
                    REFERENCES [enrollment] ([id])";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.ExecuteNonQuery();
            }


            query = @"
                USE [CarManagement]

                CREATE TABLE [wheel](
	                [id] [int] NOT NULL,
	                [pressure] [float] NOT NULL,
	                [vehicleId] [int] NOT NULL,
                CONSTRAINT [PK_wheel] PRIMARY KEY CLUSTERED (
	                [vehicleId] ASC)
                    )

                ALTER TABLE [wheel]  WITH CHECK ADD  CONSTRAINT [FK_wheel_vehicle] FOREIGN KEY([id]) REFERENCES [vehicle] ([enrollmentId])";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.ExecuteNonQuery();
            }

            conn.Close();

        }

        private static void drop(string connectionString, string destructionScript)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                String query = @"USE [CarManagement] IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[door]') AND type in (N'U')) DROP TABLE [dbo].[door]";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.ExecuteNonQuery();
                }
                query = @"USE [CarManagement] IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wheel]') AND type in (N'U')) DROP TABLE [dbo].[wheel]";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.ExecuteNonQuery();
                }
                query = @"USE [CarManagement] IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vehicle]') AND type in (N'U')) DROP TABLE [dbo].[vehicle]";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.ExecuteNonQuery();
                }
                query = @"USE [CarManagement] IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[enrollment]') AND type in (N'U')) DROP TABLE [dbo].[enrollment]";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.ExecuteNonQuery();
                }

                conn.Close();

            }
        }
    }
}
