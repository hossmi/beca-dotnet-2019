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

        public int Count {
            get
            {
                return (int) executeScalarQuery(this.connectionString, "SELECT count(*) FROM vehicle");
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
            VehicleDto vehicleDto = new VehicleDto();
            string query;

            query = $@"
                     SELECT *
                     FROM enrollment
                     WHERE serial = '{enrollment.Serial}' 
                     AND number = {enrollment.Number};";            
            vehicleDto.Enrollment.Serial = executeReaderQuery(this.connectionString, query, "serial").ToString();
            vehicleDto.Enrollment.Number = (int) executeReaderQuery(this.connectionString, query, "number");

            query = $@"
                     SELECT id
                     FROM enrollment
                     WHERE serial = '{enrollment.Serial}' 
                     AND number = {enrollment.Number};";
            int enrollmentId = (int) executeReaderQuery(this.connectionString, query, "id");

            query = $@"
                     SELECT * 
                     FROM vehicle
                     WHERE enrollmentId = '{enrollmentId}';";
            vehicleDto.Engine.IsStarted = (bool) executeReaderQuery(this.connectionString, query, "engineIsStarted");
            vehicleDto.Engine.HorsePower = (int) executeReaderQuery(this.connectionString, query, "engineHorsePower");
            vehicleDto.Color = (CarColor) executeReaderQuery(this.connectionString, query, "color");

            query = $@"
                     SELECT presure
                     FROM wheel
                     WHERE vehicleId = '{enrollmentId}';";
            double[] pressure = (double[]) executeReaderQuery(this.connectionString, query, "pressure");
            int i = 0;
            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                wheelDto.Pressure = pressure[i];
                i++;
            }

            query = $@"
                     SELECT isOpen
                     FROM door
                     WHERE vehicleId = '{enrollmentId}';";
            bool[] isOpen = (bool[]) executeReaderQuery(this.connectionString, query, "isOpen");
            i = 0;
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                doorDto.IsOpen = isOpen[i];
                i++;
            }


            /*query = $@"
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
            }*/

            return this.vehicleBuilder.import(vehicleDto);
        }

        public IEnumerable<IVehicle> getAll()
        {
            IList<IVehicle> vehicles = new List<IVehicle>();

            string query = $@"
                     SELECT *
                     FROM vehicle;";
            int[] enrollments = (int[]) executeReaderQuery(this.connectionString, query, "enrollmentId");

            for (int i = 0; i < this.Count; i++)
            {
                //vehicles.Add(get());
            }

            return vehicles;
        }

        public void set(IVehicle vehicle)
        {
            //VehicleDto vehicleDto = new VehicleDto();
            string query;

            query = $@"
                     SELECT *
                     FROM enrollment
                     WHERE serial = '{vehicle.Enrollment.Serial}' 
                     AND number = {vehicle.Enrollment.Number};";
            string serial = executeReaderQuery(this.connectionString, query, "serial").ToString();
            int number = (int)executeReaderQuery(this.connectionString, query, "number");
            int enrollmentId = (int)executeReaderQuery(this.connectionString, query, "id");                     

            query = $@"
                     SELECT * 
                     FROM vehicle
                     WHERE enrollmentId = '{enrollmentId}';";
            int isStarted = (int) executeReaderQuery(this.connectionString, query, "engineIsStarted");
            int horsePower = (int) executeReaderQuery(this.connectionString, query, "engineHorsePower");
            CarColor color = (CarColor) executeReaderQuery(this.connectionString, query, "color");

            query = $@"
                     SELECT presure
                     FROM wheel
                     WHERE vehicleId = '{enrollmentId}';";
            float[] pressure = (float[]) executeReaderQuery(this.connectionString, query, "pressure");
            
            query = $@"
                     SELECT isOpen
                     FROM door
                     WHERE vehicleId = '{enrollmentId}';";
            int[] isOpen = (int[]) executeReaderQuery(this.connectionString, query, "isOpen");

            query = $@"INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIstarted) 
                    VALUES ({enrollmentId}, {color}, {horsePower}, {isStarted});";
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

        private static object executeReaderQuery(string connectionString, string query, string atributo)
        {
            object[] result = null;
            int i = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {                
                result[i] = reader[atributo];
                i++;
            }

            connection.Close();

            return result;
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
