using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
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
        private int id;
        private object query;
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
                    sentence.CommandText = buildSimpleQuery("SELECT id ", "enrollment").ToString();
                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.idList.Add(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                    }
                    foreach (string id in this.idList)
                    {
                        this.parameter = setParameter(sentence, "@id", id);
                        sentence.Parameters.Add(this.parameter);
                        sentence.CommandText = deleteAll().ToString();
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
                    sentence.Parameters.Add(setParameter(sentence, "@id", this.id));
                    if (sentence.ExecuteScalar() != null)
                    {
                        sentence.CommandText = deleteAll().ToString();
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
                        setVehicleParameters(vehicle, sentence);

                        if (this.query != null)
                        {
                            updateVehicle(sentence);
                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            deleteWheelsDoors(sentence);
                            insertwheelsdoors(vehicle, sentence);
                        }
                        else
                        {
                            insertVehicle(sentence);
                            insertwheelsdoors(vehicle, sentence);
                        }
                    }
                    else
                    {
                        insertEnrollment(sentence);
                        selectIdEnrollment(sentence);
                        readEnrollmentId(sentence);
                        setVehicleParameters(vehicle, sentence);
                        insertVehicle(sentence);
                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                        insertwheelsdoors(vehicle, sentence);
                    }
                }
            }
        }

        //parameterTools
        private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = thing;
            return parameter;
        }
        private static void setVehicleParameters(IVehicle vehicle, IDbCommand sentence)
        {
            sentence.Parameters.Add(setParameter(sentence, "@color", (int)vehicle.Color));
            sentence.Parameters.Add(setParameter(sentence, "@engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0));
            sentence.Parameters.Add(setParameter(sentence, "@engineHorsePower", vehicle.Engine.HorsePower));
        }
        private static void inserObject(string query, IDbCommand sentence, string name, object param)
        {
            IDataParameter parameter = setParameter(sentence, name, param);
            sentence.Parameters.Add(parameter);
            sentence.CommandText = query;
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.Parameters.Remove(parameter);
        }

        //insertTools
        private static void insertEnrollment(IDbCommand sentence)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("@serial", "serial");
            dictionary.Add("@number", "number");
            sentence.CommandText = buildSimpleQuery("INSERT INTO", "enrollment", dictionary, dictionary).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static void insertVehicle(IDbCommand sentence)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("@id", "enrollmentId");
            dictionary.Add("@color", "color");
            dictionary.Add("@engineHorsePower", "engineHorsePower");
            dictionary.Add("@engineIsStarted", "engineIsStarted");
            sentence.CommandText = buildSimpleQuery("INSERT INTO ", "vehicle", dictionary, dictionary).ToString();
        }
        private static void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence)
        {
            foreach (IWheel wheel in vehicle.Wheels)
            {
                makeWheelDoor(sentence, wheel.Pressure, "pressure", "wheel");
            }
            foreach (IDoor door in vehicle.Doors)
            {
                makeWheelDoor(sentence, door.IsOpen, "isOpen", "door");
            }
        }
        private static void makeWheelDoor(IDbCommand sentence, object parameter, string column, string table)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("@id", "vehicleId");
            dictionary.Add($"@{column}", column);
            inserObject(buildSimpleQuery("INSERT INTO ", table, dictionary, dictionary).ToString(), sentence, $"@{column}", parameter);
        }
        private static void makeElements(string type, IDbCommand sentence, object[] things)
        {
            foreach (dynamic thing in things)
            {
                if (type == "door")
                {
                    makeWheelDoor(sentence, thing.IsOpen, "isOpen", "door");
                }
                else if (type == "wheel")
                {
                    makeWheelDoor(sentence, thing.Pressure, "pressure", "wheel");
                }
            }
        }

        //selectTools
        private static object selectVehicle(IDbCommand sentence)
        {
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@id", "id");
            sentence.CommandText = buildSimpleQuery("SELECT * ", "vehicle", where).ToString();
            return sentence.ExecuteScalar();
        }

        private static void selectIdEnrollment(IDbCommand sentence)
        {
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@serial", "serial");
            where.Add("@number", "number");
            sentence.CommandText = buildSimpleQuery("SELECT id", "enrollment", where).ToString();
        }
        private static void readEnrollmentId(IDbCommand sentence)
        {
            using (IDataReader reader = sentence.ExecuteReader())
            {
                //sentence.Parameters.Clear();
                reader.Read();
                sentence.Parameters.Add(setParameter(sentence, $"@{reader.GetName(0)}", reader.GetValue(0)));
                reader.Close();
            }
        }

        //updateToools
        private static void updateVehicle(IDbCommand sentence)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("@color", "color");
            dictionary.Add("@engineHorsePower", "engineHorsePower");
            dictionary.Add("@engineIsStarted", "engineIsStarted");
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@id", "enrollmentId");
            sentence.CommandText = buildSimpleQuery("UPDATE ", "vehicle", where, dictionary).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }

        //deleteTools
        private static StringBuilder deleteAll()
        {
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@id", "id");
            StringBuilder query = new StringBuilder();
            query.Insert(query.Length, $@"
                {buildSimpleQuery("DELETE ", "wheel", where, where)};
                {buildSimpleQuery("DELETE ", "door", where, where)};");
            query.Insert(query.Length, $"{buildSimpleQuery("DELETE ", "vehicle", where, where)};");
            return query;
        }
        private static void deleteWheelsDoors(IDbCommand sentence)
        {
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@id", "vehicleId");
            //sentence.CommandText = createQuery(makeSelectDelete("DELETE ", "wheel "), buildConditions(where, "vehicle"));
            sentence.CommandText = buildSimpleQuery("DELETE ", "wheel", where).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.CommandText = buildSimpleQuery("DELETE ", "door", where).ToString();
            //sentence.CommandText = createQuery(makeSelectDelete("DELETE ", "door "), buildConditions(where, "vehicle"));
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }

        //stringtools
        public static StringBuilder buildSimpleQuery(string command, string table, IDictionary<string, string> conditions = null, IDictionary<string, string> columnsValues = null)
        {
            StringBuilder query = new StringBuilder();
            query.Capacity = 60;
            //select without condition(s)
            if (conditions == null && columnsValues == null)
            {
                query.Insert(query.Length, $"{command} FROM {table}");
            }
            //select-delete with condition(s)
            else if (command.Contains("SELECT") || command.Contains("DELETE"))
            {
                query.Insert(query.Length, $"{command} FROM {table}");
                addConditions(table, conditions, query);
            }
            //update
            else if (command.Contains("UPDATE"))
            {
                //update with condition(s)
                query.Insert(query.Length, $"{command}{table} SET ");
                int counter = 0;
                foreach (KeyValuePair<string, string> columnValue in columnsValues)
                {
                    query.Insert(query.Length, $"{columnValue.Value} = {columnValue.Key}");
                    if (counter < columnsValues.Count - 1)
                    {
                        query.Insert(query.Length, ", ");
                    }
                    counter++;
                }
                addConditions(table, conditions, query);
            }
            //insert
            else if (command.Contains("INSERT"))
            {
                query.Insert(query.Length, $"{command} {table}");
                addFields(columnsValues.Values, query);
                query.Insert(query.Length, " VALUES ");
                addFields(columnsValues.Keys, query);

            }
            return query;
        }
        private static void addConditions(string table, IDictionary<string, string> conditions, StringBuilder query)
        {
            int counter = 0;
            foreach (KeyValuePair<string, string> condition in conditions)
            {
                StringBuilder conditionsp = new StringBuilder();
                conditionsp.Capacity = 30;
                if (condition.Value == "id")
                {
                    if (table == "vehicle")
                    {
                        conditionsp.Insert(conditionsp.Length, $"enrollmentId = {condition.Key}");
                    }
                    else if (table != "enrollment")
                    {
                        conditionsp.Insert(conditionsp.Length, $"{condition.Value} = {condition.Key}");
                    }
                    else
                    {
                        conditionsp.Insert(conditionsp.Length, $"vehicleId = {condition.Key}");
                    }
                }
                else
                {
                    conditionsp.Insert(conditionsp.Length, $"{condition.Value} = {condition.Key}");
                    if (counter == 0 || conditions.Count == 1)
                    {
                        query.Insert(query.Length, $" WHERE {conditionsp}");
                    }
                    else if (counter < conditions.Count - 1)
                    {
                        query.Insert(query.Length, $", AND {conditionsp}");
                        if (counter == conditions.Count - 1)
                        {
                            query.Insert(query.Length, ", ");
                        }
                    }
                }
                counter++;
            }
        }
        private static void addFields(ICollection<string> values, StringBuilder query)
        {
            int counter = 0;
            foreach (string value in values)
            {
                if (counter == 0)
                {
                    query.Insert(query.Length, $"({value}");
                    if (values.Count == 1)
                    {
                        query.Insert(query.Length, ")");
                    }
                }
                else if (counter < values.Count)
                {
                    query.Insert(query.Length, $", {value}");
                    if (counter == values.Count - 1)
                    {
                        query.Insert(query.Length, ") ");
                    }
                }
                counter++;
            }
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
                this.queryParts.Add("@engineHorsePower", createCondition("engineHorsePower", "BETWEEN", "@min", "@max"));
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
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        StringBuilder query = new StringBuilder(60);
                        query.Insert(query.Length, SELECT_VEHICLE_HEAD);
                        sentence.CommandText = createQuery(query, this.queryParts).ToString();
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);

                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.enrollmentDto = this.vehicleBuilder2.enrollmentDtoBuilder(reader.GetValue(0).ToString(), Convert.ToInt32(reader.GetValue(1)));
                                this.id = (int)reader.GetValue(2);
                                this.color = (CarColor)Enum.Parse(typeof(CarColor), reader.GetValue(3).ToString());
                                this.engineDto = this.vehicleBuilder2.engineDtoBuilder(Convert.ToBoolean(reader.GetValue(4)), Convert.ToInt32(reader.GetValue(5)));
                                CreateElement(con, sentence, "wheel");
                                CreateElement(con, sentence, "door");

                                yield return this.vehicleBuilder.import(this.vehicleBuilder2.vehicleDtoBuilder(this.enrollmentDto, this.engineDto, this.color, this.wheelsDto.ToArray(), this.doorsDto.ToArray()));
                            }
                            reader.Close();
                        }
                    }
                }
            }

            //stringTools
            private static StringBuilder createQuery(StringBuilder query, IDictionary<string, string> conditionParts)
            {
                int counter = 0;
                foreach (string conditionPart in conditionParts.Values)
                {
                    if (counter == 0)
                    {
                        query.Insert(query.Length, $" WHERE {conditionPart}");
                    }
                    else
                    {
                        query.Insert(query.Length, $" AND {conditionPart}");
                    }
                    counter++;
                }
                return query;
            }
            private static string addWhere(string field, string condition)
            {
                string where = $" WHERE {createFieldPart(field, "=", condition)}";
                return where;
            }
            private static string makeSelectDelete(string command, string table)
            {
                string select = $"{command} FROM {table}";
                return select;
            }
            private static void addParametersDictionary(string value, string column, object parameter, IDictionary<string, object> queryParameters, IDictionary<string, string> queryParts, string key = "=")
            {
                queryParameters.Add(value, parameter);
                queryParts.Add(value, createFieldPart(column, "=", value));
            }
            private static string createCondition(string column, string key, string value, string value2 = null)
            {
                string query;
                if (key == "BETWEEN")
                {
                    query = $"{column} {key} {createFieldPart(value, "AND", value2)}";
                }
                else
                {
                    query = createFieldPart(column, key, value);
                }
                return query;
            }
            private static string createFieldPart(string column, string key, string value)
            {
                return $"{column} {key} {value}";
            }

            //parameterTools
            private static void copyParameters(IDbCommand sentence, IDbCommand sentence2)
            {
                if (sentence2.Parameters == null)
                {
                    foreach (IDbDataParameter parameter in sentence.Parameters)
                    {
                        sentence2.Parameters.Add(parameter);
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

            //otherTools
            private void CreateElement(IDbConnection con, IDbCommand sentence, string type)
            {
                using (IDbCommand sentence2 = con.CreateCommand())
                {
                    copyParameters(sentence, sentence2);
                    sentence2.Parameters.Add(setParameter(sentence2, "@id", this.id));
                    if (type == "door")
                    {
                        sentence2.CommandText = makeSelectDelete("SELECT isOpen ", type) + addWhere("vehicleId", "@id");

                    }
                    else if (type == "wheel")
                    {
                        sentence2.CommandText = makeSelectDelete("SELECT pressure ", type) + addWhere("vehicleId", "@id");
                    }
                    extractElement(sentence2, type);
                }
            }
            private void extractElement(IDbCommand sentence2, string type)
            {
                using (IDataReader reader2 = sentence2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        if (type == "wheel")
                        {
                            this.wheelDto = this.vehicleBuilder2.wheelDtoBuilder(Convert.ToDouble(reader2.GetValue(0)));
                            this.wheelsDto.Add(this.wheelDto);
                        }
                        else if (type == "door")
                        {
                            this.doorDto = this.vehicleBuilder2.doorDtoBuilder(Convert.ToBoolean(reader2.GetValue(0)));
                            this.doorsDto.Add(this.doorDto);
                        }
                    }
                }
            }

            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = makeSelectDelete("SELECT * ", "enrollment");
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