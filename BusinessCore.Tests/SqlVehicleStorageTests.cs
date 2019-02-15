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
        private const string CREATE_TABLE = @"
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
            drop(this.connectionString);
        }

        [TestMethod]
        public void create_and_drop_works()
        {
        }

        private static void create(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();



            conn.Close();
            
        }

        private static void drop(string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                String query = @"USE [CarManagement] IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[door]') AND type in (N'U')) DROP TABLE [dbo].[door]";
                using (SqlCommand command = new SqlCommand(query, conn)) {
                    command.ExecuteNonQuery();
                }
                query = @"USE [CarManagement]
                         DROP TABLE [wheel]  IF EXISTS";
                using (SqlCommand command = new SqlCommand(query, conn)) {
                    command.ExecuteNonQuery();
                }
                query = @"USE [CarManagement]
                         DROP TABLE [vehicle]  IF EXISTS";
                using (SqlCommand command = new SqlCommand(query, conn)) {
                    command.ExecuteNonQuery();
                }
                query = @"USE [CarManagement]
                         DROP TABLE [enrollment]  IF EXISTS";
                using (SqlCommand command = new SqlCommand(query, conn)) {
                    command.ExecuteNonQuery();
                }

                conn.Close();

            }

        }

    }
}
