using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox.Extensions.DbCommands;
using ToolBox.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private IDataParameter parameter;
        private readonly IList<int> idList;
        private int id;
        private object query;
        private QueryBuilder queryBuilder;
        private List<whereFieldValues> whereValues;
        const string COUNT_VEHICLE = @"USE CarManagement
            SELECT count(enrollmentId) AS 'Count' FROM vehicle";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            this.idList = new List<int>();
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
                    this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "enrollment", values = new List<string>() { "id" } } }});
                    sentence.CommandText = $"{this.queryBuilder.querySelect}";
                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        while (reader.Read())
                            this.idList.Add((int)reader.GetValue(0));
                        reader.Close();
                    }
                    foreach (int id in this.idList)
                    {
                        this.parameter = setParameter(sentence, "@id", id);
                        sentence.Parameters.Add(this.parameter);
                        sentence.CommandText = deleteAllQuery().ToString();
                        sentence.ExecuteNonQuery();
                        sentence.Parameters.Remove(this.parameter);
                    }
                }
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
                using (IDbCommand sentence = con.CreateCommand())
                {
                    con.Open();
                    sentence.Parameters.Add(setParameter(sentence, "@serial", enrollment.Serial));
                    sentence.Parameters.Add(setParameter(sentence, "@number", enrollment.Number));
                    selectIdEnrollment(sentence);
                    readEnrollmentId(sentence);
                    selectVehicle(sentence);
                    if (sentence.ExecuteScalar() != null)
                    {
                        sentence.CommandText = deleteAllQuery().ToString();
                        sentence.ExecuteNonQuery();
                    }
                }
            }
        }
        public void set(IVehicle vehicle)
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    sentence.Parameters.Add(setParameter(sentence, "@serial", vehicle.Enrollment.Serial));
                    sentence.Parameters.Add(setParameter(sentence, "@number", vehicle.Enrollment.Number));
                    selectIdEnrollment(sentence);
                    if (sentence.ExecuteScalar() != null)
                    {
                        readEnrollmentId(sentence);
                        this.query = selectVehicle(sentence);
                        IList<string> columnsValues = setVehicleParameters(vehicle, sentence);
                        if (this.query != null)
                            updateVehicle(sentence, vehicle, columnsValues);
                        else
                        {
                            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "vehicle", values = columnsValues } } });
                            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
                            insertwheelsdoors(vehicle, sentence);
                        }
                    }
                    else
                    {
                        insertEnrollment(sentence);
                        selectIdEnrollment(sentence);
                        readEnrollmentId(sentence);
                        insertVehicle(sentence, vehicle);
                    }
                }
            }
        }

        private static IDataParameter insertParams(IDbCommand sentence, IList<string> columnsValues, object thing, string name)
        {
            columnsValues.Add(name);
            return setParameter(sentence, $"@{name}", thing);
        }
        private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = thing;
            return parameter;
        }
        private static IList<string> setVehicleParameters(IVehicle vehicle, IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>();
            sentence.Parameters.Add(insertParams(sentence, columnsValues, (int)vehicle.Color, "color"));
            sentence.Parameters.Add(insertParams(sentence, columnsValues, vehicle.Engine.IsStarted ? 1 : 0, "engineIsStarted"));
            sentence.Parameters.Add(insertParams(sentence, columnsValues, vehicle.Engine.HorsePower, "engineHorsePower"));
            return columnsValues;
        }
        private void insertEnrollment(IDbCommand sentence)
        {
            IList<string> columnsValues = new List<string>
            {
                "serial",
                "number"
            };
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "enrollment", values = columnsValues } } });
            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertVehicle(IDbCommand sentence, IVehicle vehicle)
        {
            IList<string> columnsValues = setVehicleParameters(vehicle, sentence);
            columnsValues.Add("id");
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "vehicle", values = columnsValues } } });
            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            insertwheelsdoors(vehicle, sentence);
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence)
        {
            foreach (IWheel wheel in vehicle.Wheels)
                makeWheelDoor(sentence, wheel.Pressure, "pressure", "wheel");
            foreach (IDoor door in vehicle.Doors)
                makeWheelDoor(sentence, door.IsOpen, "isOpen", "door");
        }
        private void makeWheelDoor(IDbCommand sentence, object parameter, string column, string table)
        {
            sentence.Parameters.Clear();
            List<string> columnsValues = new List<string>();
            sentence.Parameters.Add(insertParams(sentence, columnsValues, this.id, "id"));
            sentence.Parameters.Add(insertParams(sentence, columnsValues, parameter, column));
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = table, values = columnsValues } } });
            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private object selectVehicle(IDbCommand sentence)
        {
            IList<whereFieldValues> whereValues = new List<whereFieldValues>
            {
                WhereParam("=", "id")
            };

            this.queryBuilder = new QueryBuilder
            (
                new iQuery()
                {
                    tablesColumns = new List<FieldValues>()
                    {
                        new FieldValues()
                        {
                            field = "vehicle",
                            values = new List<string>()
                            {
                                "*"
                            }
                        }
                    },
                    whereValues = whereValues
                }
            );
            sentence.CommandText = $"{this.queryBuilder.querySelect}";
            return sentence.ExecuteScalar();
        }
        private void selectIdEnrollment(IDbCommand sentence)
        {
            this.queryBuilder = new QueryBuilder
            (
                new iQuery()
                {
                    tablesColumns = new List<FieldValues>()
                    {
                        new FieldValues()
                        {
                            field = "enrollment",
                            values = new List<string>()
                            {
                                "id"
                            }
                        }
                    },
                    whereValues = new List<whereFieldValues>()
                    {
                        WhereParam("=", "serial"),
                        WhereParam("=", "number"),
                    }
                }
            );
            sentence.CommandText = $"{this.queryBuilder.querySelect}";
        }
        private static whereFieldValues WhereParam(string key, string param)
        {
            whereFieldValues whereValues = new whereFieldValues()
            {
                field = param,
                values = new
                List<string>
                {
                    $"@{param}"
                },
                key = key
            };
            return whereValues;
        }
        private void readEnrollmentId(IDbCommand sentence)
        {
            using (IDataReader reader = sentence.ExecuteReader())
            {
                reader.Read();
                this.id = (int)reader.GetValue(0);
                sentence.Parameters.Add(setParameter(sentence, $"@{reader.GetName(0)}", reader.GetValue(0)));
                reader.Close();
            }
        }
        private void updateVehicle(IDbCommand sentence, IVehicle vehicle, IList<string> columnsValues)
        {
            this.queryBuilder = new QueryBuilder
            (
                new iQuery()
                {
                    tablesColumns = new List<FieldValues>()
                    {
                        new FieldValues()
                        {
                            field = "vehicle",
                            values = columnsValues
                        }
                    },
                    whereValues = new List<whereFieldValues>()
                    {
                        WhereParam("=", "id")
                    }
                }
            );
            sentence.CommandText = $"{this.queryBuilder.queryUpdate}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            deleteWheelsDoors(sentence);
            insertwheelsdoors(vehicle, sentence);
        }
        private StringBuilder deleteAllQuery()
        {
            IList<whereFieldValues> wherevalues = new List<whereFieldValues>()
            {
                WhereParam("=", "id")
            };
            StringBuilder query = new StringBuilder();
            this.queryBuilder = new QueryBuilder(new iQuery() {tablesColumns = new List<FieldValues>() { new FieldValues() { field = "wheel", values = new List<string>() }}, whereValues = wherevalues});
            query.Insert(query.Length, $"{this.queryBuilder.queryDelete};");
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "door", values = new List<string>() } }, whereValues = wherevalues });
            query.Insert(query.Length, $"{this.queryBuilder.queryDelete};");
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "vehicle", values = new List<string>() } }, whereValues = wherevalues });
            query.Insert(query.Length, $"{this.queryBuilder.queryDelete};");
            return query;
        }
        private void deleteWheelsDoors(IDbCommand sentence)
        {
            this.whereValues = new List<whereFieldValues>()
            {
                WhereParam("=", "id")
            };
            this.queryBuilder = new QueryBuilder(new iQuery
            {
                whereValues = this.whereValues,
                tablesColumns = new List<FieldValues>() { new FieldValues() { field = "wheel" } }
            });
            sentence.CommandText = $"{this.queryBuilder.queryDelete}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            this.queryBuilder = new QueryBuilder(new iQuery
            {
                whereValues = this.whereValues,
                tablesColumns = new List<FieldValues>() { new FieldValues() { field = "door" } }
            });
            sentence.CommandText = $"{this.queryBuilder.queryDelete}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private class PrvVehicleQuery : IVehicleQuery
        {
            private QueryBuilder queryBuilder;
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private readonly IDictionary<string, object> queryParameters;
            private readonly IList<whereFieldValues> whereValues;
            private IList<WheelDto> wheelsDto;
            private IList<DoorDto> doorsDto;
            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.queryParameters = new Dictionary<string, object>();
                this.whereValues = new List<whereFieldValues>();
            }
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "enrollment", values = new List<string>() { "serial, number " } } } });
                        sentence.CommandText = $"{this.queryBuilder.querySelect}";
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
                this.whereValues.Add(WhereParam("=", "color"));
                this.queryParameters.Add("@color", (int)color);
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.whereValues.Add(WhereParam("=", "engineIsStarted"));
                this.queryParameters.Add("@engineIsStarted", started);
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.whereValues.Add(WhereParam("=", "number"));
                this.queryParameters.Add("@number", enrollment.Number);
                this.whereValues.Add(WhereParam("=", "serial"));
                this.queryParameters.Add("@serial", enrollment.Serial);
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.whereValues.Add(WhereParam("=", "serial"));
                this.queryParameters.Add("@serial", serial);
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.whereValues.Add(WhereParam("=", "engineHorsePower"));
                this.queryParameters.Add("@engineHorsePower", horsePower);
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.whereValues.Add
                (
                    new whereFieldValues()
                    {
                        field = "engineHorsePower",
                        values =
                        {
                            "@min",
                            "@max"
                        },
                        key = "BETWEEN"
                    }

                );
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
                        this.queryBuilder = new QueryBuilder
                        (
                            new iQuery()
                            {
                                tablesColumns = new List<FieldValues>()
                                {
                                    new FieldValues()
                                    {
                                        field = "enrollment",
                                        values = new List<string>
                                        {
                                            "serial",
                                            "number",
                                            "id"
                                        }
                                    },
                                    new FieldValues()
                                    {
                                        field = "vehicle",
                                        values = new List<string>
                                        {
                                            "color",
                                            "engineIsStarted",
                                            "engineHorsePower"
                                        }
                                    }
                                },
                                innerValues = new List<FieldValues>()
                                {
                                    new FieldValues(){
                                        field = "id",
                                        values = new List<string>()
                                        {
                                            "enrollmentId"
                                        }
                                    }
                                },
                                whereValues = this.whereValues
                            }
                        );
                        sentence.CommandText = $"{this.queryBuilder.queryComplexSelect}";
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)reader.GetValue(2);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    copyParameters(sentence, sentence2);
                                    this.doorsDto = new List<DoorDto>();
                                    this.wheelsDto = new List<WheelDto>();
                                    IList<whereFieldValues> whereValues = new List<whereFieldValues>()
                                    {
                                        WhereParam("=", "id")
                                    };
                                    sentence2.Parameters.Add(setParameter(sentence2, "@id", id));
                                    this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "door", values = new List<string>() { "isOpen" } } }, whereValues = whereValues });
                                    sentence2.CommandText = $"{this.queryBuilder.querySelect}";
                                    elements(sentence2, "door");
                                    this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "wheel", values = new List<string>() { "pressure" } } }, whereValues = whereValues });
                                    sentence2.CommandText = $"{this.queryBuilder.querySelect}";
                                    elements(sentence2, "wheel");
                                }
                                yield return this.vehicleBuilder.import(
                                    new VehicleDto(
                                        (CarColor)Enum.Parse(typeof(CarColor),
                                        reader.GetValue(3).ToString()),
                                        new EngineDto(
                                            Convert.ToInt32(reader.GetValue(5)),
                                            Convert.ToBoolean(reader.GetValue(4))),
                                        new EnrollmentDto(
                                            reader.GetValue(0).ToString(),
                                            Convert.ToInt32(reader.GetValue(1))),
                                        this.wheelsDto.ToArray(),
                                        this.doorsDto.ToArray()));
                            }
                            reader.Close();
                        }
                    }
                }
            }
            void elements(IDbCommand sentence2, string type)
            {
                using (IDataReader reader2 = sentence2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        if (type == "door")
                            this.doorsDto.Add(new DoorDto(Convert.ToBoolean(reader2.GetValue(0))));
                        else
                            this.wheelsDto.Add(new WheelDto(Convert.ToDouble(reader2.GetValue(0))));
                    }
                }
            }
            private static void copyParameters(IDbCommand sentence, IDbCommand sentence2)
            {
                if (sentence2.Parameters == null)
                {
                    foreach (IDbDataParameter parameter in sentence.Parameters)
                        sentence2.Parameters.Add(parameter);
                }
            }
        }
    }
}