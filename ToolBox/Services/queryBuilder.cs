using System.Collections.Generic;
using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
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
                    {
                        query.Insert(query.Length, ", ");
                    }
                }
                if (counter < tablesColumns.Count - 1)
                {
                    query.Insert(query.Length, ", ");
                }
                counter++;
            }
            query.Insert(query.Length, " FROM ");
            counter = 0;
            foreach (KeyValuePair<string, List<string>> tableColumns in tablesColumns)
            {
                query.Insert(query.Length, $"{tableColumns.Key} {tableColumns.Key[0]}");
                if (counter < tablesColumns.Count - 1)
                {
                    query.Insert(query.Length, " INNER JOIN ");
                }
                counter++;
            }
            query.Insert(query.Length, " ON ");
            counter = 0;
            foreach (KeyValuePair<string, string> joinCondition in joinConditions)
            {
                query.Insert(query.Length, $"{joinCondition.Key} = {joinCondition.Value}");
                if (counter < joinConditions.Count - 1)
                {
                    query.Insert(query.Length, " AND ");
                }
                counter++;
            }
            return query;
        }
        public StringBuilder update(string table, List<string> values, IDictionary<List<string>, string> whereParams, List<string> keys)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"UPDATE {table} SET ");
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
            return query;
        }
        public StringBuilder insert(string table, List<string> values)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"INSERT INTO {table}");
            query.Insert(query.Length, addFields(values, table, "condition"));
            query.Insert(query.Length, " VALUES ");
            query.Insert(query.Length, addFields(values, table, "value"));
            return query;
        }
        public StringBuilder selectDelete(string command, string table, IDictionary<List<string>, string> whereParams = null, List<string> keys = null)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{command} FROM {table}");
            if (whereParams != null)
            {
                query.Insert(query.Length, where(table, whereParams, keys));
            }
            return query;
        }
        public StringBuilder where(string table, IDictionary<List<string>, string> whereParams, List<string> keys)
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
                        {
                            query.Insert(query.Length, $")");
                        }
                    }
                    else
                    {
                        query.Insert(query.Length, values[i]);
                        if (i == 0)
                        {
                            query.Insert(query.Length, $" AND ");
                        }
                    }
                }
            }
            else
            {
                query.Insert(query.Length, values[0]);
            }
            return query;
        }
    }
}
