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
        private List<int> idList;
        private object query;
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
                    sentence.CommandText = buildSimpleQuery("SELECT id ", "enrollment").ToString();
                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.idList.Add((int)reader.GetValue(0));
                        }
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
        
        private static void insertEnrollment(IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>();
            columnsValues.Add("serial");
            columnsValues.Add("number");

            sentence.CommandText = buildSimpleQuery("INSERT INTO", "enrollment", columnsValues).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static void insertVehicle(IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>();
            columnsValues.Add("id");
            columnsValues.Add("color");
            columnsValues.Add("engineHorsePower");
            columnsValues.Add("engineIsStarted");
            sentence.CommandText = buildSimpleQuery("INSERT INTO", "vehicle", columnsValues).ToString();
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
            List<string> columnsValues = new List<string>();
            columnsValues.Add("id");
            columnsValues.Add(column);
            buildSimpleQuery("INSERT INTO ", table, columnsValues);
            inserObject(buildSimpleQuery("INSERT INTO ", table, columnsValues).ToString(), sentence, $"@{column}", parameter);
        }

        private static object selectVehicle(IDbCommand sentence)
        {
            List<string> values = new List<string>();
            List<string> columnsValues = new List<string>();
            List<string> keys = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            values.Add("@id");
            whereParams.Add(values, "id");
            keys.Add("=");
            sentence.CommandText = buildSimpleQuery("SELECT * ", "vehicle", columnsValues, whereParams, keys).ToString();
            return sentence.ExecuteScalar();
        }
        private static void selectIdEnrollment(IDbCommand sentence)
        {
            List<string> values = new List<string>();
            List<string> keys = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            List<string> columnsValues = new List<string>();
            values.Add("@serial");
            whereParams.Add(values, "serial");
            keys.Add("=");
            values = new List<string>();
            values.Add("@number");
            whereParams.Add(values, "number");
            keys.Add("=");
            sentence.CommandText = buildSimpleQuery("SELECT id", "enrollment", columnsValues, whereParams, keys).ToString();
        }
        private static void readEnrollmentId(IDbCommand sentence)
        {
            using (IDataReader reader = sentence.ExecuteReader())
            {
                reader.Read();
                sentence.Parameters.Add(setParameter(sentence, $"@{reader.GetName(0)}", reader.GetValue(0)));
                reader.Close();
            }
        }
        private static void updateVehicle(IDbCommand sentence)
        {
            List<string> conditions = new List<string>();
            List<List<string>> values = new List<List<string>>();
            List<string> value = new List<string>();
            List<string> keys = new List<string>();

            List<string> columnsValues = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            columnsValues.Add("color");
            columnsValues.Add("engineHorsePower");
            columnsValues.Add("engineIsStarted");
            value.Add("@id");
            whereParams.Add(value, "id");
            keys.Add("=");
            sentence.CommandText = buildSimpleQuery("UPDATE ", "vehicle", columnsValues, whereParams, keys).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static StringBuilder deleteAllQuery()
        {
            List<string> columnsValues = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            List<string> values = new List<string>();
            List<string> keys = new List<string>();
            values.Add("@id");
            whereParams.Add(values, "id");
            keys.Add("=");
            StringBuilder query = new StringBuilder();
            query.Insert(query.Length, $@"
                {buildSimpleQuery("DELETE ", "wheel", columnsValues, whereParams, keys)};
                {buildSimpleQuery("DELETE ", "door", columnsValues, whereParams, keys)};
                {buildSimpleQuery("DELETE ", "vehicle", columnsValues, whereParams, keys)};");

            return query;
        }
        private static void deleteWheelsDoors(IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            List<string> values = new List<string>();
            List<string> keys = new List<string>();
            values.Add("@id");
            whereParams.Add(values, "id");
            keys.Add("=");

            sentence.CommandText = buildSimpleQuery("DELETE ", "wheel", columnsValues, whereParams, keys).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.CommandText = buildSimpleQuery("DELETE ", "door", columnsValues, whereParams, keys).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }

        private static StringBuilder buildSimpleQuery(string command, string table, List<string> values = null, IDictionary<List<string>, string> whereParams = null, List<string> keys = null)
        {
            StringBuilder query = new StringBuilder(60);
            if (command.Contains("SELECT") || command.Contains("DELETE"))
            {
                query.Insert(query.Length, $"{command} FROM {table}");
                if (whereParams != null)
                {
                    query.Insert(query.Length, where(table, whereParams, keys));
                }
            }
            else if (command.Contains("UPDATE"))
            {
                query.Insert(query.Length, $"{command}{table} SET ");
                int counter = 0;
                foreach (string value in values)
                {
                    query.Insert(query.Length, $"{value} = @{value}");
                    if (counter < values.Count - 1)
                    {
                        query.Insert(query.Length, ", ");
                    }
                    counter++;
                }
                query.Insert(query.Length, where(table, whereParams, keys));
            }
            else if (command.Contains("INSERT"))
            {
                query.Insert(query.Length, $"{command} {table}");

                query.Insert(query.Length, addFields(values, table, "condition"));
                query.Insert(query.Length, " VALUES ");
                query.Insert(query.Length, addFields(values, table, "value"));

            }
            return query;
        }
        private static StringBuilder addFields(List<string> values, string table, string type)
        {
            StringBuilder query = new StringBuilder(50);
            string thing = "";
            int counter = 0;
            foreach (string value in values)
            {
                if (type == "value")
                {
                    thing = $"@{value}";
                }
                else
                {
                    thing = value;
                }
                if (counter == 0)
                {
                    query.Insert(query.Length, $"({checkCondition(thing, table)}");
                    if (values.Count == 1)
                    {
                        query.Insert(query.Length, ")");
                    }
                }
                else if (counter < values.Count)
                {
                    query.Insert(query.Length, $", {checkCondition(thing, table)}");
                    if (counter == values.Count - 1)
                    {
                        query.Insert(query.Length, ") ");
                    }
                }
                counter++;
            }
            return query;
        }
        private static string checkCondition(string condition, string table)
        {
            string fix;
            if (condition == "id" && table != "enrollment")
            {
                if (table == "vehicle")
                    fix = "enrollmentId";
                else
                    fix = "vehicleId";
            }
            else
                fix = condition;
            return fix;
        }
        private static StringBuilder where(string table, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (KeyValuePair<List<string>, string> where in whereParams)
            {
                if (counter == 0 || whereParams.Count == 1)
                {
                    query.Insert(query.Length, " WHERE ");
                }
                else if (counter > 0)
                {
                    query.Insert(query.Length, " AND ");
                }
                query.Insert(query.Length, buildCondition(checkCondition(where.Value, table), keys[counter], where.Key));
                counter++;
            }
            return query;
        }
        private static StringBuilder buildCondition(string condition, string key, List<string> values)
        {
            StringBuilder query = new StringBuilder($"{condition} {key} ", 60);
            if (values.Count != 1)
            {
                if (key == "BETWEEN")
                {
                    query.Insert(query.Length, betweenfields(values));
                }
                else if (key == "IN")
                {
                    query.Insert(query.Length, inFields(values));
                }
            }
            else
            {
                query.Insert(query.Length, values[0]);
            }
            return query;
        }
        private static StringBuilder inFields(List<string> value)
        {
            StringBuilder query = new StringBuilder(60);
            for (int i = 0; i < value.Capacity; i++)
            {
                query.Insert(query.Length, $"({value[i]} ,");
                if (i == value.Capacity - 1)
                {
                    query.Insert(query.Length, $")");
                }
            }
            return query;
        }
        private static StringBuilder betweenfields(List<string> value)
        {
            StringBuilder query = new StringBuilder(60);
            for (int i = 0; i < value.Capacity; i++)
            {
                query.Insert(query.Length, value[i]);
                if (i == 0)
                {
                    query.Insert(query.Length, $" AND ");
                }
            }
            return query;
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private CarColor color;
            private bool engineIsStarted;
            private IEnrollment enrollment;
            private string enrollmentSerial;
            private int horsePower;
            private int min;
            private int max;
            private int id;
            private IDictionary<string, object> queryParameters;
            private List<string> conditions;
            private IDictionary<List<string>, string> whereParams;
            private List<List<string>> values;
            private List<string> value;
            private List<string> keys;
            private List<WheelDto> wheelsDto;
            private List<DoorDto> doorsDto;
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
                this.queryParameters = new Dictionary<string, object>();
                this.conditions = new List<string>();
                this.values = new List<List<string>>();
                this.value = new List<string>();
                this.keys = new List<string>();
                this.whereParams = new Dictionary<List<string>, string>();
            }
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {

                        sentence.CommandText = select("SELECT serial, number ", "enrollment").ToString();
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

                this.value.Add("@color");
                this.conditions.Add("color");
                this.values.Add(this.value);
                this.keys.Add("=");

                this.queryParameters.Add("@color", (int)color);
                this.color = color;
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {

                this.value.Add("@engineIsStarted");
                this.conditions.Add("engineIsStarted");
                this.values.Add(this.value);
                this.keys.Add("=");

                this.queryParameters.Add("@engineIsStarted", started);

                this.engineIsStarted = started;
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.value.Add("@number");
                this.conditions.Add("number");
                this.values.Add(this.value);
                this.keys.Add("=");

                this.queryParameters.Add("@number", enrollment.Number);

                this.value.Add("@serial");
                this.conditions.Add("serial");
                this.values.Add(this.value);
                this.keys.Add("=");

                this.queryParameters.Add("@serial", enrollment.Serial);
                this.enrollment = enrollment;
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;

                this.value.Add("@serial");
                this.conditions.Add("serial");
                this.values.Add(this.value);
                this.keys.Add("=");
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.value.Add("@engineHorsePower");
                this.conditions.Add("engineHorsePower");
                this.values.Add(this.value);
                this.keys.Add("=");

                this.queryParameters.Add("@engineHorsePower", horsePower);
                this.horsePower = horsePower;
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);

                this.value.Add("@min");
                this.value.Add("@max");
                this.conditions.Add("engineHorsePower");
                this.values.Add(this.value);
                this.keys.Add("BETWEEN");

                this.queryParameters.Add("@min", min);
                this.queryParameters.Add("@max", max);
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
                        StringBuilder query = new StringBuilder(400);
                        query.Insert(query.Length, SELECT_VEHICLE_HEAD);
                        query.Insert(query.Length, where("enrollment", this.conditions, this.values, this.keys));
                        sentence.CommandText = query.ToString();
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);

                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.id = (int)reader.GetValue(2);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    copyParameters(sentence, sentence2);
                                    this.doorsDto = new List<DoorDto>();
                                    this.wheelsDto = new List<WheelDto>();
                                    this.conditions = new List<string>();
                                    this.values = new List<List<string>>();
                                    this.value = new List<string>();
                                    this.keys = new List<string>();
                                    IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
                                    List<string> values = new List<string>();
                                    values.Add("@id");
                                    whereParams.Add(values, "id");

                                    sentence2.Parameters.Add(setParameter(sentence, "@id", this.id));

                                    this.value.Add("@id");
                                    this.conditions.Add("id");
                                    this.values.Add(this.value);
                                    this.keys.Add("=");

                                    sentence2.CommandText = buildSimpleQuery("SELECT isOpen ", "door", this.conditions, whereParams, this.keys).ToString();
                                    elements(sentence2, "door");
                                    sentence2.CommandText = buildSimpleQuery("SELECT pressure ", "wheel", this.conditions, whereParams, this.keys).ToString();
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
                        {
                            this.doorsDto.Add(new DoorDto(Convert.ToBoolean(reader2.GetValue(0))));
                        }
                        else
                        {
                            this.wheelsDto.Add(new WheelDto(Convert.ToDouble(reader2.GetValue(0))));
                        }
                    }
                }
            }

            StringBuilder select(string command, string table, List<string> conditions = null, List<List<string>> values = null, List<string> keys = null)
            {
                StringBuilder query = new StringBuilder(60);
                query.Insert(query.Length, $"{command} FROM {table}");
                if (conditions != null)
                {
                    query.Insert(query.Length, where(table, conditions, values, keys));
                }
                return query;
            }
            StringBuilder where(string table, List<string> conditions, List<List<string>> values, List<string> keys)
            {
                StringBuilder query = new StringBuilder(100);
                for (int i = 0; i < keys.Count; i++)
                {
                    StringBuilder conditionsp = new StringBuilder(30);
                    if (conditions[i] == "id" && table != "enrollment")
                    {
                        if (table == "vehicle")
                        {
                            conditionsp.Insert(conditionsp.Length, buildCondition("enrollmentId", keys[i], values[i]));
                        }
                        else
                        {
                            conditionsp.Insert(conditionsp.Length, buildCondition("vehicleId", keys[i], values[i]));
                        }
                    }
                    else
                    {
                        conditionsp.Insert(conditionsp.Length, buildCondition(conditions[i], keys[i], values[i]));
                    }

                    if (i == 0 || conditions.Count == 1)
                    {
                        query.Insert(query.Length, $" WHERE {conditionsp}");
                    }
                    else if (i < conditions.Count - 1)
                    {
                        query.Insert(query.Length, $" AND {conditionsp}");
                    }
                }
                return query;
            }
            StringBuilder buildCondition(string condition, string key, List<string> value)
            {
                StringBuilder query = new StringBuilder($"{condition} {key} ", 60);
                if (value.Count !=1)
                {
                    if (key == "BETWEEN")
                    {
                        query.Insert(query.Length, betweenfields(value));
                    }
                    else if (key == "IN")
                    {
                        query.Insert(query.Length, inFields(value));
                    }
                    else
                    {
                        query.Insert(query.Length, value[0]);
                    }
                }
                else
                {
                    query.Insert(query.Length, value[0]);
                }
                return query;
            }
            StringBuilder inFields(List<string> value)
            {
                StringBuilder query = new StringBuilder(60);
                for (int i = 0; i < value.Capacity; i++)
                {
                    query.Insert(query.Length, $"({value[i]} ,");
                    if (i == value.Capacity-1)
                    {
                        query.Insert(query.Length, $")");
                    }
                }
                return query;
            }
            StringBuilder betweenfields(List<string> value)
            {
                StringBuilder query = new StringBuilder(60);
                for (int i = 0; i < value.Capacity; i++)
                {
                    query.Insert(query.Length, value[i]);
                    if (i == 0)
                    {
                        query.Insert(query.Length, $" AND ");
                    }
                }
                return query;
            }


            private static StringBuilder buildSimpleQuery(string command, string table, List<string> values = null, IDictionary<List<string>, string> whereParams = null, List<string> keys = null)
            {
                StringBuilder query = new StringBuilder(60);
                if (command.Contains("SELECT") || command.Contains("DELETE"))
                {
                    query.Insert(query.Length, $"{command} FROM {table}");
                    if (whereParams != null)
                    {
                        query.Insert(query.Length, newwhere(table, whereParams, keys));
                    }
                }
                else if (command.Contains("UPDATE"))
                {
                    query.Insert(query.Length, $"{command}{table} SET ");
                    int counter = 0;
                    foreach (string value in values)
                    {
                        query.Insert(query.Length, $"{value} = @{value}");
                        if (counter < values.Count - 1)
                        {
                            query.Insert(query.Length, ", ");
                        }
                        counter++;
                    }
                    query.Insert(query.Length, newwhere(table, whereParams, keys));
                }
                else if (command.Contains("INSERT"))
                {
                    query.Insert(query.Length, $"{command} {table}");

                    query.Insert(query.Length, addFields(values, table, "condition"));
                    query.Insert(query.Length, " VALUES ");
                    query.Insert(query.Length, addFields(values, table, "value"));

                }
                return query;
            }
            private static StringBuilder addFields(List<string> values, string table, string type)
            {
                StringBuilder query = new StringBuilder(50);
                string thing = "";
                int counter = 0;
                foreach (string value in values)
                {
                    if (type == "value")
                    {
                        thing = $"@{value}";
                    }
                    else
                    {
                        thing = value;
                    }
                    if (counter == 0)
                    {
                        query.Insert(query.Length, $"({checkCondition(thing, table)}");
                        if (values.Count == 1)
                        {
                            query.Insert(query.Length, ")");
                        }
                    }
                    else if (counter < values.Count)
                    {
                        query.Insert(query.Length, $", {checkCondition(thing, table)}");
                        if (counter == values.Count - 1)
                        {
                            query.Insert(query.Length, ") ");
                        }
                    }
                    counter++;
                }
                return query;
            }
            private static string checkCondition(string condition, string table)
            {
                string fix;
                if (condition == "id" && table != "enrollment")
                {
                    if (table == "vehicle")
                        fix = "enrollmentId";
                    else
                        fix = "vehicleId";
                }
                else
                    fix = condition;
                return fix;
            }
            private static StringBuilder newwhere(string table, IDictionary<List<string>, string> whereParams, List<string> keys)
            {
                StringBuilder query = new StringBuilder(100);
                int counter = 0;
                foreach (KeyValuePair<List<string>, string> where in whereParams)
                {
                    if (counter == 0 || whereParams.Count == 1)
                    {
                        query.Insert(query.Length, " WHERE ");
                    }
                    else if (counter > 0)
                    {
                        query.Insert(query.Length, " AND ");
                    }
                    query.Insert(query.Length, newbuildCondition(checkCondition(where.Value, table), keys[counter], where.Key));
                    counter++;
                }
                return query;
            }
            private static StringBuilder newbuildCondition(string condition, string key, List<string> values)
            {
                StringBuilder query = new StringBuilder($"{condition} {key} ", 60);
                if (values.Count != 1)
                {
                    if (key == "BETWEEN")
                    {
                        query.Insert(query.Length, newbetweenfields(values));
                    }
                    else if (key == "IN")
                    {
                        query.Insert(query.Length, newinFields(values));
                    }
                }
                else
                {
                    query.Insert(query.Length, values[0]);
                }
                return query;
            }
            private static StringBuilder newinFields(List<string> value)
            {
                StringBuilder query = new StringBuilder(60);
                for (int i = 0; i < value.Capacity; i++)
                {
                    query.Insert(query.Length, $"({value[i]} ,");
                    if (i == value.Capacity - 1)
                    {
                        query.Insert(query.Length, $")");
                    }
                }
                return query;
            }
            private static StringBuilder newbetweenfields(List<string> value)
            {
                StringBuilder query = new StringBuilder(60);
                for (int i = 0; i < value.Capacity; i++)
                {
                    query.Insert(query.Length, value[i]);
                    if (i == 0)
                    {
                        query.Insert(query.Length, $" AND ");
                    }
                }
                return query;
            }

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
            
        }
    }
}