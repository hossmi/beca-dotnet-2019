using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
   

    public class SqlVehicleStorage : IVehicleStorage
    {
        private const string SELECT_FROM_VEHICLE = @"
                        SELECT v.[enrollmentId]
                              ,v.[color]
                              ,v.[engineHorsePower]
                              ,v.[engineIsStarted]
	                          ,e.serial
	                          ,e.number
                          FROM [vehicle] v
                          INNER JOIN enrollment e ON v.enrollmentId = e.id ;";

        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count
        {
            get
            {
                return (int)executeScalarQuery(this.connectionString, "SELECT count(*) FROM vehicle");
            }
        }

        public void clear()
        {
            string sentenceClearEnrollment = "DELETE FROM enrollment";
            string sentenceClearVehicle = "DELETE FROM vehicle";
            string sentenceClearWherl = "DELETE FROM wheel";
            string sentenceClearDoor = "DELETE FROM door";
            List<String> deleteTables = new List<String>();
            deleteTables.Add(sentenceClearDoor);
            deleteTables.Add(sentenceClearWherl);
            deleteTables.Add(sentenceClearVehicle);
            deleteTables.Add(sentenceClearEnrollment);

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                foreach (String sentenceDeleteTable in deleteTables)
                {
                    SqlCommand deleteComand = new SqlCommand(sentenceDeleteTable, connection);
                    deleteComand.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            VehicleDto vehicleDto = new VehicleDto();
            string query;
            int enrollmentId;
            bool exist = false;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                query = $@"
                     SELECT id
                     FROM enrollment
                     CONTAINS serial = '{enrollment.Serial}' 
                     AND number = {enrollment.Number};";
                exist = (bool)command.ExecuteScalar();
                if (exist)
                {
                    query = $@"
                         SELECT *
                         FROM enrollment
                         WHERE serial = '{enrollment.Serial}' 
                         AND number = {enrollment.Number};";
                    enrollmentId = (int)command.ExecuteScalar();
                    query = $@"
                         SELECT enrollmentId
                         FROM vehicle
                         CONTAINS enrollmentId = {enrollmentId};";
                    exist = (bool)command.ExecuteScalar();
                }
            }
            Asserts.isTrue(exist);
            query = $@"
                     SELECT *
                     FROM enrollment
                     WHERE serial = '{enrollment.Serial}' 
                     AND number = {enrollment.Number};";
            vehicleDto.Enrollment.Serial = executeReaderQuery(this.connectionString, query, "serial").ToString();
            vehicleDto.Enrollment.Number = (int)executeReaderQuery(this.connectionString, query, "number");
            enrollmentId = (int)executeReaderQuery(this.connectionString, query, "id");

            query = $@"
                     SELECT * 
                     FROM vehicle
                     WHERE enrollmentId = '{enrollmentId}';";
            vehicleDto.Engine.IsStarted = (bool)executeReaderQuery(this.connectionString, query, "engineIsStarted");
            vehicleDto.Engine.HorsePower = (int)executeReaderQuery(this.connectionString, query, "engineHorsePower");
            vehicleDto.Color = (CarColor)executeReaderQuery(this.connectionString, query, "color");

            query = $@"
                     SELECT presure
                     FROM wheel
                     WHERE vehicleId = '{enrollmentId}';";
            double[] pressure = (double[])executeReaderQuery(this.connectionString, query, "pressure");
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
            bool[] isOpen = (bool[])executeReaderQuery(this.connectionString, query, "isOpen");
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
            IList<IVehicle> vehicles = new List<IVehicle>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = SELECT_FROM_VEHICLE;
                using (SqlDataReader readerVehicle = command.ExecuteReader())
                {
                    while (readerVehicle.Read())
                    {
                        VehicleDto vehicleDto = new VehicleDto();

                        int enrollmentId = (int)readerVehicle["enrollmentId"];
                        vehicleDto.Color = (CarColor)Convert.ToInt32(readerVehicle["color"]);
                        vehicleDto.Engine = new EngineDto
                        {
                            HorsePower = Convert.ToInt16(readerVehicle["engineHorsePower"]),
                            IsStarted = Convert.ToBoolean(readerVehicle["engineIsStarted"]),
                        };
                        vehicleDto.Enrollment = new EnrollmentDto
                        {
                            Serial = readerVehicle["serial"].ToString(),
                            Number = Convert.ToInt16(readerVehicle["number"]),
                        };

                        List<WheelDto> wheelDtos = readWheels(connection, enrollmentId);
                        List<DoorDto> doorDtos = readDoors(connection, enrollmentId);

                        vehicleDto.Wheels = wheelDtos.ToArray();
                        vehicleDto.Doors = doorDtos.ToArray();

                        IVehicle vehicle = this.vehicleBuilder.import(vehicleDto);
                        vehicles.Add(vehicle);
                    }
                }
                connection.Close();
            }
            return vehicles;
        }

        private static List<WheelDto> readWheels(SqlConnection connection, int vehicleId)
        {
            List<WheelDto> wheelsDto = new List<WheelDto>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $@"SELECT * FROM wheel
                                        WHERE vehicleId = {vehicleId};";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WheelDto wheelDto = new WheelDto();
                        wheelDto.Pressure = Convert.ToDouble(reader["pressure"]);
                        wheelsDto.Add(wheelDto);
                    }
                }
            }
            return wheelsDto;
        }

        private static List<DoorDto> readDoors(SqlConnection connection, int vehicleId)
        {
            List<DoorDto> doorsDto = new List<DoorDto>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $@"SELECT * FROM door
                                        WHERE vehicleId = {vehicleId};";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoorDto doorDto = new DoorDto();
                        doorDto.IsOpen = Convert.ToBoolean(reader["isOpen"]);
                        doorsDto.Add(doorDto);
                    }
                }
            }
            return doorsDto;
        }

        public void set(IVehicle vehicle)
        {
            string query, sentences;

            sentences = $@"INSERT INTO enrollment (serial, number) 
                    VALUES ({vehicle.Enrollment.Serial}, {vehicle.Enrollment.Number});";
            executeCommand(this.connectionString, sentences);

            query = $@"
                     SELECT id
                     FROM enrollment
                     WHERE serial = '{vehicle.Enrollment.Serial}' 
                     AND number = {vehicle.Enrollment.Number};";
            int enrollmentId = (int)executeScalarQuery(this.connectionString, query);

            sentences = $@"INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted) 
                    VALUES ({enrollmentId}, {vehicle.Color}, {vehicle.Engine.HorsePower}, {vehicle.Engine.IsStarted});";
            executeCommand(this.connectionString, sentences);

            foreach (IWheel wheel in vehicle.Wheels)
            {
                sentences = $@"INSERT INTO wheel (vehicleId, pressure) 
                    VALUES ({enrollmentId}, {wheel.Pressure});";
                executeCommand(this.connectionString, sentences);
            }

            foreach (IDoor door in vehicle.Doors)
            {
                sentences = $@"INSERT INTO door (vehicleId, isOpen) 
                    VALUES ({enrollmentId}, {door.IsOpen});";
                executeCommand(this.connectionString, sentences);
            }
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
