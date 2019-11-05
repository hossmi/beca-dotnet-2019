using System.Collections.Generic;
using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        private readonly string table;
        private readonly string column;
        private readonly FieldValues tableValues;
        private readonly IList<whereFieldValues> whereValues;

        public QueryBuilder(FieldValues tableValues, IList<whereFieldValues> whereValues)
        {
            this.tableValues = tableValues;
            this.whereValues = whereValues;
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
        public QueryBuilder(string column, string table, IList<whereFieldValues> whereValues)
        {
            this.column = column;
            this.table = table;
            this.whereValues = whereValues;
        }
        public QueryBuilder(string table, IList<whereFieldValues> whereValues)
        {
            this.table = table;
            this.whereValues = whereValues;
        }

        public QueryBuilder()
        {

        }

        public StringBuilder querySelect
        {
            get
            {
                if (this.whereValues == null)
                {
                    return select(this.column, this.table);
                }
                else
                {
                    return select(this.column, this.table, this.whereValues);
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
                return update(this.tableValues, this.whereValues);
            }
        }
        public StringBuilder queryDelete
        {
            get
            {
                return delete(this.table, this.whereValues);
            }
        }

        public StringBuilder complexSelect(IList<FieldValues> fieldValues, IList<FieldValues> joinConditions)
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
            foreach (FieldValues joinCondition in joinConditions)
            {
                query.Insert(query.Length, $"{joinCondition.field} = {joinCondition.values[0]}");
                if (counter < joinConditions.Count - 1)
                    query.Insert(query.Length, " AND ");
                counter++;
            }
            return query;
        }
        private StringBuilder update(FieldValues tableValues, IList<whereFieldValues> whereValues)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"UPDATE {tableValues.field} SET {addFields(tableValues.values, tableValues.field, "UPDATE")} {where(tableValues.field, whereValues)}");
            return query;
        }
        private StringBuilder update(iQuery iquery)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"UPDATE {iquery.tablesColumns[0].field} SET {addFields(iquery.tablesColumns[0].values, iquery.tablesColumns[0].field, "UPDATE")} {where(iquery.tablesColumns[0].field, iquery.whereValues)}");
            return query;
        }
        private StringBuilder insert(FieldValues tableValues)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"INSERT INTO {tableValues.field} ({addFields(tableValues.values, tableValues.field, "INSERT", "condition")}) VALUES ({addFields(tableValues.values, tableValues.field, "INSERT", "value")})");
            return query;
        }
        private StringBuilder insert(iQuery iquery)
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"INSERT INTO {iquery.tablesColumns[0].field} ({addFields(iquery.tablesColumns[0].values, iquery.tablesColumns[0].field, "INSERT", "condition")}) VALUES ({addFields(iquery.tablesColumns[0].values, iquery.tablesColumns[0].field, "INSERT", "value")})");
            return query;
        }
        private StringBuilder select(string column, string table, IList<whereFieldValues> whereValues = null)
        {
            return selectDelete($"SELECT {column}", table, this.whereValues);
        }
        private StringBuilder select(iQuery iquery)
        {
            return selectDelete($"SELECT", iquery);
        }
        private StringBuilder delete(string table, IList<whereFieldValues> whereValues)
        {
            return selectDelete("DELETE", table, whereValues);
        }
        private StringBuilder delete(iQuery iquery)
        {
            return selectDelete("DELETE", iquery);
        }

        private StringBuilder selectDelete(string command, string table, IList<whereFieldValues> whereValues = null)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{command} FROM {table}");
            if (this.whereValues != null)
                query.Insert(query.Length, where(table, whereValues));
            return query;
        }
        private StringBuilder selectDelete(string command, iQuery iquery)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{command} {iquery.tablesColumns[0].values[0]} FROM {iquery.tablesColumns[0].field}");
            if (this.whereValues != null)
                query.Insert(query.Length, where(iquery.tablesColumns[0].field, iquery.whereValues));
            return query;
        }
        public StringBuilder where(string table, IList<whereFieldValues> whereValues)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (whereFieldValues wherevalues in whereValues)
            {
                if (counter == 0 || whereValues.Count == 1)
                    query.Insert(query.Length, " WHERE ");
                else if (counter > 0)
                    query.Insert(query.Length, " AND ");
                query.Insert(query.Length, buildCondition(checkCondition(wherevalues.field, table), wherevalues.key, wherevalues.values));
                counter++;
            }
            return query;
        }
        public StringBuilder where(iQuery iquery)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (whereFieldValues wherevalues in iquery.whereValues)
            {
                if (counter == 0 || whereValues.Count == 1)
                    query.Insert(query.Length, " WHERE ");
                else if (counter > 0)
                    query.Insert(query.Length, " AND ");
                query.Insert(query.Length, buildCondition(checkCondition(wherevalues.field, table), wherevalues.key, wherevalues.values));
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
