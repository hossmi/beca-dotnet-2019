﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private const string sentenceCountVehicle = "SELECT count(*) FROM vehicle;";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            /*this.connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY].ToString();

            string baseFolder = ConfigurationManager.AppSettings[SCRIPTS_FOLDER_KEY].ToString();
            baseFolder = Path.Combine(Environment.CurrentDirectory, baseFolder);

            this.creationScript = ConfigurationManager.AppSettings[CREATION_SCRIPT_FILE_KEY].ToString();
            this.creationScript = Path.Combine(baseFolder, this.creationScript);

            this.destructionScript = ConfigurationManager.AppSettings[DESTRUCTION_SCRIPT_FILE_KEY].ToString();
            this.destructionScript = Path.Combine(baseFolder, this.destructionScript);

            this.fakeStorage = new ArrayVehicleStorage();*/
        }

        public int Count {
            get
            {
                return executeScalarQuery(this.connectionString, sentenceCountVehicle);
            }
        }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            if(existsEntollment(this.connectionString, enrollment))
            {
                int enrollmentId = getEnrollmentId(this.connectionString, enrollment);
            }
            throw new NotImplementedException();
        }

        private int getEnrollmentId(string connectionString, IEnrollment enrollment)
        {
            string sentenceEnrollmentID = "SELECT id FROM enrollment " +
                "WHERE serial=" + enrollment.Serial + " AND number=" + enrollment.Number + ";";

            int enrollmentID = 0;

            throw new NotImplementedException();
        }

        public IEnumerable<IVehicle> getAll()
        {
            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }

        private static int executeScalarQuery(string connectionString, string query)
        {
            int result;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);

            result = (int)command.ExecuteScalar();

            connection.Close();

            return result;
        }

        private bool existsEntollment(String connectionString, IEnrollment enrollment)
        {
            string serial = enrollment.Serial;
            int number = enrollment.Number;
            bool existeEnrollment = false;

            string sentenceExistEnrollment = "SELECT count(*) FROM enrollment " +
                "WHERE serial=" + serial + " AND number=" + number + ";";
            int enrollmentId = 0;
            enrollmentId = executeScalarQuery(connectionString, sentenceExistEnrollment);

            if (enrollmentId != 0)
            {
                existeEnrollment = true;
            }
            return existeEnrollment;
        }
    }
}