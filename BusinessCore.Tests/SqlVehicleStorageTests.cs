using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
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
            throw new NotImplementedException();
        }

        private static void create(string connectionString, string creationScript)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            String query = @"
                    USE [CarManagement]
                    CREATE TABLE [enrollment](
	                    [serial] [char](3) NOT NULL,
	                    [number] [smallint] NOT NULL,
	                    [id] [int] IDENTITY(1,1) NOT NULL,
                        CONSTRAINT [PK_enrollment] PRIMARY KEY NONCLUSTERED ([id] ASC)
                    )
                    CREATE UNIQUE CLUSTERED INDEX [IX_enrollment] ON [dbo].[enrollment]
                    (
	                    [serial] ASC,
	                    [number] ASC
                    )";
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.ExecuteNonQuery();
            }

            query = @"USE [CarManagement]
                    CREATE TABLE [vehicle](
	                [color] [smallint] NOT NULL,
	                [engineHorsePower] [smallint] NOT NULL,
	                [engineIsStarted] [bit] NOT NULL,
	                [enrollmentId] [int] NOT NULL,
                    CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED 
                    ([enrollmentId] ASC
                    ))
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
	                [vehicleId] [int] NOT NULL,
	                [isOpen] [bit] NOT NULL,
                    CONSTRAINT [PK_door] PRIMARY KEY CLUSTERED 
                (
	                [id] ASC,
	                [vehicleId] ASC
                ))

                ALTER TABLE [door] WITH CHECK ADD CONSTRAINT [FK_door_vehicle] FOREIGN KEY([vehicleId]) REFERENCES [vehicle] ([enrollmentId])";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.ExecuteNonQuery();
            }

            query = @"USE [CarManagement]
                    CREATE TABLE [wheel](
	                    [id] [int] NOT NULL,
	                    [vehicleId] [int] NOT NULL,
	                    [pressure] [float] NOT NULL,
                     CONSTRAINT [PK_wheel] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC,
	                    [vehicleId] ASC
                    ))
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
                using (SqlCommand command = new SqlCommand(query, conn)) {
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
