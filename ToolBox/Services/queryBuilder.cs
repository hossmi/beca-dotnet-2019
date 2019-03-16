using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        private IDictionary<string, List<string>> tablesColumns;
        private IDictionary<string, string> joinConditions;
        private IDictionary<List<string>, string> whereParams;
        private List<string> keys;
        private List<string> values;
        private string table;
        private string column;

        public QueryBuilder(IDictionary<string, List<string>> tablesColumns, IDictionary<string, string> joinConditions)
        {
            this.joinConditions = joinConditions;
            this.tablesColumns = tablesColumns;
        }
        public QueryBuilder(string table, List<string> values, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            this.table = table;
            this.values = values;
            this.whereParams = whereParams;
            this.keys = keys;
        }
        public QueryBuilder(string table, List<string> values)
        {
            this.table = table;
            this.values = values;
        }
        public QueryBuilder(string column, string table)
        {
            this.column = column;
            this.table = table;
        }
        public QueryBuilder(string column, string table, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            this.column = column;
            this.table = table;
            this.whereParams = whereParams;
            this.keys = keys;
        }
        public QueryBuilder(string table, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            this.table = table;
            this.whereParams = whereParams;
            this.keys = keys;
        }
        public QueryBuilder()
        {

        }

        public StringBuilder querySelect
        {
            get
            {
                if (this.whereParams == null)
                {
                    return select(this.column, this.table);
                }
                else
                {
                    return select(this.column, this.table, this.whereParams, this.keys);
                }
                
            }
        }
        public StringBuilder queryInsert
        {
            get
            {
                return insert(this.table, this.values);
            }
        }
        public StringBuilder queryUpdate
        {
            get
            {
                return update(this.table, this.values, this.whereParams, this.keys);
            }
        }
        public StringBuilder queryDelete
        {
            get
            {
                return delete(this.table, this.whereParams, this.keys);
            }
        }

        public StringBuilder complexSelect(IDictionary<string, List<string>> tablesColumns, IDictionary<string, string> joinConditions)
        {
            StringBuilder query = new StringBuilder(100);
            query.Insert(query.Length, "SELECT ");
            int counter = 0;
            foreach (KeyValuePair<string, List<string>> tableColumns in tablesColumns)
            {
                for (int i = 0; i < tableColumns.Value.Count; i++)
                {
                    query.Insert(query.Length, $"{tableColumns.Key[0]}.{tableColumns.Value[i]}");
                    if (i < tableColumns.Value.Count - 1)
                        query.Insert(query.Length, ", ");
                }
                if (counter < tablesColumns.Count - 1)
                    query.Insert(query.Length, ", ");
                counter++;
            }
            query.Insert(query.Length, " FROM ");
            counter = 0;
            foreach (KeyValuePair<string, List<string>> tableColumns in tablesColumns)
            {
                query.Insert(query.Length, $"{tableColumns.Key} {tableColumns.Key[0]}");
                if (counter < tablesColumns.Count - 1)
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
        private StringBuilder update(string table, List<string> values, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"UPDATE {table} SET {addFields(values, table, "UPDATE")} {where(table, whereParams, keys)}");
            return query;
        }
        private StringBuilder insert(string table, List<string> values)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"INSERT INTO {table} ({addFields(values, table, "INSERT", "condition")}) VALUES ({addFields(values, table, "INSERT", "value")})");
            return query;
        }
        private StringBuilder select(string column, string table, IDictionary<List<string>, string> whereParams = null, List<string> keys = null)
        {
            return selectDelete($"SELECT {column}", table, whereParams, keys);
        }
        private StringBuilder delete(string table, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            return selectDelete("DELETE", table, whereParams, keys);
        }
        private StringBuilder selectDelete(string command, string table, IDictionary<List<string>, string> whereParams = null, List<string> keys = null)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{command} FROM {table}");
            if (whereParams != null)
                query.Insert(query.Length, where(table, whereParams, keys));
            return query;
        }
        public StringBuilder where(string table, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (KeyValuePair<List<string>, string> where in whereParams)
            {
                if (counter == 0 || whereParams.Count == 1)
                    query.Insert(query.Length, " WHERE ");
                else if (counter > 0)
                    query.Insert(query.Length, " AND ");
                query.Insert(query.Length, buildCondition(checkCondition(where.Value, table), keys[counter], where.Key));
                counter++;
            }
            return query;
        }
        private static StringBuilder addFields(List<string> list, string table, string from, string type = null)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (string item in list)
            {
                if (from.Contains("INSERT"))
                {
                    query.Insert(query.Length, element(item, table, type));
                }
                else
                    query.Insert(query.Length, $"{element(item, table, "condition")} = {element(item, table, "value")}");
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
        private static StringBuilder buildCondition(string condition, string key, List<string> values)
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
