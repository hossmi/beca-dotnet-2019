using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count { get; }

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
            VehicleDto vehicleDto = new VehicleDto();
            string query;

            query = $@"
                     SELECT serial
                     FROM enrollment
                     WHERE serial = '{enrollment.Serial}' 
                     AND number = {enrollment.Number};";
            string serial = executeScalarQuery(this.connectionString, query).ToString();
            vehicleDto.Enrollment.Serial = serial;

            query = $@"
                     SELECT number
                     FROM enrollment
                     WHERE serial = '{enrollment.Serial}' 
                     AND number = {enrollment.Number};";
            int number = (int) executeScalarQuery(this.connectionString, query);
            vehicleDto.Enrollment.Number = number;

            query = $@"
                     SELECT id
                     FROM enrollment
                     WHERE serial = '{enrollment.Serial}' 
                     AND number = {enrollment.Number};";
            int enrollmentId = (int)executeScalarQuery(this.connectionString, query);

            query = $@"
                     SELECT engineIsStarted 
                     FROM vehicle
                     WHERE enrollmentId = '{enrollmentId}';";
            bool isStarted = (bool) executeScalarQuery(this.connectionString, query);
            vehicleDto.Engine.IsStarted = isStarted;

            query = $@"
                     SELECT engineHorsePower
                     FROM vehicle
                     WHERE enrollmentId = '{enrollmentId}';";
            int horsePower = (int) executeScalarQuery(this.connectionString, query);
            vehicleDto.Engine.HorsePower = horsePower;

            query = $@"
                     SELECT color
                     FROM vehicle
                     WHERE enrollmentId = '{enrollmentId}';";
            CarColor color = (CarColor) executeScalarQuery(this.connectionString, query);
            vehicleDto.Color = color;

            query = $@"
                     SELECT presure
                     FROM wheel
                     WHERE id = '{enrollmentId}';";
            double[] pressure = (double[]) executeScalarQuery(this.connectionString, query);
            int i = 0;
            foreach(WheelDto wheelDto in vehicleDto.Wheels)
            {
                wheelDto.Pressure = pressure[i];
                i++;
            }

            query = $@"
                     SELECT isOpen
                     FROM door
                     WHERE id = '{enrollmentId}';";
            bool[] isOpen = (bool[])executeScalarQuery(this.connectionString, query);
            i = 0;
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                doorDto.IsOpen = isOpen[i];
                i++;
            }

            return this.vehicleBuilder.import(vehicleDto);
        }

        public IEnumerable<IVehicle> getAll()
        {
            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }

        private static void executeCommand(string connectionString, string sentencies)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            using (IDbCommand command = new SqlCommand())
            {
                command.CommandText = sentencies;
                command.Connection = connection;

                connection.Open();
                int afectedRows = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static object executeScalarQuery(string connectionString, string query)
        {
            object result = null;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);

            result = command.ExecuteScalar();
 
            connection.Close();

            return result;
        }
    }
}
