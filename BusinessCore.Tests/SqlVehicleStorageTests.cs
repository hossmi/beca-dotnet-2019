using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestCategory("Requires SQL Server connection")]
    [TestClass]
    public class SqlVehicleStorageTests
    {
        private const string ConnectionStringKey = "CarManagerConnectionString";
        private readonly string connectionString;

        public SqlVehicleStorageTests()
        {
            Assert.IsTrue(ConfigurationManager.AppSettings.AllKeys.Contains(ConnectionStringKey));
            this.connectionString = ConfigurationManager.AppSettings[ConnectionStringKey].ToString();
        }

        [TestInitialize]
        public void recreate()
        {
            drop(this.connectionString);
            create(this.connectionString);
        }

        [TestCleanup]
        public void cleanUp()
        {
            //drop(this.connectionString);
        }

        [TestMethod]
        public void create_and_drop_works()
        {
        }

        private static void create(string connectionString)
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

        private static void drop(string connectionString)
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
