using System.Collections.Generic;
using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        private readonly IList<FieldValues> fieldValues;
        private readonly IList<string> keys;
        private readonly string table;
        private readonly string column;
        private readonly FieldValues tableValues;

        public QueryBuilder(FieldValues tableValues, IList<FieldValues> fieldValues, IList<string> keys)
        {
            this.tableValues = tableValues;
            this.fieldValues = fieldValues;
            this.keys = keys;
        }
        public QueryBuilder(FieldValues tableValues)
        {
            this.tableValues = tableValues;
        }
        public QueryBuilder(string column, string table)
        {
            this.column = column;
            this.table = table;
        }
        public QueryBuilder(string column, string table, IList<FieldValues> fieldValues, IList<string> keys)
        {
            this.column = column;
            this.table = table;
            this.fieldValues = fieldValues;
            this.keys = keys;
        }
        public QueryBuilder(string table, IList<FieldValues> fieldValues, IList<string> keys)
        {
            this.table = table;
            this.fieldValues = fieldValues;
            this.keys = keys;
        }

        public QueryBuilder()
        {

        }

        public StringBuilder querySelect
        {
            get
            {
                if (this.fieldValues == null)
                {
                    return select(this.column, this.table);
                }
                else
                {
                    return select(this.column, this.table, this.fieldValues, this.keys);
                }

            }
        }
        public StringBuilder queryInsert
        {
            get
            {
                return insert(this.tableValues);
            }
        }
        public StringBuilder queryUpdate
        {
            get
            {
                return update(this.tableValues, this.fieldValues, this.keys);
            }
        }
        public StringBuilder queryDelete
        {
            get
            {
                return delete(this.table, this.fieldValues, this.keys);
            }
        }

        public StringBuilder complexSelect(IList<FieldValues> fieldValues, IDictionary<string, string> joinConditions)
        {
            StringBuilder query = new StringBuilder(100);
            query.Insert(query.Length, "SELECT ");
            int counter = 0;
            foreach (FieldValues fieldvalues in fieldValues)
            {
                for (int i = 0; i < fieldvalues.values.Count; i++)
                {
                    query.Insert(query.Length, $"{fieldvalues.field[0]}.{fieldvalues.values[i]}");
                    if (i < fieldvalues.values.Count - 1)
                        query.Insert(query.Length, ", ");
                }
                if (counter < fieldValues.Count - 1)
                    query.Insert(query.Length, ", ");
                counter++;
            }
            query.Insert(query.Length, " FROM ");
            counter = 0;
            foreach (FieldValues fieldvalues in fieldValues)
            {
                query.Insert(query.Length, $"{fieldvalues.field} {fieldvalues.field[0]}");
                if (counter < fieldValues.Count - 1)
                    query.Insert(query.Length, " INNER JOIN ");
                counter++;
            }
            query.Insert(query.Length, " ON ");
            counter = 0;
            foreach (KeyValuePair<string, string> joinCondition in joinConditions)
            {
                query.Insert(query.Length, $"{joinCondition.Key} = {joinCondition.Value}");
                if (counter < joinConditions.Count - 1)
                    query.Insert(query.Length, " AND ");
                counter++;
            }
            return query;
        }
        private StringBuilder update(FieldValues tableValues, IList<FieldValues> fieldValues, IList<string> keys)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"UPDATE {tableValues.field} SET {addFields(tableValues.values, tableValues.field, "UPDATE")} {where(tableValues.field, fieldValues, keys)}");
            return query;
        }
        private StringBuilder insert(FieldValues tableValues)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"INSERT INTO {tableValues.field} ({addFields(tableValues.values, tableValues.field, "INSERT", "condition")}) VALUES ({addFields(tableValues.values, tableValues.field, "INSERT", "value")})");
            return query;
        }
        private StringBuilder select(string column, string table, IList<FieldValues> fieldValues = null, IList<string> keys = null)
        {
            return selectDelete($"SELECT {column}", table, fieldValues, keys);
        }
        private StringBuilder delete(string table, IList<FieldValues> fieldValues, IList<string> keys)
        {
            return selectDelete("DELETE", table, fieldValues, keys);
        }
        private StringBuilder selectDelete(string command, string table, IList<FieldValues> fieldValues = null, IList<string> keys = null)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{command} FROM {table}");
            if (fieldValues != null)
                query.Insert(query.Length, where(table, fieldValues, keys));
            return query;
        }
        public StringBuilder where(string table, IList<FieldValues> fieldValues, IList<string> keys)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (FieldValues fieldvalues in fieldValues)
            {
                if (counter == 0 || fieldValues.Count == 1)
                    query.Insert(query.Length, " WHERE ");
                else if (counter > 0)
                    query.Insert(query.Length, " AND ");
                query.Insert(query.Length, buildCondition(checkCondition(fieldvalues.field, table), keys[counter], fieldvalues.values));
                counter++;
            }
            return query;
        }
        private static StringBuilder addFields<T>(IList<T> list, string table, string from, string type = null)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (T item in list)
            {
                if (from.Contains("INSERT"))
                    query.Insert(query.Length, element($"{item}", table, type));
                else
                    query.Insert(query.Length, $"{element($"{item}", table, "condition")} = {element($"{item}", table, "value")}");
                if (counter < list.Count - 1)
                    query.Insert(query.Length, ", ");
                counter++;
            }
            return query;
        }
        private static StringBuilder element(string item, string table, string type)
        {
            StringBuilder query = new StringBuilder(30);
            if (type != "condition")
                query.Insert(query.Length, $"@{item}");
            else
                query.Insert(query.Length, checkCondition(item, table));
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
        private static StringBuilder buildCondition(string condition, string key, IList<string> values)
        {
            StringBuilder query = new StringBuilder($"{condition} {key} ", 60);
            if (values.Count != 1)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (key == "IN")
                    {
                        query.Insert(query.Length, $"({values[i]} ,");
                        if (i == values.Count - 1)
                            query.Insert(query.Length, $")");
                    }
                    else
                    {
                        query.Insert(query.Length, values[i]);
                        if (i == 0)
                            query.Insert(query.Length, $" AND ");
                    }
                }
            }
            else
                query.Insert(query.Length, values[0]);
            return query;
        }

    }
}
