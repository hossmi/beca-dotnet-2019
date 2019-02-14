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
        #region "SQL"
        private const string SELECT_FROM_VEHICLE = @"
                        SELECT v.enrollmentId
                              ,v.color
                              ,v.engineHorsePower
                              ,v.engineIsStarted
	                          ,e.serial
	                          ,e.number
                          FROM vehicle v
                          INNER JOIN enrollment e ON v.enrollmentId = e.id ";
        private const string SELECT_FROM_DOOR = "";
        private const string SELECT_COUNT_VEHICLE = "SELECT count(*) FROM vehicle ";
        #endregion 
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
                return executeScalarQuery(this.connectionString, SELECT_COUNT_VEHICLE);
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
                                        WHERE vehicleId = {vehicleId}";
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
                command.CommandText = $@"SELECT  FROM door
                                        WHERE vehicleId = {vehicleId}";
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
            string querySelect, queryInsert;

            queryInsert = $@"
                    INSERT 
                    INTO enrollment (serial, number) 
                    VALUES ('{vehicle.Enrollment.Serial}', {vehicle.Enrollment.Number});";

            executeCommand(this.connectionString, queryInsert);

            querySelect = $@"
                     SELECT id
                     FROM enrollment
                     WHERE serial = '{vehicle.Enrollment.Serial}' 
                     AND number = {vehicle.Enrollment.Number};";
            int enrollmentId = executeScalarQuery(this.connectionString, querySelect);

            queryInsert = $@"
                    INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted) 
                    VALUES ({enrollmentId}, {Convert.ToInt32(vehicle.Color)}, {Convert.ToInt16(vehicle.Engine.HorsePower)}, {(vehicle.Engine.IsStarted ? 0 : 1)}) ";
            /*var parameter = new Dictionary<string, object>
                {
                    {
                        "@id",enrollmentId
                    },
                    {
                        "@pressure",wheel.Pressure
                    }
                };*/

            executeCommand(this.connectionString, queryInsert);

            foreach (IWheel wheel in vehicle.Wheels)
            {
                queryInsert = $@"
                    INSERT INTO wheel (vehicleId, pressure) 
                    VALUES (@id, @pressure);";
                var parameter = new Dictionary<string, object>
                {
                    {
                        "@id",enrollmentId
                    },
                    {
                        "@pressure",wheel.Pressure
                    }
                };

                executeCommand(this.connectionString, queryInsert, parameter);
            }

            foreach (IDoor door in vehicle.Doors)
            {
                queryInsert = $@"
                    INSERT INTO door (vehicleId, isOpen) 
                    VALUES (@id, @isOpen);";
                var parameter = new Dictionary<string, object>
                {
                    {
                        "@id",enrollmentId
                    },
                    {
                        "@isOpen",door.IsOpen
                    }
                };
                executeCommand(this.connectionString, queryInsert, parameter);
            }
        }

        private void executeCommand(string connectionString, string queryInsert, Dictionary<string, object> parameter)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            using (IDbCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = queryInsert;
                command.Connection = connection;

                foreach (var item in parameter)
                {
                    IDbDataParameter dbDataParameter = command.CreateParameter();
                    dbDataParameter.ParameterName = item.Key;
                    dbDataParameter.Value = item.Value;
                    command.Parameters.Add(dbDataParameter);
                }
                
                int afectedRows = command.ExecuteNonQuery();
                connection.Close();
            };
        }

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
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

        private static int executeScalarQuery(string connectionString, string query)
        {
            int result = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            result = (int)command.ExecuteScalar();
            connection.Close();

            return result;
        }

        

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IDictionary<string, string> filters;
            private readonly string indexWhereHorsePower = "whereHorsePower";


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
                this.filters[nameof(whereColorIs)] = " color = " + (int)(color) + " ";

                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEngineIsStarted)));
                this.filters[nameof(whereEngineIsStarted)] = " engineIsStarted = " + (started ? 0 : 1) + " ";

                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentIs)));
                this.filters[nameof(whereEnrollmentIs)] = " serial = '" + enrollment.Serial +
                                                          "' AND number = " + enrollment.Number + " ";

                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentSerialIs)));
                this.filters[nameof(whereEnrollmentSerialIs)] = " serial = '" + serial + "' ";

                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isFalse(this.filters.ContainsKey(this.indexWhereHorsePower));
                this.filters[this.indexWhereHorsePower] = " engineHorsePower = " + horsePower.ToString() + " ";

                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isFalse(this.filters.ContainsKey(this.indexWhereHorsePower));
                this.filters[this.indexWhereHorsePower] = " engineHorsePower >= " + min.ToString()
                                                    + " AND engineHorsePower <= " + max.ToString() + " ";

                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                string query = composeQuery(this.filters.Values);
                IEnumerable<IVehicle> vehicles = executeQuery(query, this.connectionString, this.vehicleBuilder);

                return vehicles.GetEnumerator();
            }

            private static IEnumerable<IVehicle> executeQuery(string query, string connectionString, IVehicleBuilder vehicleBuilder)
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int enrollmentId = (int)reader["enrollmentId"];

                                VehicleDto vehicleDto = new VehicleDto
                                {
                                    Color = (CarColor)Convert.ToInt32(reader["color"]),
                                    Engine = new EngineDto
                                    {
                                        HorsePower = Convert.ToInt32(reader["engineHorsePower"]),
                                        IsStarted = Convert.ToBoolean(reader["engineIsStarted"])
                                    },
                                    Enrollment = new EnrollmentDto
                                    {
                                        Number = Convert.ToInt32(reader["number"]),
                                        Serial = Convert.ToString(reader["serial"])
                                    }
                                };
                                vehicleDto.Doors = getDoor(sqlConnection, enrollmentId).ToArray();
                                vehicleDto.Wheels = getWheel(sqlConnection, enrollmentId).ToArray();

                                yield return vehicleBuilder.import(vehicleDto); ;
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }

            private static IEnumerable<DoorDto> getDoor(SqlConnection sqlConnection, int enrollmentId)
            {
                string query = "SELECT isOpen FROM door WHERE vehicleId = " + enrollmentId + " ";//MIRAR
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new DoorDto
                            {
                                IsOpen = Convert.ToBoolean(reader["isOpen"])
                            };
                        }
                    }
                }
            }

            private static IEnumerable<WheelDto> getWheel(SqlConnection sqlConnection, int enrollmentId)
            {
                string query = "SELECT pressure FROM wheel WHERE vehicleId = " + enrollmentId + " ";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new WheelDto
                            {
                                Pressure = Convert.ToDouble(reader["pressure"])
                            };
                        }
                    }
                }
            }

            private static string composeQuery(IEnumerable<string> filters)
            {
                string query = "";
                foreach (string filter in filters)
                {
                    query += " AND " + filter;
                }

                if (query != "")
                {
                    query = " WHERE " + query.Substring(4);
                }

                query = SELECT_FROM_VEHICLE + query;

                return query;
            }
        }
    }
}
