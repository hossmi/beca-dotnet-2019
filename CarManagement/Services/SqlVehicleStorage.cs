using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox;
using ToolBox.Extensions.DbCommands;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private IDataReader reader;
        private IDataParameter parameter;
        private List<string> idList;
        private object query;
        private IDoor[] doors;
        private IWheel[] wheels;
        const string DELETE_ALL = @"USE Carmanagement;
            DELETE FROM door WHERE vehicleId = @id
            DELETE FROM wheel WHERE vehicleId = @id
            DELETE FROM vehicle WHERE enrollmentId = @id
            DELETE FROM enrollment WHERE id = @id";
        const string SELECT_ENROLLMENT_FROM_VEHICLE = @"USE Carmanagement;
            SELECT enrollmentId FROM vehicle";
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
                        int count = (int)sentence.ExecuteScalar();
                        return count;
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
                    sentence.CommandText = SELECT_ENROLLMENT_FROM_VEHICLE;
                    this.reader = sentence.ExecuteReader();
                    while (this.reader.Read())
                    {
                        this.idList.Add(this.reader["enrollmentId"].ToString());
                    }
                    this.reader.Close();
                    foreach (string id in this.idList)
                    {
                        this.parameter = sentence.CreateParameter();
                        this.parameter.ParameterName = "@id";
                        this.parameter.Value = id;
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
        public void set(IVehicle vehicle)
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    this.parameter = sentence.CreateParameter();
                    this.parameter.ParameterName = "@serial";
                    this.parameter.Value = vehicle.Enrollment.Serial;
                    sentence.Parameters.Add(this.parameter);
                    this.parameter = sentence.CreateParameter();
                    this.parameter.ParameterName = "@number";
                    this.parameter.Value = vehicle.Enrollment.Number;
                    sentence.Parameters.Add(this.parameter);
                    sentence.CommandText = SELECT_FROM_ENROLLMENT;
                    if (sentence.ExecuteScalar() != null)
                    {
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            reader.Read();
                            this.parameter = sentence.CreateParameter();
                            this.parameter.ParameterName = "@id";
                            this.parameter.Value = (int)reader["id"];
                            sentence.Parameters.Add(this.parameter);
                            reader.Close();
                            sentence.CommandText = SELECT_FROM_VEHICLE;
                            this.query = sentence.ExecuteScalar();
                            using (IDataReader reader2 = sentence.ExecuteReader())
                            {
                                reader2.Read();
                                this.parameter = sentence.CreateParameter();
                                this.parameter.ParameterName = "@color";
                                this.parameter.Value = (int)vehicle.Color;
                                sentence.Parameters.Add(this.parameter);

                                this.parameter = sentence.CreateParameter();
                                this.parameter.ParameterName = "@engineIsStarted";
                                this.parameter.Value = vehicle.Engine.IsStarted ? 1 : 0;
                                sentence.Parameters.Add(this.parameter);

                                this.parameter = sentence.CreateParameter();
                                this.parameter.ParameterName = "@engineHorsePower";
                                this.parameter.Value = vehicle.Engine.HorsePower;
                                sentence.Parameters.Add(this.parameter);
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
                                        if (sentence.Parameters.Contains(this.parameter))
                                        {
                                            sentence.Parameters.Remove(this.parameter);
                                        }
                                        this.parameter = sentence.CreateParameter();
                                        this.parameter.ParameterName = "@pressure";
                                        this.parameter.Value = wheel.Pressure;
                                        sentence.Parameters.Add(this.parameter);
                                        sentence.CommandText = INSERT_WHEEL;
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    }
                                    sentence.CommandText = DELETE_DOOR;
                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    this.doors = vehicle.Doors;
                                    foreach (IDoor door in this.doors)
                                    {
                                        if (sentence.Parameters.Contains(this.parameter))
                                        {
                                            sentence.Parameters.Remove(this.parameter);
                                        }
                                        this.parameter = sentence.CreateParameter();
                                        this.parameter.ParameterName = "@isOpen";
                                        this.parameter.Value = door.IsOpen;
                                        sentence.Parameters.Add(this.parameter);
                                        sentence.CommandText = INSERT_DOOR;
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    }
                                }
                                else
                                {
                                    sentence.CommandText = INSERT_VEHICLE;

                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    this.wheels = vehicle.Wheels;
                                    foreach (IWheel wheel in this.wheels)
                                    {
                                        if (sentence.Parameters.Contains("@pressure"))
                                        {
                                            sentence.Parameters.Remove("@pressure");
                                        }
                                        this.parameter = sentence.CreateParameter();
                                        this.parameter.ParameterName = "@pressure";
                                        this.parameter.Value = wheel.Pressure;
                                        sentence.Parameters.Add(this.parameter);
                                        sentence.CommandText = INSERT_WHEEL;
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    }
                                    this.doors = vehicle.Doors;
                                    foreach (IDoor door in this.doors)
                                    {
                                        if (sentence.Parameters.Contains("@isOpen"))
                                        {
                                            sentence.Parameters.Remove("@isOpen");
                                        }
                                        this.parameter = sentence.CreateParameter();
                                        this.parameter.ParameterName = "@isOpen";
                                        this.parameter.Value = door.IsOpen;
                                        sentence.Parameters.Add(this.parameter);
                                        sentence.CommandText = INSERT_DOOR;
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
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
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            reader.Read();
                            this.parameter = sentence.CreateParameter();
                            this.parameter.ParameterName = "@id";
                            this.parameter.Value = (int)reader["id"];
                            sentence.Parameters.Add(this.parameter);
                            reader.Close();

                            this.parameter = sentence.CreateParameter();
                            this.parameter.ParameterName = "@color";
                            this.parameter.Value = (int)vehicle.Color;
                            sentence.Parameters.Add(this.parameter);

                            this.parameter = sentence.CreateParameter();
                            this.parameter.ParameterName = "@engineHorsePower";
                            this.parameter.Value = vehicle.Engine.HorsePower;
                            sentence.Parameters.Add(this.parameter);

                            this.parameter = sentence.CreateParameter();
                            this.parameter.ParameterName = "@engineIsStarted";
                            this.parameter.Value = Convert.ToInt16(vehicle.Engine.IsStarted);
                            sentence.Parameters.Add(this.parameter);

                            sentence.CommandText = INSERT_VEHICLE;
                            sentence.ExecuteNonQuery();
                            this.wheels = vehicle.Wheels;
                            foreach (IWheel wheel in this.wheels)
                            {
                                if (sentence.Parameters.Contains(this.parameter))
                                {
                                    sentence.Parameters.Remove(this.parameter);
                                }
                                this.parameter = sentence.CreateParameter();
                                this.parameter.ParameterName = "@pressure";
                                this.parameter.Value = wheel.Pressure;
                                sentence.Parameters.Add(this.parameter);
                                sentence.CommandText = INSERT_WHEEL;
                                Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            }
                            this.doors = vehicle.Doors;
                            foreach (IDoor door in this.doors)
                            {
                                if (sentence.Parameters.Contains(this.parameter))
                                {
                                    sentence.Parameters.Remove(this.parameter);
                                }
                                this.parameter = sentence.CreateParameter();
                                this.parameter.ParameterName = "@isOpen";
                                this.parameter.Value = door.IsOpen;
                                sentence.Parameters.Add(this.parameter);
                                sentence.CommandText = INSERT_DOOR;
                                Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            }
                        }
                    }
                }
            }
        }
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
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
            //private IDictionary<string, string> Type;
            private IEnrollmentProvider enrollmentProvider;
            private IVehicle vehicle_get;
            private IDbDataParameter parameter;
            private EnrollmentDto enrollmentDto;
            private VehicleDto vehicleDto;
            private EngineDto engineDto;
            private List<WheelDto> wheelsDto;
            private List<DoorDto> doorsDto;
            private WheelDto wheelDto;
            private DoorDto doorDto;
            private string query;
            //private object data;
            //private string dataType;
            private const string SELECT_VEHICLE_HEAD = @"
                SELECT e.serial, 
                    e.number, 
                    e.id, 
                    v.color, 
                    v.engineIsStarted, 
                    v.engineHorsePower 
                FROM enrollment e 
                INNER JOIN vehicle v ON e.id = v.enrollmentId ";
            private const string SELECT_WHEELS = @"
                    SELECT pressure 
                    FROM wheel 
                    WHERE vehicleId = @ID";
            private const string SELECT_DOORS = @"
                    SELECT isOpen 
                    FROM door 
                    WHERE vehicleId = @ID";
            private const string SELECT_ENROLLMENT = @"
                    SELECT *
                    FROM enrollment";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.queryParts = new Dictionary<string, string>();
                this.queryParameters = new Dictionary<string, object>();
                //this.Type = new Dictionary<string, string>();
                this.vehicleDto = new VehicleDto();
                this.enrollmentDto = new EnrollmentDto();
                this.engineDto = new EngineDto();
                this.wheelsDto = new List<WheelDto>();
                this.doorsDto = new List<DoorDto>();
                this.enrollmentProvider = new DefaultEnrollmentProvider();
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
                this.queryParameters.Add("@color", (int)color);
                this.queryParts.Add("color"," color = " + (int)color);
                this.color = color;
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.queryParameters.Add("@engineIsStarted", started);
                this.queryParts.Add("@engineIsStarted", " engineIsStarted = @engineIsStarted");
                //this.Type.Add("@engineIsStarted", "int");
                this.engineIsStarted = started;
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.queryParameters.Add("@number", enrollment.Number);
                this.queryParameters.Add("@serial", enrollment.Serial);
                this.queryParts.Add("@number", "number = @number");
                this.queryParts.Add("@serial", "serial = @serial");
                //this.Type.Add("@serial", "string");
                //this.Type.Add("@number", "int");
                this.enrollment = enrollment;
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;
                this.queryParameters.Add("@serial", serial);
                this.queryParts.Add("@serial", "serial = @serial");
                //this.Type.Add("@serial", "string");
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.queryParameters.Add("@engineHorsePower", horsePower);
                this.queryParts.Add("@engineHorsePower", "engineHorsePower = @engineHorsePower");
                this.horsePower = horsePower;
                //this.Type.Add("@engineHorsePower", "int");
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.queryParameters.Add("@min", min);
                this.queryParameters.Add("@max", max);
                this.queryParts.Add("@engineHorsePower", "engineHorsePower BETWEEN @min AND @max");
                //this.Type.Add("@min", "int");
                //this.Type.Add("@max", "int");
                this.max = max;
                this.min = min;
                return this;
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
                this.query = createQuery(SELECT_VEHICLE_HEAD, this.queryParts);
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = this.query;
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);
                        /*foreach (var item in this.queryParameters.Keys)
                        {
                            this.Type
                                .TryGetValue(item, out this.dataType);
                            if (this.dataType == "int")
                            {
                                this.queryParameters
                                    .TryGetValue(item, out this.data);
                                this.data = Convert.ToInt32(this.data);
                            }
                            else
                            {
                                this.queryParameters
                                    .TryGetValue(item, out this.data);
                                this.data = Convert.ToString(this.data);
                            }
                            this.parameter = sentence.CreateParameter();
                            this.parameter.ParameterName = $"{item}";
                            this.parameter.Value = this.data;
                            sentence.Parameters.Add(this.parameter);
                        }*/

                    using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.parameter = sentence.CreateParameter();
                                this.parameter.ParameterName = "@ID";
                                this.parameter.Value = (int)reader["id"];
                                sentence.Parameters.Add(this.parameter);
                                this.id = (int)reader["id"];
                                this.enrollmentDto.Serial = reader["serial"].ToString();
                                this.enrollmentDto.Number = Convert.ToInt32(reader["number"]);
                                this.color = (CarColor)Enum.Parse(typeof(CarColor), reader["color"].ToString());
                                this.engineDto.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);
                                this.engineDto.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    sentence2.CommandText = SELECT_WHEELS;
                                    this.parameter = sentence2.CreateParameter();
                                    this.parameter.ParameterName = "@ID";
                                    this.parameter.Value = this.id;
                                    sentence2.Parameters.Add(this.parameter);

                                    using (IDataReader reader2 = sentence2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            this.wheelDto = new WheelDto();
                                            this.wheelDto.Pressure = Convert.ToDouble(reader2["pressure"]);
                                            this.wheelsDto.Add(this.wheelDto);
                                        }
                                    }
                                }
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    sentence2.CommandText = SELECT_DOORS;
                                    this.parameter = sentence2.CreateParameter();
                                    this.parameter.ParameterName = "@ID";
                                    this.parameter.Value = (int)reader["id"];
                                    sentence2.Parameters.Add(this.parameter);

                                    using (IDataReader reader2 = sentence2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            this.doorDto = new DoorDto();
                                            this.doorDto.IsOpen = Convert.ToBoolean(reader2["isOpen"]);
                                            this.doorsDto.Add(this.doorDto);
                                        }
                                    }
                                }
                                this.vehicleDto.Color = this.color;
                                this.vehicleDto.Doors = this.doorsDto.ToArray();
                                this.vehicleDto.Wheels = this.wheelsDto.ToArray();
                                this.vehicleDto.Engine = this.engineDto;
                                this.vehicleDto.Enrollment = this.enrollmentDto;
                                this.vehicle_get = this.vehicleBuilder.import(this.vehicleDto);

                                yield return this.vehicle_get;
                            }
                        }
                    }
                }
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
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = SELECT_ENROLLMENT;
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.enrollmentDto.Number = Convert.ToInt16(reader["number"]);
                                this.enrollmentDto.Serial = reader["serial"].ToString();
                                yield return this.enrollmentProvider.import(this.enrollmentDto.Serial, this.enrollmentDto.Number);
                            }
                        }
                    }
                }
                
            }

        }
    }
}