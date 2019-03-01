using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox.Extensions.DbCommands;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private IDataParameter parameter;
        private List<string> idList;
        private object query;
        private IDoor[] doors;
        private IWheel[] wheels;
        private int id;
        const string DELETE_ALL = @"USE Carmanagement;
            DELETE FROM door WHERE vehicleId = @id
            DELETE FROM wheel WHERE vehicleId = @id
            DELETE FROM vehicle WHERE enrollmentId = @id
            DELETE FROM enrollment WHERE id = @id";
        const string SELECT_FROM_ENROLLMENT = @"USE CarManagement; 
            SELECT id FROM enrollment 
            WHERE serial = @serial AND number = @number";
        const string INSERT_ENROLLMENT = @"INSERT INTO enrollment(serial, number)
            VALUES (@serial, @number)";
        const string SELECT_FROM_VEHICLE = @"SELECT * FROM vehicle
            WHERE enrollmentId = @id";
        const string INSERT_VEHICLE = @"INSERT INTO vehicle(enrollmentId, 
            color, engineHorsePower, engineIsStarted)
            VALUES (@id, @color, @engineHorsePower, @engineIsStarted)";
        const string UPDATE_VEHICLE = @"UPDATE vehicle
            SET color = @color, 
            engineHorsePower = @engineHorsePower, 
            engineIsStarted = @engineIsStarted
            WHERE enrollmentId = @id";
        const string DELETE_WHEEL = @"DELETE FROM wheel
            WHERE vehicleId = @id";
        const string INSERT_WHEEL = @"INSERT INTO wheel(vehicleId, pressure)
            VALUES (@id, @pressure)";
        const string DELETE_DOOR = @"DELETE FROM door
            WHERE vehicleId = @id";
        const string INSERT_DOOR = @"INSERT INTO door(vehicleId, isOpen)
            VALUES (@id, @isOpen)";
        const string COUNT_VEHICLE = @"USE CarManagement
            SELECT count(enrollmentId) AS 'Count' FROM vehicle";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            this.idList = new List<string>();
        }
        public int Count {
            get
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = COUNT_VEHICLE;
                        return (int)sentence.ExecuteScalar();
                    }

                }

            }
        }
        public void clear()
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                using (IDbCommand sentence = con.CreateCommand())
                {
                    con.Open();
                    sentence.CommandText = selectMethod("enrollment");
                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.idList.Add(reader["id"].ToString());
                        }
                        reader.Close();
                    }
                    foreach (string id in this.idList)
                    {
                        this.parameter = setParameter(sentence, "@id", id);
                        sentence.Parameters.Add(this.parameter);

                        sentence.CommandText = DELETE_ALL;
                        sentence.ExecuteNonQuery();
                        sentence.Parameters.Remove(this.parameter);

                    }
                }

            }

        }
        public void Dispose()
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Close();
            }    
        }
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }
        public void remove(IEnrollment enrollment)
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                using (IDbCommand sentence = con.CreateCommand())
                {
                    con.Open();
                    sentence.Parameters.Add(setParameter(sentence, "@serial", enrollment.Serial));
                    sentence.Parameters.Add(setParameter(sentence, "@number", enrollment.Number));
                    sentence.CommandText = selectMethod("enrollment") + "WHERE serial = @serial AND number = @number";

                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        reader.Read();
                        this.id = (int)reader["enrollmentId"];
                        reader.Close();
                    }
                    sentence.CommandText = SELECT_FROM_VEHICLE;
                    sentence.Parameters.Add(setParameter(sentence, "@id", this.id));
                    if (sentence.ExecuteScalar() != null)
                    {
                        sentence.CommandText = DELETE_ALL;
                        sentence.ExecuteNonQuery();
                    }
                }
            }
        }
        public void set(IVehicle vehicle)
        {
            void buildObject(string name, object param, string query, IDbCommand sentence)
            {
                this.parameter = setParameter(sentence, name, param);
                sentence.Parameters.Add(this.parameter);
                sentence.CommandText = query;
                Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                sentence.Parameters.Remove(this.parameter);
            }
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    sentence.Parameters.Add(setParameter(sentence, "@serial", vehicle.Enrollment.Serial));
                    sentence.Parameters.Add(setParameter(sentence, "@number", vehicle.Enrollment.Number));
                    sentence.CommandText = SELECT_FROM_ENROLLMENT;
                    if (sentence.ExecuteScalar() != null)
                    {
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            sentence.Parameters.Clear();
                            reader.Read();
                            sentence.Parameters.Add(setParameter(sentence, "@id", (int)reader["id"]));
                            reader.Close();
                            sentence.CommandText = SELECT_FROM_VEHICLE;
                            this.query = sentence.ExecuteScalar();
                            using (IDataReader reader2 = sentence.ExecuteReader())
                            {
                                reader2.Read();
                                sentence.Parameters.Add(setParameter(sentence, "@color", (int)vehicle.Color));
                                sentence.Parameters.Add(setParameter(sentence, "@engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0));
                                sentence.Parameters.Add(setParameter(sentence, "@engineHorsePower", vehicle.Engine.HorsePower));
                                reader2.Close();

                                if (this.query != null)
                                {
                                    sentence.CommandText = UPDATE_VEHICLE;
                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    sentence.CommandText = DELETE_WHEEL;
                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    this.wheels = vehicle.Wheels;
                                    foreach (IWheel wheel in this.wheels)
                                    {
                                        buildObject("@pressure", wheel.Pressure, INSERT_WHEEL, sentence);
                                    }
                                    sentence.CommandText = DELETE_DOOR;
                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    this.doors = vehicle.Doors;
                                    foreach (IDoor door in this.doors)
                                    {
                                        buildObject("@isOpen", door.IsOpen, INSERT_DOOR, sentence);
                                    }
                                }
                                else
                                {
                                    sentence.CommandText = INSERT_VEHICLE;

                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    this.wheels = vehicle.Wheels;
                                    foreach (IWheel wheel in this.wheels)
                                    {
                                        buildObject("@pressure", wheel.Pressure, INSERT_WHEEL, sentence);
                                    }
                                    this.doors = vehicle.Doors;
                                    foreach (IDoor door in this.doors)
                                    {
                                        buildObject("@isOpen", door.IsOpen, INSERT_DOOR, sentence);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        sentence.CommandText = INSERT_ENROLLMENT;
                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                        sentence.CommandText = SELECT_FROM_ENROLLMENT;
                        Asserts.isTrue(sentence.ExecuteScalar() != null);
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            reader.Read();
                            sentence.Parameters.Add(setParameter(sentence, "@id", (int)reader["id"]));
                            reader.Close();

                            sentence.Parameters.Add(setParameter(sentence, "@color", (int)vehicle.Color));
                            sentence.Parameters.Add(setParameter(sentence, "@engineHorsePower", vehicle.Engine.HorsePower));
                            sentence.Parameters.Add(setParameter(sentence, "@engineIsStarted", Convert.ToInt16(vehicle.Engine.IsStarted)));

                            sentence.CommandText = INSERT_VEHICLE;
                            sentence.ExecuteNonQuery();
                            this.wheels = vehicle.Wheels;
                            foreach (IWheel wheel in this.wheels)
                            {
                                buildObject("@pressure", wheel.Pressure, INSERT_WHEEL, sentence);
                            }
                            this.doors = vehicle.Doors;
                            foreach (IDoor door in this.doors)
                            {
                                buildObject("@isOpen", door.IsOpen, INSERT_DOOR, sentence);
                            }
                        }
                    }
                }
            }
        }
        private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = thing;
            return parameter;
        }
        private static string selectMethod(string table)
        {
            string select = $"SELECT * FROM {table}";
            return select;
        }
        private static string whereMethod(string field, string condition)
        {
            string where = $" WHERE {field} = {condition}";
            return where;
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private VehicleBuilder vehicleBuilder2;
            private CarColor color;
            private bool engineIsStarted;
            private IEnrollment enrollment;
            private string enrollmentSerial;
            private int horsePower;
            private int min;
            private int max;
            private int id;
            private IDictionary<string, object> queryParameters;
            private IDictionary<string, string> queryParts;
            private IEnrollmentProvider enrollmentProvider;
            private EnrollmentDto enrollmentDto;
            private VehicleDto vehicleDto;
            private EngineDto engineDto;
            private List<WheelDto> wheelsDto;
            private List<DoorDto> doorsDto;
            private WheelDto wheelDto;
            private DoorDto doorDto;
            private IDictionary<string, object> dictionaryId;
            private IDictionary<string, string> dictionaryId2;
            private const string SELECT_VEHICLE_HEAD = @"
                SELECT e.serial, 
                    e.number, 
                    e.id, 
                    v.color, 
                    v.engineIsStarted, 
                    v.engineHorsePower 
                FROM enrollment e 
                INNER JOIN vehicle v ON e.id = v.enrollmentId ";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.vehicleBuilder2 = new VehicleBuilder(this.enrollmentProvider);
                this.queryParts = new Dictionary<string, string>();
                this.queryParameters = new Dictionary<string, object>();
                this.vehicleDto = new VehicleDto();
                this.enrollmentDto = new EnrollmentDto();
                this.engineDto = new EngineDto();
                this.wheelsDto = new List<WheelDto>();
                this.doorsDto = new List<DoorDto>();
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.dictionaryId = new Dictionary<string, object>();
                this.dictionaryId2 = new Dictionary<string, string>();

            }

            public IEnumerable<IEnrollment> Keys
            {
                get
                {
                    return enumerateEnrollments();
                }
            }
            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isEnumDefined(color);
                addParametersDictionary("@color", "color", (int)color, this.queryParameters, this.queryParts);
                this.color = color;
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                addParametersDictionary("@engineIsStarted", "engineIsStarted", started, this.queryParameters, this.queryParts);
                this.engineIsStarted = started;
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                addParametersDictionary("@number", "number", enrollment.Number, this.queryParameters, this.queryParts);
                addParametersDictionary("@serial", "serial", enrollment.Serial, this.queryParameters, this.queryParts);
                this.enrollment = enrollment;
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;
                addParametersDictionary("@serial", "serial", serial, this.queryParameters, this.queryParts);
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                addParametersDictionary("@engineHorsePower", "engineHorsePower", horsePower, this.queryParameters, this.queryParts);
                this.horsePower = horsePower;
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.queryParameters.Add("@min", min);
                this.queryParameters.Add("@max", max);
                this.queryParts.Add("@engineHorsePower", "engineHorsePower BETWEEN @min AND @max");
                this.max = max;
                this.min = min;
                return this;
            }
            private static void addParametersDictionary(string key, string sqlCollumn, object parameter, IDictionary<string, object> queryParameters, IDictionary<string, string> queryParts)
            {
                string command = command = sqlCollumn + " = " + key;
                queryParameters.Add(key, parameter);
                queryParts.Add(key, command);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }
            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }
            private IEnumerator<IVehicle> enumerate()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = createQuery(SELECT_VEHICLE_HEAD, this.queryParts);
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);

                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.id = (int)reader["id"];
                                this.enrollmentDto = this.vehicleBuilder2.enrollmentDtoBuilder(reader["serial"].ToString(), Convert.ToInt32(reader["number"]));
                                this.color = (CarColor)Enum.Parse(typeof(CarColor), reader["color"].ToString());
                                this.engineDto = this.vehicleBuilder2.engineDtoBuilder(Convert.ToBoolean(reader["engineIsStarted"]), Convert.ToInt32(reader["engineHorsePower"]));

                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    sentence2.CommandText = selectDeleteMethod("SELECT pressure ", "wheel") + whereMethod("vehicleId", "@id");
                                    sentence2.Parameters.Add(setParameter(sentence2, "@id", this.id));

                                    using (IDataReader reader2 = sentence2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            this.wheelDto = this.vehicleBuilder2.wheelDtoBuilder(Convert.ToDouble(reader2["pressure"]));
                                            this.wheelsDto.Add(this.wheelDto);
                                        }
                                    }
                                }
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    sentence2.CommandText = selectDeleteMethod("SELECT isOpen ", "door") + whereMethod("vehicleId", "@id");
                                    sentence2.Parameters.Add(setParameter(sentence2, "@id", (int)reader["id"]));

                                    using (IDataReader reader2 = sentence2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            this.doorDto = this.vehicleBuilder2.doorDtoBuilder(Convert.ToBoolean(reader2["isOpen"]));
                                            this.doorsDto.Add(this.doorDto);
                                        }
                                    }
                                }
                                yield return this.vehicleBuilder.import(this.vehicleBuilder2.vehicleDtoBuilder(this.enrollmentDto, this.engineDto, this.color, this.wheelsDto.ToArray(), this.doorsDto.ToArray()));
                            }
                        }
                    }
                }
            }
            private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
            {
                IDataParameter parameter = sentence.CreateParameter();
                parameter.ParameterName = name;
                parameter.Value = thing;
                return parameter;
            }
            private static string createQuery(string query, IDictionary<string, string> conditionParts)
            {
                int counter = 0;
                foreach (string conditionPart in conditionParts.Values)
                {
                    if (counter == 0)
                    {
                        query += " WHERE " + conditionPart;
                    }
                    else
                    {
                        query += " AND " + conditionPart;
                    }
                    counter++;
                }
                return query;
            }
            private static string whereMethod(string field, string condition)
            {
                string where = $" WHERE {field} = {condition}";
                return where;
            }
            private static string elementString(string query, List<string> elements)
            {
                int counter = 0;
                foreach (string element in elements)
                {
                    if (counter < elements.Count)
                    {
                        query += element + ", ";
                    }
                    else
                    {
                        query += element + ")";
                    }
                    counter++;
                }
                return query;
            }
            private static string selectDeleteMethod(string command, string table)
            {
                string select = $"{command} FROM {table}";
                return select;
            }
            private static string insertupdateMethod(string table, IDictionary<string, string> dataDictionary, string type, string instruction)
            {
                List<string> columns = new List<string>();
                List<string> values = new List<string>();
                foreach (string value in dataDictionary.Values)
                {
                    values.Add(value);
                }
                foreach (string column in dataDictionary.Keys)
                {
                    columns.Add(column);
                }
                string query = "";
                query = $"{type}{table} (";
                query = query + $"{elementString(query, columns)}{instruction}(";
                query = query + elementString(query, values);

                return query;
            }
            private static string simpleQueryMethod(string table, string field, IDictionary<string, string> conditionParts, string command, IDictionary<string, string> dataDictionary)
            {
                string query ="";
                if(command.Contains("SELECT") || command.Contains("DELETE"))
                {
                    query = selectDeleteMethod(command, table);
                    query = query + createQuery(query, conditionParts);
                }
                else if(command.Contains("INSERT"))
                {
                    query = insertupdateMethod(table, dataDictionary, command, " SET ");
                }
                else if(command.Contains("UPDATE"))
                {
                    query = insertupdateMethod(table, dataDictionary, command, " VALUES ");
                }
                return query;
            }

            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = selectDeleteMethod("SELECT * ", "enrollment");
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                yield return this.enrollmentProvider.import(reader["serial"].ToString(), Convert.ToInt16(reader["number"]));
                            }
                        }
                    }
                }
                
            }

        }
    }
}