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
using ToolBox.Extensions.DbCommands;

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
                          FROM vehicle v
                          INNER JOIN enrollment e 
                          ON v.enrollmentId = e.id ";
        private const string DELETE_ALL_TABLES = "DELETE FROM door  DELETE FROM wheel  DELETE FROM vehicle  DELETE FROM enrollment";
        private const string SELECT_COUNT_VEHICLE = "SELECT count(*) FROM vehicle ";
        private const string COUNT_ENROLLMENT_ID = @" SELECT count(*) FROM enrollment WHERE serial = @serial AND number = @number ";
        private const string SELECT_ENROLLMENT_ID = @"SELECT id FROM enrollment WHERE serial = @serial AND number = @number ";
        private const string SELECT_ALL_ERNOLLMETNS = @"
                          SELECT e.serial
                                ,e.number
                          FROM vehicle v 
                          INNER JOIN enrollment e 
                          ON v.enrollmentId = e.id ";
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
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand deleteComand = new SqlCommand(DELETE_ALL_TABLES, connection);
                deleteComand.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {
            int enrollmentId = 0;
            var parameterEnrollmentId = new Dictionary<string, object>
                {
                    {
                        "@serial", vehicle.Enrollment.Serial
                    },
                    {
                        "@number", vehicle.Enrollment.Number
                    }
                };

            if (tryGetEnrollmentId(this.connectionString, vehicle.Enrollment,out enrollmentId))
            {

                string deleteTablesWithEnrollmentId = @"DELETE FROM door WHERE vehicleId = @id 
                                                        DELETE FROM wheel WHERE vehicleId = @id 
                                                        DELETE FROM vehicle WHERE enrollmentId = @id ";
                var parameterDeleteTablesWithEnrollmentId = new Dictionary<string, object>
                {
                    { "@id", enrollmentId }
                };
                executeCommand(this.connectionString, deleteTablesWithEnrollmentId, parameterDeleteTablesWithEnrollmentId);
            }
            else
            {
                string queryInsertEnrollment = @"INSERT INTO enrollment (serial, number) 
                                                 VALUES (@serial, @number) ";
                var parameterEnrollment = new Dictionary<string, object>
                {
                    { "@serial",vehicle.Enrollment.Serial },
                    { "@number",vehicle.Enrollment.Number }
                };
                executeCommand(this.connectionString, queryInsertEnrollment, parameterEnrollment);

                Asserts.isTrue(tryGetEnrollmentId(this.connectionString, vehicle.Enrollment, out enrollmentId));
            }


            string queryInsertVehicle = $@"
                    INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted) 
                    VALUES (@id, @color, @horsePower, @isStarted) ";
            var parameterVehicle = new Dictionary<string, object>
                {
                    {
                        "@id", enrollmentId
                    },
                    {
                        "@color", vehicle.Color
                    },
                    {
                        "@horsePower", vehicle.Engine.HorsePower
                    },
                    {
                        "@isStarted", vehicle.Engine.IsStarted
                    }
                };
            executeCommand(this.connectionString, queryInsertVehicle, parameterVehicle);


            foreach (IWheel wheel in vehicle.Wheels)
            {
                string queryInsertWheel = $@"
                    INSERT INTO wheel (vehicleId, pressure) 
                    VALUES (@id, @pressure);";
                var parameterWheel = new Dictionary<string, object>
                {
                    {
                        "@id",enrollmentId
                    },
                    {
                        "@pressure",wheel.Pressure
                    }
                };
                executeCommand(this.connectionString, queryInsertWheel, parameterWheel);
            }

            foreach (IDoor door in vehicle.Doors)
            {
                string queryInsertDoor = $@"
                    INSERT INTO door (vehicleId, isOpen) 
                    VALUES (@id, @isOpen);";
                var parameterDoor = new Dictionary<string, object>
                {
                    {
                        "@id",enrollmentId
                    },
                    {
                        "@isOpen",door.IsOpen
                    }
                };
                executeCommand(this.connectionString, queryInsertDoor, parameterDoor);
            }
        }        

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        private void executeCommand(string connectionString, string queryInsert, Dictionary<string, object> parameter)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            using (IDbCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = queryInsert;
                command.Connection = connection;
                command.setParameters(parameter);

                int afectedRows = command.ExecuteNonQuery();
                connection.Close();
            };
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

            public IEnumerable<IEnrollment> Keys
            {
                get
                {
                    return enumerateEnrollments();
                }
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

            private IEnumerable<IEnrollment> enumerateEnrollments() 
            {
                IEnumerable<IEnrollment> enrollments = executeQueryEnrollment(SELECT_ALL_ERNOLLMETNS, this.connectionString, this.vehicleBuilder);

                return enrollments;
            }

            private static IEnumerable<IEnrollment> executeQueryEnrollment(string query, string connectionString, IVehicleBuilder vehicleBuilder)
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
                                EnrollmentDto enrollmentDto = new EnrollmentDto
                                {
                                    Number = Convert.ToInt32(reader["number"]),
                                    Serial = Convert.ToString(reader["serial"])
                                };

                                yield return vehicleBuilder.import(enrollmentDto.Serial, enrollmentDto.Number);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
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

        private bool tryGetEnrollmentId(string connectionString, IEnrollment enrollment, out int enrollmentId)
        {
            bool existEnrollment;

            using (IDbConnection connection = new SqlConnection(connectionString))
            using (IDbCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = SELECT_ENROLLMENT_ID;
                command.Connection = connection;
                command.setParameter("@serial", enrollment.Serial);
                command.setParameter("@number", enrollment.Number);

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        enrollmentId = (int)reader["id"];
                        existEnrollment = true;
                    }
                    else
                    {
                        enrollmentId = 0;
                        existEnrollment = false;
                    }
                }
                connection.Close();
            }

            return existEnrollment;
        }
    }
}
