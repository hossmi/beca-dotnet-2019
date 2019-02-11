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
                        SELECT v.enrollmentId AS id
                            ,v.color
                            ,v.engineHorsePower
                            ,v.engineIsStarted
	                        ,e.serial
	                        ,e.number
	                        ,d.isOpen
	                        ,d.id
	                        ,w.pressure
	                        ,w.id
                        FROM [vehicle] v
                        INNER JOIN enrollment e ON v.enrollmentId = e.id
                        INNER JOIN wheel w ON v.enrollmentId = w.vehicleId
                        INNER JOIN door d ON v.enrollmentId = d.vehicleId
                        ORDER BY v.enrollmentId ASC, d.id ASC, w.id ASC;";

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
            IEnumerable<IVehicle> vehicles;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = SELECT_FROM_VEHICLE;
                using (SqlDataReader readerVehicle = command.ExecuteReader())
                {
                    vehicles = readVehicles(readerVehicle);
                }

                while (readerVehicle.Read())
                {
                    VehicleDto vehicleDto = new VehicleDto();

                    int enrollmentId = (int)readerVehicle["enrollmentId"];
                    vehicleDto.Color = (CarColor)Convert.ToInt32(readerVehicle["color"]);
                    vehicleDto.Engine.HorsePower = Convert.ToInt16(readerVehicle["engineHorsePower"]);
                    vehicleDto.Engine.IsStarted = Convert.ToBoolean(readerVehicle["engineIsStarted"]);
                    vehicleDto.Enrollment.Serial = readerVehicle["serial"].ToString();
                    vehicleDto.Enrollment.Number = Convert.ToInt16(readerVehicle["number"]);

                    List<WheelDto> wheelDtos = readWheels(connection, enrollmentId);
                    List<DoorDto> doorDtos = readDoors(connection, enrollmentId);

                    vehicleDto.Wheels = wheelDtos.ToArray();
                    vehicleDto.Doors = doorDtos.ToArray();

                    IVehicle vehicle = this.vehicleBuilder.import(vehicleDto);
                    vehicles.Add(vehicle);
                }
                connection.Close();
            }

            return vehicles;
        }

        private static IEnumerable<IVehicle> readVehicles(SqlDataReader reader, IVehicleBuilder vehicleBuilder)
        {
            while (reader.Read())
            {
                int id = (int)reader["id"];
                
                VehicleDto vehicleDto = new VehicleDto()
                {
                    Color = (CarColor)Convert.ToInt32(reader["color"]),
                    Engine = new EngineDto
                    {
                        HorsePower = Convert.ToInt16(reader["engineHorsePower"]),
                        IsStarted = Convert.ToBoolean(reader["engineIsStarted"]),
                    },
                    Enrollment = new EnrollmentDto
                    {
                        Number = Convert.ToInt16(reader["number"]),
                        Serial = reader["serial"].ToString(),
                    },
                };
                vehicleDto.Doors = readDoors(reader, id).ToArray();
                vehicleDto.Wheels = readWheels(reader, id).ToArray();
                

                IVehicle vehicle = vehicleBuilder.import(vehicleDto);
                yield return vehicle;
            }            
        }

        private static IEnumerable<DoorDto> readDoors(SqlDataReader reader, int id)
        {

        }

        private static IEnumerable<WheelDto> readWheels(SqlDataReader reader, int id)
        {
            
        }

        //private static List<WheelDto> readWheels(SqlConnection connection, int vehicleId)
        //{
        //    List<WheelDto> wheelsDto = new List<WheelDto>();

        //    using (SqlCommand command = new SqlCommand())
        //    {
        //        command.Connection = connection;
        //        command.CommandText = $@"SELECT * FROM wheel
        //                                WHERE vehicleId = {vehicleId};";
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            WheelDto wheelDto = new WheelDto();
        //            wheelDto.Pressure = (double)reader["pressure"];
        //            wheelsDto.Add(wheelDto);
        //        }
        //    }

        //    return wheelsDto;
        //}

        //private static List<DoorDto> readDoors(SqlConnection connection, int vehicleId)
        //{
        //    List<DoorDto> doorsDto = new List<DoorDto>();

        //    using (SqlCommand command = new SqlCommand())
        //    {
        //        command.Connection = connection;
        //        command.CommandText = $@"SELECT * FROM door
        //                                WHERE vehicleId = {vehicleId};";
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            DoorDto doorDto = new DoorDto();
        //            doorDto.IsOpen = Convert.ToBoolean(reader["isOpen"]);
        //            doorsDto.Add(doorDto);
        //        }
        //    }
        //    return doorsDto;
        //}

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
