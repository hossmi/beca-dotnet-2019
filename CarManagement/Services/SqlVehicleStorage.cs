using System;
using System.Collections;
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
                        SELECT v.enrollmentId
                              ,v.color
                              ,v.engineHorsePower
                              ,v.engineIsStarted
	                          ,e.serial
	                          ,e.number
                          FROM [vehicle] v
                          INNER JOIN enrollment e ON v.enrollmentId = e.id ";

        private const string SELECT_FROM_ENROLLMENT = @"
                        SELECT e.serial
	                          ,e.number
                          FROM [vehicle] v
                          INNER JOIN enrollment e ON v.enrollmentId = e.id ";

        private const string SELECT_COUNT = "SELECT count(*) FROM vehicle";

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
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.CommandText = SELECT_COUNT;
                    command.Connection = connection;

                    int count = (int)command.ExecuteScalar();

                    connection.Close();
                    return count;
                }
            }
        }

        public void clear()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                string query = @"DELETE FROM wheel;
                                DELETE FROM door;
                                DELETE FROM vehicle;
                                DELETE FROM enrollment;";
                connection.Open();
                command.Connection = connection;
                command.CommandText = query;

                int afectedRows = command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Dispose()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.Dispose();

                connection.Close();
            }
        }

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void set(IVehicle vehicle)
        {
            string query = $@"
                     SELECT id
                     FROM enrollment
                     WHERE serial = '{vehicle.Enrollment.Serial}'
                     AND number = {vehicle.Enrollment.Number};";

            int enrollmentId = 0;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Connection = connection;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        enrollmentId = (int)reader["id"];
                    }
                }
                if (enrollmentId > 0)
                {
                    updateVehicle(vehicle, this.connectionString, query, enrollmentId);
                }
                else
                {
                    insertNewVehicle(vehicle, this.connectionString, query);
                }

                connection.Close();
            }
        }

        private static void updateVehicle(IVehicle vehicle, string connectionString, string queryEnrollment, int enrollmentId)
        {
            string sentences = "";

            using(SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = queryEnrollment;
                command.Connection = connection;

                command.CommandText = $@"
                                SELECT *
                                FROM vehicle
                                WHERE enrollmentId = {enrollmentId};";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CarColor colorAux = (CarColor)Convert.ToInt32(reader["color"]);
                        int horsePowerAux = (short)reader["engineHorsePower"];
                        bool startedAux = Convert.ToBoolean(reader["engineIsStarted"]);
                        if (colorAux != vehicle.Color)
                        {
                            sentences = $@"UPDATE vehicle
                                        SET color = @color
                                        FROM vehicle
                                        WHERE enrollmentId = @id;";
                            var parameters = new Dictionary<string, object>
                            {
                                {"@id", enrollmentId},
                                {"@color", vehicle.Color},
                            };
                            executeCommand(connectionString, sentences, parameters);
                        }
                        if (horsePowerAux != vehicle.Engine.HorsePower)
                        {
                            sentences = $@"UPDATE vehicle
                                        SET engineHorsePower = @horsePower
                                        FROM vehicle
                                        WHERE enrollmentId = @id;";
                            var parameters = new Dictionary<string, object>
                            {
                                {"@id", enrollmentId},
                                {"@horsePower", vehicle.Engine.HorsePower},
                            };
                            executeCommand(connectionString, sentences, parameters);
                        }
                        if (startedAux != vehicle.Engine.IsStarted)
                        {
                            sentences = $@"UPDATE vehicle
                                        SET engineIsStarted = @started
                                        WHERE enrollmentId = @id;";
                            var parameters = new Dictionary<string, object>
                            {
                                {"@id", enrollmentId},
                                {"@started", vehicle.Engine.IsStarted},
                            };
                            executeCommand(connectionString, sentences, parameters);
                        }
                    }
                }

                command.CommandText = $@"
                                SELECT *
                                FROM wheel, door
                                WHERE vehicleId = {enrollmentId};";

                sentences = $@"DELETE wheel
                                WHERE vehicleId = {enrollmentId}";
                executeCommand(connectionString, sentences);
                sentences = $@"DELETE door
                                WHERE vehicleId = {enrollmentId};";
                executeCommand(connectionString, sentences);

                insetWheelsAndDoors(connectionString, sentences, vehicle, enrollmentId);

                connection.Close();
            }
        }

        private static void insertNewVehicle(IVehicle vehicle, string connectionString, string query)
        {
            string sentences;
            int enrollmentId;

            sentences = $@"INSERT INTO enrollment (serial, number) 
                    VALUES (@serial, @number);";
            var parameters = new Dictionary<string, object>
                {
                    {"@serial", vehicle.Enrollment.Serial},
                    {"@number", vehicle.Enrollment.Number},
                };
            executeCommand(connectionString, sentences, parameters);

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Connection = connection;

                enrollmentId = (int)command.ExecuteScalar();

                connection.Close();
            }

            sentences = $@"INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted) 
                    VALUES (@enrollmentId, @color, @horsePower, @isStarted);";
            parameters = new Dictionary<string, object>
                {
                    {"@enrollmentId", enrollmentId},
                    {"@color", vehicle.Color},
                    {"@horsePower", vehicle.Engine.HorsePower},
                    {"@isStarted", vehicle.Engine.IsStarted},
                };
            executeCommand(connectionString, sentences, parameters);

            insetWheelsAndDoors(connectionString, sentences, vehicle, enrollmentId);
        }

        private static void insetWheelsAndDoors(string connectionString, string sentences, IVehicle vehicle, int enrollmentId)
        {
            foreach (IWheel wheel in vehicle.Wheels)
            {
                sentences = $@"INSERT INTO wheel (vehicleId, pressure) 
                                            VALUES (@enrollmentId, @pressure);";
                var parameters = new Dictionary<string, object>
                                {
                                    {"@enrollmentId", enrollmentId},
                                    {"@pressure", wheel.Pressure},
                                };
                executeCommand(connectionString, sentences, parameters);
            }

            foreach (IDoor door in vehicle.Doors)
            {
                sentences = $@"INSERT INTO door (vehicleId, isOpen) 
                                        VALUES (@enrollmentId, @isOpen);";
                var parameters = new Dictionary<string, object>
                            {
                                {"@enrollmentId", enrollmentId},
                                {"@isOpen", door.IsOpen},
                            };
                executeCommand(connectionString, sentences, parameters);
            }
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

        private static void executeCommand(string connectionString, string sentencies, IDictionary<string, object> parameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            using (IDbCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = sentencies;
                command.Connection = connection;

                foreach (var item in parameters)
                {
                    IDbDataParameter dbDataParameter = command.CreateParameter();
                    dbDataParameter.ParameterName = item.Key;
                    dbDataParameter.Value = item.Value;
                    command.Parameters.Add(dbDataParameter);
                }

                int afectedRows = command.ExecuteNonQuery();
                connection.Close();
            }
        }


        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IDictionary<string, string> filters;

            public IEnumerable<IEnrollment> Keys
            {
                get
                {
                    return enumerateEnrollments();
                }
            }

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.filters = new Dictionary<string, string>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereColorIs)));
                this.filters[nameof(whereColorIs)] = $" v.color = {(int)color} ";

                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEngineIsStarted)));
                this.filters[nameof(whereEngineIsStarted)] = $" v.engineIsStarted = {(started ? 0 : 1)} ";

                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentIs)));
                this.filters[nameof(whereEnrollmentIs)] = $" e.serial = '{enrollment.Serial}' AND e.number = {enrollment.Number} ";

                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentSerialIs)));
                this.filters[nameof(whereEnrollmentSerialIs)] = $" e.serial = '{serial}' ";

                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereHorsePowerEquals)));
                this.filters[nameof(whereHorsePowerEquals)] = $" v.engineHorsePower = {horsePower} ";

                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereHorsePowerIsBetween)));
                this.filters[nameof(whereHorsePowerIsBetween)] = $" v.engineHorsePower >= {min} AND v.engineHorsePower <= {max} ";

                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                string query = composeQuery(this.filters.Values, SELECT_FROM_VEHICLE);
                IEnumerable<IVehicle> vehicles = executeQuery(query, this.connectionString, this.vehicleBuilder);

                return vehicles.GetEnumerator();
            }

            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                string query = composeQuery(this.filters.Values, SELECT_FROM_ENROLLMENT);
                IEnumerable<IEnrollment> enrollments = executeQueryEnrollment(query, this.connectionString, this.vehicleBuilder);

                return enrollments;
            }

            private IEnumerable<IEnrollment> executeQueryEnrollment(string query, string connectionString, IVehicleBuilder vehicleBuilder)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Connection = connection;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IEnrollment enrollment = vehicleBuilder.import(reader["serial"].ToString(), (int)reader["number"]);
                            yield return enrollment;
                        }
                    }
                    connection.Close();
                }
            }

            private static IEnumerable<IVehicle> executeQuery(string query, string connectionString, IVehicleBuilder vehicleBuilder)
            {
                int enrollmentId = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand())
                {
                    connection.Open();
                    command.CommandText = query;
                    command.Connection = connection;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            enrollmentId = (int)reader["enrollmentId"];

                            VehicleDto vehicleDto = new VehicleDto
                            {
                                Color = (CarColor)Convert.ToInt32(reader["color"]),
                                Engine = new EngineDto
                                {
                                    HorsePower = (short)reader["engineHorsePower"],
                                    IsStarted = Convert.ToBoolean(reader["engineIsStarted"]),
                                },
                                Enrollment = new EnrollmentDto
                                {
                                    Number = (short)reader["number"],
                                    Serial = reader["serial"].ToString(),
                                },
                            };
                            List<WheelDto> wheelDtos = readWheels(connection, enrollmentId);
                            List<DoorDto> doorDtos = readDoors(connection, enrollmentId);

                            vehicleDto.Wheels = wheelDtos.ToArray();
                            vehicleDto.Doors = doorDtos.ToArray();

                            IVehicle vehicle = vehicleBuilder.import(vehicleDto);

                            yield return vehicle;
                        }
                    }
                    connection.Close();
                }
            }

            private static string composeQuery(IEnumerable<string> filters, string querySelect)
            {
                string query = "";

                foreach (string filter in filters)
                {
                    query += $" AND {filter}";
                }
                if (query != "")
                {
                    query = $" WHERE {query.Substring(4)} ";
                }
                query = querySelect + query;

                return query;
            }

            

        }
    }
}
