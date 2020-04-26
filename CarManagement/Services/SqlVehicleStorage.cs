using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                        sentence.CommandText = $@"{QueryWords.USE} CarManagement;
                        {QueryWords.SELECT} {$"{QueryWords.COUNT}({FieldNames.enrollmentId})"} {QueryWords.FROM} {TableNames.vehicle}";
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
                    sentence.CommandText = $"{$"{QueryWords.DELETE} {QueryWords.FROM} {TableNames.wheel};"} {$"{QueryWords.DELETE} {QueryWords.FROM} {TableNames.door};"} {$"{QueryWords.DELETE} {QueryWords.FROM} {TableNames.vehicle};"}";
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
                    FieldNames id = check_tag_name(FieldNames.id, TableNames.enrollment);
                    sentence.CommandText = $@"{QueryWords.DECLARE} @ID INT = {QueryWords.SELECT} {FieldNames.id} {QueryWords.FROM} {TableNames.enrollment} {QueryWords.WHERE} {$"{FieldNames.serial} = @{FieldNames.serial}"} {QueryWords.AND} {$"{FieldNames.number} = @{FieldNames.number}"};
                                            {QueryWords.DELETE} {QueryWords.FROM} {TableNames.wheel} {QueryWords.WHERE} {QueryWords.WHERE} {$"{id} = @ID"}; 
                                            {QueryWords.DELETE} {QueryWords.FROM} {TableNames.door} {QueryWords.WHERE} {$"{id} = @ID"}; 
                                            {QueryWords.DELETE} {QueryWords.FROM} {TableNames.vehicle} {QueryWords.WHERE} {$"{id} = @ID"};";
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
                    sentence.CommandText = $"{QueryWords.SELECT} {FieldNames.id} {QueryWords.FROM} {TableNames.enrollment} {QueryWords.WHERE} {$"{FieldNames.serial} = @{FieldNames.serial}"} {QueryWords.AND} {$"{FieldNames.number} = @{FieldNames.number}"}";
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
                        FieldNames key = check_tag_name(FieldNames.id, TableNames.enrollment);
                        sentence.CommandText = $"{QueryWords.SELECT} * {QueryWords.FROM} {TableNames.enrollment} {QueryWords.WHERE} {key} = @{FieldNames.id}";
                        if (sentence.ExecuteScalar() != null)
                        {
                            FieldNames id = check_tag_name(FieldNames.id, TableNames.vehicle);
                            sentence.CommandText = $@"
                            {QueryWords.UPDATE} {TableNames.vehicle}
                            {QueryWords.SET} {FieldNames.color} = @{FieldNames.color}, {FieldNames.engineIsStarted} = @{FieldNames.engineIsStarted}, {FieldNames.engineHorsePower} = @{FieldNames.engineHorsePower}
                            {QueryWords.WHERE} {id} = @{FieldNames.id}";

                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            id = check_tag_name(FieldNames.id, TableNames.wheel);
                            sentence.CommandText = $"{$"{QueryWords.DELETE} {QueryWords.FROM} {TableNames.wheel} {QueryWords.WHERE} {id} = @{FieldNames.id};"} {$"{QueryWords.DELETE} {QueryWords.FROM} {TableNames.door} {QueryWords.WHERE} {id} = @{FieldNames.id};"}";
                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            insertwheelsdoors(vehicle, sentence, ID);
                        }
                        else
                        {
                            insertVehicle(sentence, vehicle, ID);
                        }
                    }
                    else
                    {
                        sentence.CommandText = $"{QueryWords.INSERT} {QueryWords.INTO} {TableNames.enrollment} ({FieldNames.serial}, {FieldNames.number}) { QueryWords.OUTPUT} INSERTED.ID {QueryWords.VALUES} (@{FieldNames.serial}, @{FieldNames.number})";
                        ID = (int)sentence.ExecuteScalar();
                        DBCommandExtensions.setParameter(sentence, FieldNames.id, ID );
                        insertVehicle(sentence, vehicle, ID);
                    }
                }
            }
        }
        private void insertVehicle(IDbCommand sentence, IVehicle vehicle, int id)
        {
            sentence.Parameters.Clear();
            DBCommandExtensions.setParameters
            (
                sentence,
                new Dictionary<FieldNames, object>() 
                {
                    { FieldNames.id, id },
                    { FieldNames.color, (int)vehicle.Color },
                    { FieldNames.engineIsStarted, vehicle.Engine.IsStarted ? 1 : 0 },
                    { FieldNames.engineHorsePower, vehicle.Engine.HorsePower }
                }
            );
            
            sentence.CommandText = $"{QueryWords.INSERT} {QueryWords.INTO} {TableNames.vehicle} {QueryWords.VALUES} (@{FieldNames.id}, @{FieldNames.color}, @{FieldNames.engineHorsePower}, @{FieldNames.engineIsStarted})";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            insertwheelsdoors(vehicle, sentence, id);
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence, int id)
        {
            foreach (IWheel wheel in vehicle.Wheels)
            {
                makeWheelDoor
                (
                    sentence, 
                    new Dictionary<FieldNames, object>() 
                    {
                        { FieldNames.id, id }, 
                        { FieldNames.pressure,  wheel.Pressure } 
                    }, TableNames.wheel
                );
            }
            foreach (IDoor door in vehicle.Doors)
            {
                makeWheelDoor
                (
                    sentence, 
                    new Dictionary<FieldNames, object>() 
                    { 
                        { FieldNames.id, id }, 
                        { FieldNames.isOpen, door.IsOpen } 
                    }, TableNames.door
                );
            }
        }
        private void makeWheelDoor(IDbCommand sentence, IDictionary<FieldNames, object> parameters, TableNames table)
        {
            sentence.Parameters.Clear();
            DBCommandExtensions.setParameters(sentence, parameters);
            string[] values = new string[parameters.Count];
            string[] fields = new string[parameters.Count];
            int counter = 0;
            foreach (KeyValuePair<FieldNames, object> parameter in parameters)
            {
                fields[counter] = $"{check_tag_name(parameter.Key, table)}";
                values[counter] = $"@{parameter.Key}";
                counter++;
            }

            sentence.CommandText = $"{QueryWords.INSERT} {QueryWords.INTO} {table} ({string.Join(", ", fields)}) {QueryWords.VALUES} ({string.Join(", ", values)})";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
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
                        sentence.CommandText = $"{QueryWords.SELECT} * {QueryWords.FROM} {TableNames.enrollment}";
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
                this.queryParameters.Add($"@{FieldNames.color}", (int)color);
                this.where.Add($"{FieldNames.color} = @{FieldNames.color}");
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.queryParameters.Add($"@{FieldNames.engineIsStarted}", started);
                this.where.Add($"{FieldNames.engineIsStarted} = @{FieldNames.engineIsStarted}");
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.where.Add($"{FieldNames.number} = @{FieldNames.number}");
                this.queryParameters.Add($"@{FieldNames.number}", enrollment.Number);
                this.queryParameters.Add($"@{FieldNames.serial}", enrollment.Serial);
                this.where.Add($"{FieldNames.serial} = @{FieldNames.serial}");
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.queryParameters.Add($"@{FieldNames.serial}", serial);
                this.where.Add($"{FieldNames.serial} = @{FieldNames.serial}");
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.queryParameters.Add($"@{FieldNames.engineHorsePower}", horsePower);
                this.where.Add($"{FieldNames.engineHorsePower} = @{FieldNames.engineHorsePower}");
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.where.Add($"{FieldNames.engineHorsePower} {QueryWords.BETWEEN} {"@min"} {QueryWords.AND} {"@max"}");
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
                        sentence.CommandText = $@"
                            {QueryWords.SELECT} {string.Join(", ", new List<string>() { $"e.{FieldNames.serial}", $"e.{FieldNames.number}", $"e.{FieldNames.id}", $"v.{FieldNames.color}", $"v.{FieldNames.engineIsStarted}", $"v.{FieldNames.engineHorsePower}" })}
                            {QueryWords.FROM} {TableNames.enrollment} e {QueryWords.INNER} {QueryWords.JOIN} {TableNames.vehicle} v {QueryWords.ON} {$"{FieldNames.id} = {FieldNames.enrollmentId}"}";

                        if (this.where.Count > 0)
                        {
                            sentence.CommandText += $" {QueryWords.WHERE} {string.Join($" {QueryWords.AND} ", this.where)}";
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
                                    DBCommandExtensions.setParameter(sentence2, $"{FieldNames.id}", id);
                                    FieldNames key = check_tag_name(FieldNames.id, TableNames.door);
                                    sentence2.CommandText = $"{QueryWords.SELECT} {FieldNames.isOpen} {QueryWords.FROM} {TableNames.door} {QueryWords.WHERE} {$"{key} = @{FieldNames.id}"}";
                                    elements(sentence2, TableNames.door);
                                    sentence2.CommandText = $"{QueryWords.SELECT} {FieldNames.pressure} {QueryWords.FROM} {TableNames.wheel} {QueryWords.WHERE} {$"{key} = @{FieldNames.id}"}";
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
                                this.doorsDto.Add(new DoorDto() { IsOpen = Convert.ToBoolean(reader2.GetValue(0)) });
                                break;
                            case TableNames.wheel:
                                this.wheelsDto.Add(new WheelDto() { Pressure = Convert.ToDouble(reader2.GetValue(0)) });
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