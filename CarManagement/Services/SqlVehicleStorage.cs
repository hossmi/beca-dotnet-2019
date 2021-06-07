using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox.Extensions.DbCommands;
using static ToolBox.Services.QueryBuilder;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private const string USE = "USE CarManagement;";
        private const string DECLARE = "DECLARE @ID INT = SELECT id FROM enrollment WHERE serial = @serial AND number = @number;";
        private readonly string COUNT_VEHICLE = "SELECT COUNT(enrollmentId) FROM vehicle";
        private const string SELECT_ID_ENROLLMENT = "SELECT id FROM enrollment WHERE serial = @serial AND number = @number;";
        private const string SELECT_ENROLLMENT = "SELECT * FROM enrollment;";
        private const string SELECT_DOOR = "SELECT isOpen FROM door WHERE vehicleId = @id;";
        private const string SELECT_WHEEL = "SELECT pressure FROM wheel WHERE vehicleId = @id;";
        private const string INSERT_ENROLLMENT = "INSERT INTO enrollment (serial, number) OUTPUT INSERTED.ID VALUES (@serial, @number);";
        private const string DELETE_VEHICLE_COMPLEX = "DELETE FROM vehicle WHERE enrollmentId = @ID;";
        private const string DELETE_DOOR_COMPLEX = "DELETE FROM door WHERE vehicleId = @ID;";
        private const string DELETE_WHEEL_COMPLEX = "DELETE FROM wheel WHERE vehicleId = @ID;";
        private const string DELETE_WHEEL = "DELETE FROM wheel;";
        private const string DELETE_DOOR = "DELETE FROM door;";
        private const string DELETE_VEHICLE = "DELETE FROM vehicle;";
        private const string SELECT_ALL_ENROLLMENT = "SELECT * FROM enrollment WHERE id = @id;";
        private const string UPDATE_VEHICLE = "UPDATE vehicle SET color = @color, engineIsStarted = @engineIsStarted, engineHorsePower = @engineHorsePower WHERE enrollmentId = @id;";
        private const string SELECT_ENROLLMENT_VEHICLE = "SELECT e.serial, e.number, e.id, v.color, v.engineIsStarted, v.engineHorsePower FROM enrollment e INNER JOIN vehicle v ON id = enrollmentId";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count
        {
            get
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    int counter;
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = $"{USE} {COUNT_VEHICLE}";
                        counter = (int)sentence.ExecuteScalar();
                    }
                    con.Close();
                    return counter;
                }

            }
        }

        public void clear()
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    sentence.CommandText = $"{DELETE_WHEEL} {DELETE_DOOR} {DELETE_VEHICLE}";
                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                }
                con.Close();
            }
        }
        public void Dispose()
        {
            SqlConnection.ClearAllPools();
        }
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }
        public void remove(IEnrollment enrollment)
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    DBCommandExtensions.setParameters
                    (
                        sentence, 
                        new Dictionary<FieldNames, object>() 
                        {
                            { FieldNames.serial , enrollment.Serial },  
                            { FieldNames.number, enrollment.Number } 
                        }
                    );
                    
                    sentence.CommandText = $"{DECLARE} {DELETE_WHEEL_COMPLEX} {DELETE_DOOR_COMPLEX} {DELETE_VEHICLE_COMPLEX}";
                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                }
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
                    int ID;
                    DBCommandExtensions.setParameters
                    (
                        sentence, 
                        new Dictionary<FieldNames, object>()
                        { 
                            { FieldNames.serial, vehicle.Enrollment.Serial }, 
                            { FieldNames.number, vehicle.Enrollment.Number } 
                        }
                    );
                    sentence.CommandText = SELECT_ID_ENROLLMENT;
                    if (sentence.ExecuteScalar() != null)
                    {
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            reader.Read();
                            ID = (int)reader.GetValue(0);
                            DBCommandExtensions.setParameter(sentence, $"{reader.GetName(0)}", reader.GetValue(0));
                            reader.Close();
                        }

                        DBCommandExtensions.setParameters
                        (
                            sentence, 
                            new Dictionary<FieldNames, object>()
                            {
                                {FieldNames.color, (int)vehicle.Color },
                                {FieldNames.engineIsStarted, vehicle.Engine.IsStarted ? 1 : 0 },
                                {FieldNames.engineHorsePower, vehicle.Engine.HorsePower }
                            }
                        );
                        sentence.CommandText = SELECT_ALL_ENROLLMENT;
                        if (sentence.ExecuteScalar() != null)
                        {
                            sentence.CommandText = UPDATE_VEHICLE;

                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            sentence.CommandText = $"{DELETE_WHEEL_COMPLEX} {DELETE_DOOR_COMPLEX}";
                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);

                            sentence.CommandText = insertwheelsdoors(vehicle, sentence, ID);
                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                        }
                        else
                        {
                            insertVehicle(sentence, vehicle, ID);
                        }
                    }
                    else
                    {
                        sentence.CommandText = INSERT_ENROLLMENT;
                        insertVehicle(sentence, vehicle, (int)sentence.ExecuteScalar());
                    }
                }
            }
        }
        private void insertVehicle(IDbCommand sentence, IVehicle vehicle, int id)
        {
            int boolean = vehicle.Engine.IsStarted ? 1 : 0;            
            sentence.CommandText = $"INSERT INTO vehicle VALUES ({id}, {(int)vehicle.Color}, {vehicle.Engine.HorsePower}, {boolean});";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.CommandText = insertwheelsdoors(vehicle, sentence, id);
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private string insertwheelsdoors(IVehicle vehicle, IDbCommand sentence, int id)
        {
            string sentences = string.Empty;
            foreach (IWheel wheel in vehicle.Wheels)
            {
                sentence.Parameters.Clear();
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                sentences += $"INSERT INTO wheel (vehicleId, pressure) VALUES ({id}, {wheel.Pressure.ToString(provider)});";           
            }
            foreach (IDoor door in vehicle.Doors)
            {
                int boolean = door.IsOpen ? 1 : 0;
                sentence.Parameters.Clear();
                sentences += $"INSERT INTO door (vehicleId, isOpen) VALUES ({id}, {boolean});";
            }
            return sentences;
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private readonly IDictionary<string, object> queryParameters;
            private readonly IList<string> where;
            private IList<WheelDto> wheelsDto;
            private IList<DoorDto> doorsDto;
            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.queryParameters = new Dictionary<string, object>();
                this.where = new List<string>();
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
                                yield return this.enrollmentProvider
                                    .import(
                                    reader.GetValue(0).ToString(),
                                    Convert.ToInt16(reader.GetValue(1)));
                            }
                        }
                    }
                }

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
                this.queryParameters.Add($"@color", (int)color);
                this.where.Add($"color = @color");
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.queryParameters.Add($"@engineIsStarted", started);
                this.where.Add($"engineIsStarted = @engineIsStarted");
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.where.Add($"number = @number");
                this.queryParameters.Add($"@number", enrollment.Number);
                this.queryParameters.Add($"@Serial", enrollment.Serial);
                this.where.Add($"Serial = @Serial");
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.queryParameters.Add($"@Serial", serial);
                this.where.Add($"Serial = @Serial");
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.queryParameters.Add($"@{FieldNames.engineHorsePower}", horsePower);
                this.where.Add($"engineHorsePower = @engineHorsePower");
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.where.Add($"engineHorsePower BETWEEN @min AND @max");
                this.queryParameters.Add("@min", min);
                this.queryParameters.Add("@max", max);
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
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = SELECT_ENROLLMENT_VEHICLE;

                        if (this.where.Count > 0)
                        {
                            sentence.CommandText += $" WHERE {string.Join($" AND ", this.where)}";
                        }
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)reader.GetValue(2);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    if (sentence2.Parameters == null)
                                    {
                                        foreach (IDbDataParameter parameter in sentence.Parameters)
                                        {
                                            sentence2.Parameters.Add(parameter);
                                        }
                                    }

                                    this.doorsDto = new List<DoorDto>();
                                    this.wheelsDto = new List<WheelDto>();
                                    DBCommandExtensions.setParameter(sentence2, $"id", id);
                                    //FieldNames key = check_tag_name(FieldNames.id, TableNames.door);
                                    sentence2.CommandText = SELECT_DOOR;
                                    elements(sentence2, TableNames.door);
                                    sentence2.CommandText = SELECT_WHEEL;
                                    elements(sentence2, TableNames.wheel);
                                }
                                yield return this.vehicleBuilder.import(
                                new VehicleDto()
                                {
                                    Engine = new EngineDto()
                                    {
                                        IsStarted = Convert.ToBoolean(reader.GetValue(4)),
                                        HorsePower = Convert.ToInt32(reader.GetValue(5))
                                    },
                                    Enrollment = new EnrollmentDto()
                                    {
                                        Serial = reader.GetValue(0).ToString(),
                                        Number = Convert.ToInt32(reader.GetValue(1))
                                    },
                                    Wheels = this.wheelsDto.ToArray(),
                                    Doors = this.doorsDto.ToArray(),
                                    Color = (CarColor)Enum.Parse(typeof(CarColor), reader.GetValue(3).ToString())
                                });
                            }
                            reader.Close();
                        }
                    }
                }
            }

            void elements(IDbCommand sentence2, TableNames type)
            {
                using (IDataReader reader2 = sentence2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        switch (type)
                        {
                            case TableNames.door:
                                this.doorsDto.Add(new DoorDto(Convert.ToBoolean(reader2.GetValue(0))));
                                break;
                            case TableNames.wheel:
                                this.wheelsDto.Add(new WheelDto(Convert.ToDouble(reader2.GetValue(0))));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}