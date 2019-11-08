using System.Collections.Generic;
using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        private readonly iQuery iquery;

        public QueryBuilder(iQuery iquery)
        {
            this.iquery = iquery;
        }

        public StringBuilder querySelect
        {
            get
            {
                return select(this.iquery);
            }
        }
        public StringBuilder queryInsert
        {
            get
            {
                return insert(this.iquery);
            }
        }
        public StringBuilder queryUpdate
        {
            get
            {
                return update(this.iquery);
            }
        }
        public StringBuilder queryDelete
        {
            get
            {
                return delete(this.iquery);
            }
        }
        public StringBuilder queryComplexSelect
        {
            get
            {
                return complexSelect(this.iquery);
            }
        }

        private StringBuilder complexSelect(iQuery iQuery)
        {
            StringBuilder query = new StringBuilder(100);
            query.Insert(query.Length, "SELECT ");
            int counter = 0;
            foreach (FieldValues fieldvalues in iQuery.tablesColumns)
            {
                for (int i = 0; i < fieldvalues.values.Count; i++)
                {
                    query.Insert(query.Length, $"{fieldvalues.field[0]}.{fieldvalues.values[i]}");
                    if (i < fieldvalues.values.Count - 1)
                        query.Insert(query.Length, ", ");
                }
                if (counter < iQuery.tablesColumns.Count - 1)
                    query.Insert(query.Length, ", ");
                counter++;
            }
            query.Insert(query.Length, " FROM ");
            counter = 0;
            foreach (FieldValues fieldvalues in iQuery.tablesColumns)
            {
                query.Insert(query.Length, $"{fieldvalues.field} {fieldvalues.field[0]}");
                if (counter < iQuery.tablesColumns.Count - 1)
                    query.Insert(query.Length, " INNER JOIN ");
                counter++;
            }
            query.Insert(query.Length, " ON ");
            counter = 0;
            foreach (FieldValues joinCondition in iQuery.innerValues)
            {
                query.Insert(query.Length, $"{joinCondition.field} = {joinCondition.values[0]}");
                if (counter < iQuery.innerValues.Count - 1)
                    query.Insert(query.Length, " AND ");
                counter++;
            }
            query.Insert(query.Length, where(iQuery));
            return query;
        }
        private StringBuilder update(iQuery iquery)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"UPDATE {iquery.tablesColumns[0].field} SET {addFields(iquery.tablesColumns[0].values, iquery.tablesColumns[0].field, "UPDATE")} {where(iquery)}");
            return query;
        }
        private StringBuilder insert(iQuery iquery)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"INSERT INTO {iquery.tablesColumns[0].field} ({addFields(iquery.tablesColumns[0].values, iquery.tablesColumns[0].field, "INSERT", "condition")}) VALUES ({addFields(iquery.tablesColumns[0].values, iquery.tablesColumns[0].field, "INSERT", "value")})");
            return query;
        }
        private StringBuilder select(iQuery iquery)
        {
            return selectDelete($"SELECT {iquery.tablesColumns[0].values[0]}", iquery);
        }
        private StringBuilder delete(iQuery iquery)
        {
            return selectDelete("DELETE", iquery);
        }

        private StringBuilder selectDelete(string command, iQuery iquery)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{command} FROM {iquery.tablesColumns[0].field}");
            if (iquery.whereValues != null)
                query.Insert(query.Length, where(iquery));
            return query;
        }
        public StringBuilder where(iQuery iquery)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (whereFieldValues wherevalues in iquery.whereValues)
            {
                if (counter == 0 || iquery.whereValues.Count == 1)
                    query.Insert(query.Length, " WHERE ");
                else if (counter > 0)
                    query.Insert(query.Length, " AND ");
                query.Insert(query.Length, buildCondition(checkCondition(wherevalues.field, iquery.tablesColumns[0].field), wherevalues.key, wherevalues.values));
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
